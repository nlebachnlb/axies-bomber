using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Collections;
using Spine.Unity;
using DG.Tweening;

public enum EnemyState
{
    Spawn,
    Normal
}

public class SimpleSlimeMovement : MonoBehaviour
{
    public enum MovementAxis
    {
        Horizontal,
        Vertical,
        Both
    }


    public MovementAxis axis;
    public float moveSpeed = 2f;
    public float autoChangeDirectionTime = -1f;
    [SerializeField] private float animSpeedRatio = 0.25f;

    [SerializeField] private float skinRootOffset = 3f;
    [SerializeField] private SkeletonAnimation anim;
    [SerializeField] private ParticleSystem spawnFx;
    [SerializeField] private float spawnTime = 1f;
    [SerializeField] private Transform skinRoot;
    [SerializeField] private AnimationReferenceAsset animIdle, animMove;

    private EnemyState state = EnemyState.Spawn;
    private EnemyController enemyController;
    private Collider colBox;
    private new Rigidbody rigidbody;
    private Vector3 direction;
    private EnemyHealth health;
    private LayerMask layerMask;
    private float timer = 0f;

    private void Awake()
    {
        enemyController = GetComponent<EnemyController>();
        colBox = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();
        health = GetComponent<EnemyHealth>();
        layerMask = ~LayerMask.GetMask("Collectible");
    }

    private IEnumerator Start()
    {
        GenerateDirection();
        skinRoot.Translate(0f, -skinRootOffset, 0f);

        state = EnemyState.Spawn;
        colBox.enabled = false;
        anim.state.SetAnimation(0, animIdle, true);
        spawnFx.Play();
        spawnFx.transform.localScale = Vector3.zero;
        spawnFx.transform.DOScale(1f, 0.2f * spawnTime);
        yield return new WaitForSeconds(0.2f * spawnTime);

        skinRoot.DOLocalMoveY(0f, 0.2f * spawnTime);
        yield return new WaitForSeconds(0.4f * spawnTime);

        spawnFx.transform.DOScale(0f, 0.2f);
        colBox.enabled = true;
        yield return new WaitForSeconds(0.2f * spawnTime);

        state = EnemyState.Normal;
        anim.state.SetAnimation(0, animMove, true).TimeScale = animSpeedRatio * moveSpeed;
        timer = autoChangeDirectionTime;

        yield return new WaitForSeconds(0.2f * spawnTime);
        spawnFx.Stop();
    }

    private void GenerateDirection()
    {
        int dir = Random.Range(0, 100) < 50 ? -1 : 1;
        if (axis == MovementAxis.Horizontal)
            direction = Vector3.right * dir;
        else if (axis == MovementAxis.Vertical)
            direction = Vector3.forward * dir;
        else
            direction = Random.Range(0, 100) < 50 ? Vector3.right * dir : Vector3.forward * dir;

        skinRoot.localScale = new Vector3(-dir, 1f, 1f);
    }

    private void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
                timer = 0;
        }
    }

    private void FixedUpdate()
    {
        if (health.hp <= 0)
        {
            return;
        }

        if (state == EnemyState.Normal)
        {
            Vector3 position = rigidbody.position;
            Vector3 translation = moveSpeed * Time.fixedDeltaTime * direction;

            rigidbody.MovePosition(position + translation);

            Collider[] colliders = Physics.OverlapSphere(transform.position + direction * 0.5f, 0.1f, layerMask);
            if (colliders.Length > 0 || (autoChangeDirectionTime > 0f && timer <= 0f))
            {
                if (axis != MovementAxis.Both)
                {
                    direction *= -1;
                }
                else
                {
                    int dir = Random.Range(0, 100) < 50 ? -1 : 1;
                    if (direction.x != 0)
                        direction = Vector3.forward * dir;
                    else
                        direction = Vector3.right * dir;
                }

                if (direction.x != 0)
                    skinRoot.localScale = new Vector3(-direction.x, 1f, 1f);

                if (autoChangeDirectionTime > 0f)
                    timer = autoChangeDirectionTime;
            }
        }
    }
}
