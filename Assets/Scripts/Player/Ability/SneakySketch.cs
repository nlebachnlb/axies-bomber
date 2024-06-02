using System.Collections;
using System.Collections.Generic;
using Ability.Component;
using Spine;
using Spine.Unity;
using UnityEngine;

public class SneakySketch : AxieAbility<SneakySketchStats>
{
    [SerializeField] private SneakySketchStats defaultStats;
    [SerializeField] private Cooldown cooldown;

    private SkeletonAnimation skeletonAnimation;
    private new Collider collider;

    private bool isDeployed;
    private float effectTimer = 0;

    private void Awake()
    {
        Stats = Instantiate(defaultStats);
    }

    public override void AssignOwner(GameObject owner)
    {
        base.AssignOwner(owner);
        skeletonAnimation = owner.GetComponent<AxieConfigReader>().Skin;
        collider = owner.GetComponent<Collider>();
    }

    private void Update()
    {
        if (!isDeployed)
            return;

        effectTimer -= Time.deltaTime;
        if (effectTimer <= 0)
        {
            isDeployed = false;
            effectTimer = 0;
            skeletonAnimation.skeleton.A = 1;
            collider.enabled = true;
            cooldown.StartCountdown();
        }
    }

    public override bool CanDeploy()
    {
        return cooldown.IsAvailable;
    }

    public override void DeployAbility()
    {
        isDeployed = true;
        effectTimer = Stats.effectDuration;

        skeletonAnimation.skeleton.A = 0.2f;
        collider.enabled = false;
    }
}
