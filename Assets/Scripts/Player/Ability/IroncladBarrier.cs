using System.Collections;
using System.Collections.Generic;
using Ability.Component;
using UnityEngine;

public class IroncladBarrier : AxieAbility<IroncladBarrierStats>
{
    [SerializeField] private IroncladBarrierStats defaultStats;
    [SerializeField] private Cooldown cooldown;

    private JumpController jumpController;

    private void Awake()
    {
        Stats = Instantiate(defaultStats);
    }

    public override void AssignOwner(GameObject owner)
    {
        base.AssignOwner(owner);
        jumpController = owner.GetComponent<JumpController>();
    }

    public override bool CanDeploy()
    {
        return cooldown.IsAvailable;
    }

    public override void DeployAbility()
    {
        cooldown.StartCountdown();
        jumpController.Jump(Owner.transform.position, 1, 0.25f, onCompleted: PerformPush);
    }

    private void PerformPush()
    {
        var origin = Owner.transform.position;
        var results = Physics.OverlapSphere(origin, Stats.EffectRadius);
        if (results != null && results.Length > 0)
        {
            foreach (var result in results)
            {
                if (result.transform.CompareTag(Tag.ENEMY) && result.TryGetComponent<EnemyController>(out var enemyController))
                {
                    var force = (result.transform.position - origin).normalized * 10;
                    enemyController.AddForce(force, 2f);
                }
            }
        }
    }
}
