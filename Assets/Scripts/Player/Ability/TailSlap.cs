using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailSlap : AxieAbility<TailSlapStats>
{
    [SerializeField] private ParticleSystem fx;
    [SerializeField] private TailSlapStats defaultStats;
    [SerializeField] private Bomb bombPrefab;

    private int placedBombs;
    private AxieHeroData axieData;

    private void Awake()
    {
        Stats = Instantiate(defaultStats);
        EventBus.onBombPlace += OnBombPlace;
    }

    private void OnDestroy()
    {
        EventBus.onBombPlace -= OnBombPlace;
        axieData.SetExtraParam("placedBombs", placedBombs);
    }

    private void Start()
    {
    }

    public override void SetExtraParams(AxieHeroData axieHero)
    {
        base.SetExtraParams(axieHero);
        if (axieHero.ability != null)
            Stats = (TailSlapStats)Instantiate(axieHero.ability);
        placedBombs = (int)axieHero.GetExtraParam("placedBombs", Stats.placedBombsNeeded);
        axieData = axieHero;
        RaiseOnCooldown(placedBombs, Stats.placedBombsNeeded);
        EventBus.RaiseOnAbilityCooldown(placedBombs, Stats.placedBombsNeeded, 1);
        Debug.Log("Extra param: " + placedBombs);
    }

    public override void DeployAbility()
    {
        Debug.Log("TailSlap activated");
        BombController bombController = Owner.GetComponent<BombController>();
        MovementController movement = Owner.GetComponent<MovementController>();

        Vector3 position = transform.position;
        position.x = Mathf.Round(position.x);
        position.z = Mathf.Round(position.z);

        Collider[] colliders = Physics.OverlapBox(position, Vector3.one * 0.25f, Quaternion.identity, bombController.bombLayerMask);
        if (colliders.Length > 0)
        {
            return;
        }

        bombController.axieHeroData.bombsRemaining--;

        Bomb bomb = Instantiate(bombPrefab, position, Quaternion.identity);
        bomb.bombOwner = bombController.axieHeroData;

        bomb.bombFuseTime = bombController.axieHeroData.bombStats.bombFuseTime;
        bomb.explosionLength = bombController.axieHeroData.axieStats.Calculate().bombExplosionRadius;
        bomb.LoadSkin(bombController.config.Axie.bombSprite);
        bomb.color = bombController.config.Axie.auraColor;
        bomb.OnBombFuse = () =>
        {
            bomb.bombOwner.bombsRemaining++;
        };
        bomb.SetMoving(movement.LastDirection * defaultStats.speed);

        placedBombs = 0;
        RaiseOnCooldown(placedBombs, Stats.placedBombsNeeded);
        EventBus.RaiseOnAbilityCooldown(placedBombs, Stats.placedBombsNeeded, 1);
    }

    public override bool CanDeploy()
    {
        return placedBombs >= Stats.placedBombsNeeded;
    }

    private void OnBombPlace(AxieHeroData owner)
    {
        placedBombs++;
        RaiseOnCooldown(placedBombs, Stats.placedBombsNeeded);
        EventBus.RaiseOnAbilityCooldown(placedBombs, Stats.placedBombsNeeded, 1);
        Debug.Log("Place bomb: " + placedBombs);
    }
}