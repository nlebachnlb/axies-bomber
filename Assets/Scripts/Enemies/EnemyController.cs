using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Collections;
using Spine.Unity;
using Sirenix.OdinInspector;

public class EnemyController : MonoBehaviour
{
    public System.Action onDeath;
    [SerializeField] private SkeletonAnimation anim;

    [ShowInInspector, HideInEditorMode]
    public int Coin { get; set; }

    private new Collider collider;

    private void Awake()
    {
        collider = GetComponent<Collider>();
    }

    public void OnDeath()
    {
        collider.enabled = false;
        onDeath?.Invoke();
        EventBus.RaiseOnEnemyDeath();
        Destroy(gameObject);
    }

    public void AddForce(Vector3 force, float stuntDuration)
    {
        var movement = GetComponent<SimpleSlimeMovement>();
        var rigidbody = GetComponent<Rigidbody>();
        if (movement != null && rigidbody != null)
        {
            movement.enabled = false;
            collider.isTrigger = false;
            rigidbody.AddForce(force, ForceMode.VelocityChange);
            Invoke(nameof(Reenable), stuntDuration);
        }
    }

    private void Reenable()
    {
        if (TryGetComponent<SimpleSlimeMovement>(out var movement))
            movement.enabled = true;

        collider.isTrigger = true;
    }
}
