using Ability.Component;
using Spine.Unity;
using UnityEngine;

public class SneakySketch : AxieAbility<SneakySketchStats>
{
    [SerializeField] private Timer timer;

    public Timer Timer => timer;
    private SkeletonAnimation skeletonAnimation;
    private new Collider collider;

    private bool isDeployed;
    private float effectTimer = 0;

    public override void Init(AbilityController controller)
    {
        base.Init(controller);

        skeletonAnimation = Owner.GetComponent<AxieConfigReader>().Skin;
        collider = Owner.GetComponent<Collider>();
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
            timer.StartCountdown(Stats.cooldown);
        }
    }

    public override bool CanDeploy()
    {
        return timer.IsAvailable;
    }

    public override void DeployAbility()
    {
        isDeployed = true;
        effectTimer = Stats.effectDuration;

        skeletonAnimation.skeleton.A = 0.2f;
        collider.enabled = false;
    }
}