using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientHunter : AxieAbility<PatientHunterStats>
{
    [SerializeField] private ParticleSystem fx;
    [SerializeField] private PatientHunterStats defaultStats;

    private void Awake()
    {
        Stats = Instantiate(defaultStats);
    }

    public override void DeployAbility()
    {
        Debug.Log("PatientHunter activated");
        if (healthController == null)
            healthController = Owner.GetComponent<HealthController>();

        StartCoroutine(DeployProgress());
    }

    public override bool CanDeploy()
    {
        return true;
    }

    private IEnumerator DeployProgress()
    {
        healthController.SetInvincible(Stats.dismissDuration);
        fx.Play();
        yield return new WaitForSeconds(Stats.dismissDuration);
        fx.Stop();
    }

    private HealthController healthController;
}