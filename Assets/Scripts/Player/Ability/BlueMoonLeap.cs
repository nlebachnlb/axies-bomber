using System.Collections;
using System.Collections.Generic;
using Ability.Component;
using UnityEngine;

public class BlueMoonLeap : AxieAbility<BlueMoonLeapStats>
{
    [SerializeField] private Cooldown cooldown;
    [SerializeField] private BlueMoonLeapStats defaultStats;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private ParticleSystem landingAreaEffect;

    public bool IsJumpable { get; private set; } = false;
    public Vector3 JumpTarget { get; private set; }

    private MovementController movementController;
    private JumpController jumpController;

    private void Awake()
    {
        Stats = Instantiate(defaultStats);

        cooldown.OnStartCountdown += Raise;
        cooldown.OnCooldownFinished += Raise;
    }

    public override void AssignOwner(GameObject owner)
    {
        base.AssignOwner(owner);

        movementController = Owner.GetComponent<MovementController>();
        jumpController = Owner.GetComponent<JumpController>();
    }

    private void Update()
    {
        if (!cooldown.IsAvailable || jumpController.IsJumping)
        {
            IsJumpable = false;
            return;
        }

        var direction = movementController.LastDirection;
        var position = Owner.transform.position;

        var adjacentTile = position + direction + Vector3.up * 0.5f;
        var nextTile = position + direction * 2 + Vector3.up * 0.5f;

        var halfExtents = Vector3.one * 0.5f;
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
        return IsJumpable && cooldown.IsAvailable;
    }

    public override void DeployAbility()
    {
        jumpController.Jump(JumpTarget);
        landingAreaEffect.gameObject.SetActive(false);
        IsJumpable = false;
        cooldown.StartCountdown();
    }

    private void Raise()
    {
        RaiseOnCooldown(cooldown.CooldownValue - cooldown.RemainingTime, cooldown.CooldownValue);
    }
}
