using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine.Collections;
using Spine;

public class HealthController : MonoBehaviour
{
    private AxieConfigReader config;
    private StatsModifier stats;
    private int health;
    private float recoveryTime = 0f;

    [SerializeField] private SkeletonAnimation skeleton;

    public void TakeDamage(int damage)
    {
        if (recoveryTime > 0f)
        {
            return;
        }

        health -= damage;
        if (health <= 0)
        {
            // Game over flow here
        }
        else
        {
            Debug.Log("Hit by " + damage + " damage");
            skeleton.state.SetAnimation(0, config.Axie.animHit, false);
            skeleton.state.Complete += (TrackEntry trackEntry) =>
            {
                if (!trackEntry.Loop && trackEntry.Next == null)
                {
                    skeleton.state.SetAnimation(0, config.Axie.animIdle, true);
                }
            };
        }

        recoveryTime = stats.axieStats.recoveryTimeAfterDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            TakeDamage(1);
        }
    }

    private void Awake()
    {
        config = GetComponent<AxieConfigReader>();
        stats = GetComponent<StatsModifier>();

    }

    private void Update()
    {
        if (recoveryTime > 0f)
        {
            recoveryTime -= Time.deltaTime;
        }
    }

    private void Start()
    {
        health = stats.axieStats.health;
        recoveryTime = 0f;
    }
}
