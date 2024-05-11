using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityHUDv2 : MonoBehaviour
{
    public Animator animator;
    public Image progress;
    public TextMeshProUGUI textProgress;
    public TextMeshProUGUI textAuxilliary;
    public Image mask;

    [SerializeField] private TextMeshProUGUI textDeploymentKey;

    [ShowInInspector, HideInEditorMode]
    public AxieAbility currentAbility { get; private set; }

    public void Assign(SkillType skillType, AxieAbility axieAbility)
    {
        Clear();
        currentAbility = axieAbility;

        if (AppRoot.Instance.Config.inputSettings.skillDeploymentKeys.TryGetValue(skillType, out var keyCode))
            textDeploymentKey.text = keyCode.ToString();

        if (currentAbility.IsPassive())
        {
            gameObject.AddComponent<AbilityPassive>();
        }
        else
        {
            gameObject.AddComponent<AbilityActive>();
        }

        if (currentAbility is IEnemyKillTrackBehaviour)
            gameObject.AddComponent<KilledEnemiesCounter>();
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

    private void Clear()
    {
        var components = GetComponents<AbilityHUDComponent>();
        for (int i = 0; i < components.Length; ++i)
            Destroy(components[i]);
    }
}
