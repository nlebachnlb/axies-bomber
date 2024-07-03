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

    [ShowInInspector, HideInEditorMode] public AxieAbility currentAbility { get; private set; }

    private bool isActivated = false;

    private void OnDestroy()
    {
        Clear();
    }

    public void Assign(SkillType skillType, AxieAbility axieAbility)
    {
        Clear();
        currentAbility = axieAbility;

        if (AppRoot.Instance.Config.inputSettings.skillDeploymentKeys.TryGetValue(skillType, out var keyCode))
            textDeploymentKey.text = keyCode.ToString();

        card.sprite = axieAbility.GetStats().targetAxie;
        AssignAbilityUI();
        gameObject.SetActive(true);
    }

    public void SetNotAvailable()
    {
        Clear();
        SetDefaultState();
        gameObject.SetActive(false);
    }

    public void SetCardChargedState(bool state)
    {
        if (state && !isActivated)
        {
            mask.gameObject.SetActive(false);
            animator.SetTrigger("Charged");
        }
        else if (!state && isActivated)
        {
            mask.gameObject.SetActive(true);
            animator.SetTrigger("Cooldown");
        }
        isActivated = state;
    }

    public void SetDefaultState()
    {
        textProgress.text = "";
        SetCardChargedState(false);
        progress.fillAmount = 0;
        textDeploymentKey.text = "-";
    }

    public void SetCountdownState(bool value, float remainingTime = 0, float remainingTimeAsPercentage = 0)
    {
        if (value)
        {
            mask.gameObject.SetActive(true);
            var flooredRemainingTime = Mathf.FloorToInt(remainingTime);
            textProgress.text = flooredRemainingTime == 0 ? remainingTime.ToString("f1") : flooredRemainingTime.ToString();
            progress.fillAmount = remainingTimeAsPercentage;
        }
        else
        {
            mask.gameObject.SetActive(false);
            progress.fillAmount = 0;
        }
    }

    private void AssignAbilityUI()
    {
        switch (currentAbility)
        {
            case JoyOfBoom:
                AssignUI<JoyOfBoom, JoyOfBoomUI>(currentAbility);
                break;
            case BlueMoonLeap:
                AssignUI<BlueMoonLeap, BlueMoonLeapUI>(currentAbility);
                break;
            case TailSlap:
                AssignUI<TailSlap, TailSlapUI>(currentAbility);
                break;
            case JuicyRush:
                AssignUI<JuicyRush, JuicyRushUI>(currentAbility);
                break;
            case ForestHero:
                AssignUI<ForestHero, ForestHeroUI>(currentAbility);
                break;
            case SneakySketch:
                AssignUI<SneakySketch, SneakySketchUI>(currentAbility);
                break;
            case IroncladBarrier:
                AssignUI<IroncladBarrier, IroncladBarrierUI>(currentAbility);
                break;
            case SucculentBloom:
                AssignUI<SucculentBloom, SucculentBloomUI>(currentAbility);
                break;
            case NaturalHealing:
                AssignUI<NaturalHealing, NaturalHealingUI>(currentAbility);
                break;
        }
    }

    private void AssignUI<T, K>(AxieAbility ability) where T : AxieAbility where K : AbilityUI<T>
    {
        gameObject.AddComponent<K>().Init(this, ability as T);
    }

    private void Clear()
    {
        if (TryGetComponent<BaseAbilityUI>(out var baseAbilityUi))
        {
            baseAbilityUi.OnDispose();
            Destroy(baseAbilityUi);
        }
    }
}
