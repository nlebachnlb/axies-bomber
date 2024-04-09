using UnityEngine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class MovementController : MonoBehaviour
{
    [Header("Input")]
    public KeyCode inputUp = KeyCode.W;
    public KeyCode inputDown = KeyCode.S;
    public KeyCode inputLeft = KeyCode.A;
    public KeyCode inputRight = KeyCode.D;

    [Header("Visual")]
    public SkeletonAnimation characterAnimation;
    public AxieConfigReader config;

    public Vector3 LastDirection { get; set; } = Vector3.right;

    private new Rigidbody rigidbody;
    private Vector3 direction = Vector3.right;
    private string currentState = "idle";
    private float facing = 1;
    private AxieStats stats;

    private List<KeyCode> movementInput;
    private List<bool> pressedKeys;
    private List<System.Action> moveActions;
    private Stack<int> inputStack;

    private bool isInteract = false;
    private SkillPoolEntrance pool;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        config = GetComponent<AxieConfigReader>();

        EventBus.onSwitchAxieHero += OnSwitchHero;
        EventBus.onPickSkill += OnPickSkill;
        EventBus.onGameOver += OnGameOver;
        InitInput();
    }

    private void OnDestroy()
    {
        EventBus.onSwitchAxieHero -= OnSwitchHero;
        EventBus.onPickSkill -= OnPickSkill;
        EventBus.onGameOver -= OnGameOver;
    }

    private void Update()
    {
        UpdateInput();
        int pressedKeyIndex = GetPressedKeyIndex();
        if (pressedKeyIndex != -1)
        {
            moveActions[pressedKeyIndex]();
        }
        else
        {
            SetDirection(Vector3.zero);
        }

        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        Vector3 position = rigidbody.position;
        Vector3 translation = direction * stats.Calculate().speed * Time.fixedDeltaTime;

        rigidbody.MovePosition(position + translation);
    }

    private void SetDirection(Vector3 newDirection)
    {
        direction = newDirection;

        if (direction.x != 0)
        {
            facing = direction.x;
        }

        if (direction != Vector3.zero)
        {
            LastDirection = direction;
        }

        characterAnimation.gameObject.transform.localScale = new Vector3(-facing, 1f, 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MapChanger"))
        {
            Debug.Log("Change map");
            Destroy(other.gameObject);
            string nextMap = other.GetComponent<MapChanger>().targetMapId;
            EventBus.RaiseOnMapChange(nextMap);
        }

        if (other.CompareTag("SkillPool"))
        {
            other.GetComponent<SkillPoolEntrance>().DisplayInteract(true);
            pool = other.GetComponent<SkillPoolEntrance>();
            isInteract = true;
        }

        if (other.CompareTag("Collectible") && other.TryGetComponent<Collectible>(out Collectible collectible))
        {
            PickCollectible(collectible);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SkillPool"))
        {
            other.GetComponent<SkillPoolEntrance>().DisplayInteract(false);
            isInteract = false;
            pool = null;
        }
    }

    private void InitInput()
    {
        movementInput = new List<KeyCode>()
        {
            inputUp, inputDown, inputLeft, inputRight
        };

        pressedKeys = new List<bool>()
        {
            false, false, false, false
        };

        moveActions = new List<System.Action>()
        {
            () => SetDirection(Vector3.forward),
            () => SetDirection(Vector3.back),
            () => SetDirection(Vector3.left),
            () => SetDirection(Vector3.right)
        };

        inputStack = new Stack<int>();
    }

    public void ResetInput()
    {
        for (int i = 0; i < pressedKeys.Count; ++i)
            pressedKeys[i] = false;

        inputStack.Clear();
    }

    private void UpdateInput()
    {
        if (Input.GetKeyDown(KeyCode.E) && isInteract)
        {
            SkillPoolEntrance entrance = pool;
            EventBus.RaiseOnOpenSkillPool(entrance.isAbilityPool);
            SetDirection(Vector3.zero);
            ResetInput();
            enabled = false;
        }

        for (int i = 0; i < movementInput.Count; ++i)
        {
            if (Input.GetKeyDown(movementInput[i]) && !pressedKeys[i])
            {
                for (int j = 0; j < pressedKeys.Count; ++j)
                {
                    if (pressedKeys[j])
                    {
                        pressedKeys[j] = false;
                        inputStack.Push(j);
                    }
                }

                pressedKeys[i] = true;
                break;
            }

            if (Input.GetKeyUp(movementInput[i]))
            {
                if (pressedKeys[i])
                    pressedKeys[i] = false;

                if (inputStack.Count > 0)
                {
                    int lastInput = inputStack.Pop();
                    if (GetPressedKeyIndex() == -1 && Input.GetKey(movementInput[lastInput]))
                        pressedKeys[lastInput] = true;
                }
            }
        }
    }

    private int GetPressedKeyIndex()
    {
        for (int i = 0; i < movementInput.Count; ++i)
        {
            if (pressedKeys[i])
                return i;
        }

        return -1;
    }

    private void UpdateAnimation()
    {
        if (direction != Vector3.zero)
        {
            SetState("move");
        }
        else
        {
            SetState("idle");
        }
    }

    private void DeathSequence()
    {
        enabled = false;
        Invoke(nameof(OnDeathSequenceEnded), 1.25f);
    }

    private void OnDeathSequenceEnded()
    {
        gameObject.SetActive(false);
    }

    private void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        characterAnimation.state.SetAnimation(0, animation, loop).TimeScale = timeScale;
    }

    private void SetState(string state)
    {
        if (currentState.Equals(state))
        {
            return;
        }

        if (state.Equals("idle"))
        {
            SetAnimation(config.Axie.animIdle, true, 1f);
        }
        else if (state.Equals("move"))
        {
            SetAnimation(config.Axie.animRun, true, 0.25f * stats.speed);
        }

        currentState = state;
    }

    private void PickCollectible(Collectible collectible)
    {
        GameObject floatingText = Instantiate(AppRoot.Instance.Config.floatingText);
        floatingText.transform.position = collectible.transform.position;

        TextMeshPro text = floatingText.GetComponent<TextMeshPro>();
        text.text = $"+{collectible.Amount} <sprite index=0>";

        AppRoot.Instance.UserDataModel.Collect(collectible);
        EventBus.RaiseOnPickCollectible(collectible);

        Destroy(collectible.gameObject);
    }

    private void OnSwitchHero(AxieHeroData heroData)
    {
        stats = heroData.axieStats.Calculate();
        currentState = "";
    }

    private void OnPickSkill(SkillConfig skill)
    {
        enabled = true;
    }

    private void OnGameOver()
    {
        characterAnimation.gameObject.SetActive(false);
        enabled = false;
    }
}
