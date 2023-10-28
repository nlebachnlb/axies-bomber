using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine.Collections;
using Spine;

public class HealthController : MonoBehaviour
{
    private AxieConfigReader config;
    private AxieHeroData axieHeroData;
    private float recoveryTime = 0f;
    private Animator animator;

    [SerializeField] private SkeletonAnimation skeleton;
    [SerializeField] private ParticleSystem death;

    public void TakeDamage(int damage)
    {
        if (recoveryTime > 0f)
        {
            return;
        }

        axieHeroData.health -= damage;
        if (axieHeroData.health <= 0)
        {
            death.Play();
            EventBus.RaiseOnAxieHeroDeath(axieHeroData);
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

            AppRoot.Instance.SoundManager.PlayAudio(SoundManager.AudioType.BeingHitType);
        }

        recoveryTime = axieHeroData.axieStats.recoveryTimeAfterDamage;
    }

    public void SetInvincible(float duration)
    {
        recoveryTime = duration;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            TakeDamage(1);
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }

    private void Awake()
    {
        config = GetComponent<AxieConfigReader>();
        animator = GetComponent<Animator>();
        EventBus.onSwitchAxieHero += OnSwitchHero;
    }

    private void OnDestroy()
    {
        EventBus.onSwitchAxieHero -= OnSwitchHero;
    }

    private void Update()
    {
        if (recoveryTime > 0f)
        {
            recoveryTime -= Time.deltaTime;
        }

        animator.SetFloat("RecoveryTime", recoveryTime);
    }

    private void Start()
    {
        recoveryTime = 0f;
    }

    private void OnSwitchHero(AxieHeroData heroData)
    {
        axieHeroData = heroData;
    }
}
