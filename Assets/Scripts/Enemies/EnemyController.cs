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
    //[SerializeField] private AnimationReferenceAsset animDie;

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
}
