using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SkillPoolEntrance : MonoBehaviour
{
    [SerializeField] private Transform visual;
    [SerializeField] private Light auraLight;
    public bool isAbilityPool = false;

    public void PlayAuraLightIntensity()
    {
        auraLight.DOIntensity(12f, 0.2f).OnComplete(() =>
        {
            auraLight.DOIntensity(3f, 0.8f);
        });
    }

    private void Awake()
    {
        EventBus.onPickSkill += OnPickSkill;
        EventBus.onEnterSkillPool += OnEnterSkillPool;
    }

    private void Start()
    {
        visual.localScale = Vector3.zero;
        visual.DOScale(1f, 0.25f).SetEase(Ease.OutBack);
        PlayAuraLightIntensity();
    }

    private void OnDestroy()
    {
        EventBus.onPickSkill -= OnPickSkill;
        EventBus.onEnterSkillPool -= OnEnterSkillPool;
    }

    private void OnPickSkill(SkillConfig config)
    {
        GetComponent<Collider>().enabled = false;
        transform.DOScaleX(0f, 0.35f).SetEase(Ease.InBack);
        transform.DOScaleZ(0f, 0.35f).SetEase(Ease.InBack);
        Destroy(gameObject, 0.35f);
    }

    private void OnEnterSkillPool(List<SkillConfig> skills)
    {
        PlayAuraLightIntensity();
    }
}
