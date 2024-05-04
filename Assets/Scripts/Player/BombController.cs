using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombController : MonoBehaviour
{
    [Header("Bomb")]
    public KeyCode inputKey = KeyCode.Space;
    public Bomb bombPrefab;
    public LayerMask bombLayerMask;

    [Header("Explosion")]
    public LayerMask explosionLayerMask;

    public AxieHeroData axieHeroData;
    public AxieConfigReader config;

    private void Awake()
    {
        config = GetComponent<AxieConfigReader>();

        EventBus.onSwitchAxieHero += OnSwitchHero;
        EventBus.onEnterSkillPool += OnEnterSkillPool;
        EventBus.onPickSkill += OnPickSkill;
    }

    private void OnDestroy()
    {
        EventBus.onSwitchAxieHero -= OnSwitchHero;
        EventBus.onEnterSkillPool -= OnEnterSkillPool;
        EventBus.onPickSkill -= OnPickSkill;
    }

    private void Start()
    {
        //axieHeroData.bombsRemaining = stats.axieStats.bombMagazine;
    }

    private void Update()
    {
        if (axieHeroData.bombsRemaining > 0 && (Input.GetKeyDown(inputKey) || Input.GetMouseButtonDown(0)))
        {
            PlaceBomb();
        }
    }

    private void PlaceBomb()
    {
        Vector3 position = transform.position;
        position.x = Mathf.Round(position.x);
        position.z = Mathf.Round(position.z);

        Collider[] colliders = Physics.OverlapBox(position, Vector3.one * 0.25f, Quaternion.identity, bombLayerMask);
        if (colliders.Length > 0)
        {
            return;
        }

        axieHeroData.bombsRemaining--;

        Bomb bomb = Instantiate(bombPrefab, position, Quaternion.identity);
        bomb.SetOwner(axieHeroData);

        bomb.bombFuseTime = axieHeroData.bombStats.bombFuseTime;
        bomb.explosionLength = axieHeroData.axieStats.Calculate().bombExplosionRadius;
        bomb.LoadSkin(config.Axie.bombSprite);
        bomb.color = config.Axie.auraColor;
        bomb.OnBombFuse = () =>
        {
            bomb.bombOwner.bombsRemaining++;
        };

        AppRoot.Instance.SoundManager.PlayAudio(SoundManager.AudioType.BombSetType);
        EventBus.RaiseOnBombPlace(bomb.bombOwner);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            other.isTrigger = false;
        }
    }

    private void OnSwitchHero(AxieHeroData heroData)
    {
        axieHeroData = heroData;
    }

    private void OnEnterSkillPool(List<SkillConfig> skills)
    {
        enabled = false;
    }

    private void OnPickSkill(SkillConfig skill)
    {
        enabled = true;
    }
}
