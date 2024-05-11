using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPassive : AbilityHUDComponent
{
    public GameObject mask;
    public Animator animator;

    protected override void Awake()
    {
        base.Awake();

        mask = hud.mask.gameObject;
        animator = hud.animator;
    }

    private void OnEnable()
    {
        mask.SetActive(false);
        animator.SetTrigger("Charged");
    }

    private void OnDisable()
    {
        mask.SetActive(true);
        animator.SetTrigger("Cooldown");
    }
}
