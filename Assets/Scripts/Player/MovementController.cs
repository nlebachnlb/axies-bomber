using UnityEngine;
using Spine.Unity;

[RequireComponent(typeof(Rigidbody))]
public class MovementController : MonoBehaviour
{
    public float speed = 5f;

    [Header("Input")]
    public KeyCode inputUp = KeyCode.W;
    public KeyCode inputDown = KeyCode.S;
    public KeyCode inputLeft = KeyCode.A;
    public KeyCode inputRight = KeyCode.D;

    [Header("Visual")]
    public SkeletonAnimation characterAnimation;
    public AnimationReferenceAsset idle, move;

    private new Rigidbody rigidbody;
    private Vector3 direction = Vector3.right;
    private string currentState = "idle";
    private float facing = 1;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
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
    }

    private void FixedUpdate()
    {
        Vector3 position = rigidbody.position;
        Vector3 translation = direction * speed * Time.fixedDeltaTime;

        rigidbody.MovePosition(position + translation);
    }

    private void SetDirection(Vector3 newDirection)
    {
        direction = newDirection;

        if (newDirection != Vector3.zero)
        {
            SetState("move");
        }
        else
        {
            SetState("idle");
        }

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
            SetAnimation(idle, true, 1f);
        }
        else if (state.Equals("move"))
        {
            SetAnimation(move, true, 0.5f * speed);
        }

        currentState = state;
    }
}
