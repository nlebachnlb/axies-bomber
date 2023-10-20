using UnityEngine;
using Spine.Unity;

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

    private new Rigidbody rigidbody;
    private Vector3 direction = Vector3.right;
    private string currentState = "idle";
    private float facing = 1;
    private StatsModifier stats;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        stats = GetComponent<StatsModifier>();
        config = GetComponent<AxieConfigReader>();
    }

    private void Update()
    {
        if (Input.GetKey(inputUp)) {
            SetDirection(Vector3.forward);
        } else if (Input.GetKey(inputDown)) {
            SetDirection(Vector3.back);
        } else if (Input.GetKey(inputLeft)) {
            SetDirection(Vector3.left);
        } else if (Input.GetKey(inputRight)) {
            SetDirection(Vector3.right);
        } else {
            SetDirection(Vector3.zero);
        }

        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        Vector3 position = rigidbody.position;
        Vector3 translation = direction * stats.axieStats.speed * Time.fixedDeltaTime;

        rigidbody.MovePosition(position + translation);
    }

    private void SetDirection(Vector3 newDirection)
    {
        direction = newDirection;

        if (direction.x != 0)
            facing = direction.x;

        characterAnimation.gameObject.transform.localScale = new Vector3(-facing, 1f, 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.layer == LayerMask.NameToLayer("Explosion")) {
        //    DeathSequence();
        //}
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
            SetAnimation(config.Axie.animRun, true, 0.25f * stats.axieStats.speed);
        }

        currentState = state;
    }
}
