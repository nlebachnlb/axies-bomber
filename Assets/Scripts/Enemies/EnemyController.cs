using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Collections;
using Spine.Unity;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation anim;
    //[SerializeField] private AnimationReferenceAsset animDie;

    private Collider collider;

    private void Awake()
    {
        collider = GetComponent<Collider>();    
    }

    public void OnDeath()
    {
        collider.enabled = false;
        Destroy(gameObject);
    }
}
