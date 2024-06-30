using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Ability.UI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySlot : MonoBehaviour
{
    public Animator animator;
    public Image progress;
    public TextMeshProUGUI textProgress;
    public TextMeshProUGUI textAuxilliary;
    public Image mask;
    public Image card;
    public TextMeshProUGUI textDeploymentKey;

    [ShowInInspector, HideInEditorMode]
    public AxieAbility currentAbility { get; private set; }

    public void Assign(SkillType skillType, AxieAbility axieAbility)
    {
        Clear();
        currentAbility = axieAbility;

        if (AppRoot.Instance.Config.inputSettings.skillDeploymentKeys.TryGetValue(skillType, out var keyCode))
            textDeploymentKey.text = keyCode.ToString();

        card.sprite = axieAbility.GetStats().targetAxie;
        AssignUiComponents();
    }

    public void SetNotAvailable()
    {
        Clear();
        textProgress.text = "";
        mask.gameObject.SetActive(true);
        progress.fillAmount = 0;
        textDeploymentKey.text = "-";
    }

    private void OnDestroy()
    {
        Clear();
    }

    private void AssignUiComponents()
    {
        if (currentAbility.IsPassive())
        {
            gameObject.AddComponent<Passive>();
        }
        else
        {
            gameObject.AddComponent<Active>();
        }

        if (currentAbility is IEnemyKillTrackBehaviour)
            gameObject.AddComponent<KilledEnemiesCounter>();

        if (currentAbility.TryGetComponent<Ability.Component.Cooldown>(out var _))
            gameObject.AddComponent<Cooldown>();
    }

    private void Clear()
    {
        var components = GetComponents<AbilityUIComponent>();
        for (int i = 0; i < components.Length; ++i)
            Destroy(components[i]);
    }
}
