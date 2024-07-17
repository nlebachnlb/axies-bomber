using System.Collections;
using System.Collections.Generic;
using Ability.Component;
using UnityEngine;

public class BlueMoonLeap : AxieAbility<BlueMoonLeapStats>
{
    [SerializeField] private Timer timer;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private ParticleSystem landingAreaEffect;

    public bool IsJumpable { get; private set; } = false;
    public Vector3 JumpTarget { get; private set; }
    public bool IsInCooldown => !timer.IsAvailable;
    public Timer Timer => timer;

    private MovementController movementController;
    private JumpController jumpController;

    protected override void Awake()
    {
        base.Awake();

        timer.OnStartCountdown += Raise;
        timer.OnCooldownFinished += Raise;
    }

    public override void Init(AbilityController controller)
    {
        base.Init(controller);

        movementController = controller.Owner.GetComponent<MovementController>();
        jumpController = controller.Owner.GetComponent<JumpController>();
    }

    private void Update()
    {
        if (!timer.IsAvailable || jumpController.IsJumping)
        {
            IsJumpable = false;
            return;
        }

        var direction = movementController.LastDirection;
        var position = Owner.transform.position;

        var adjacentTile = position + direction + Vector3.up * 0.5f;
        var nextTile = position + direction * 2 + Vector3.up * 0.5f;

        var halfExtents = Vector3.one * 0.45f;
        if (!Physics.CheckBox(adjacentTile, Vector3.one * 0.1f, Quaternion.identity, layerMask))
        {
            IsJumpable = false;
            return;
        }

        IsJumpable = !Physics.CheckBox(nextTile, halfExtents, Quaternion.identity, layerMask);
        JumpTarget = IsJumpable ? nextTile - Vector3.up * 0.5f : Vector3.zero;
    }

    private void LateUpdate()
    {
        landingAreaEffect.gameObject.SetActive(IsJumpable);

        if (IsJumpable)
            landingAreaEffect.transform.position = JumpTarget;

        if (IsJumpable && !landingAreaEffect.isPlaying)
        {
            landingAreaEffect.Play();
        }
        else if (!IsJumpable && landingAreaEffect.isPlaying)
        {
            landingAreaEffect.Stop();
        }
    }

    public override bool CanDeploy()
    {
        return IsJumpable && timer.IsAvailable;
    }

    public override void DeployAbility()
    {
        jumpController.Jump(JumpTarget, 1.5f, 0.5f);
        landingAreaEffect.gameObject.SetActive(false);
        IsJumpable = false;
        timer.StartCountdown(4);
    }

    private void Raise()
    {
        RaiseOnCooldown(timer.CurrentCooldownValue - timer.RemainingTime, timer.CurrentCooldownValue);
    }
}
