using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AxieCard : MonoBehaviour
{
    [Header("Figure")]
    [SerializeField] private Image axieIcon;
    [SerializeField] private TMPro.TextMeshProUGUI textAxieName;

    [Header("Basic Stats")]
    [SerializeField] private TMPro.TextMeshProUGUI textSpeed;
    [SerializeField] private TMPro.TextMeshProUGUI textHp;
    [SerializeField] private TMPro.TextMeshProUGUI textBomb;
    [SerializeField] private TMPro.TextMeshProUGUI textLength;

    [Header("Action")]
    [SerializeField] private Button buttonSlot1;
    [SerializeField] private Button buttonSlot2;
    [SerializeField] private Button buttonSlot3;

    private Animator animator;
    private Button button;

    [HideInInspector]
    public AxiePackedConfig config;

    public int slotIndex;
    public delegate void OnSelect(AxieCard card);
    public event OnSelect onSelect;

    private void Awake()
    {
        buttonSlot1.onClick.AddListener(() => EventBus.RaiseOnPickAxie(0, config));
        buttonSlot2.onClick.AddListener(() => EventBus.RaiseOnPickAxie(1, config));
        buttonSlot3.onClick.AddListener(() => EventBus.RaiseOnPickAxie(2, config));

        animator = GetComponent<Animator>();
        button = GetComponent<Button>();

        button.onClick.AddListener(() =>
        {
            onSelect?.Invoke(this);
            animator.SetTrigger("Action");
        });
    }

    public void HideAction()
    {
        animator.SetTrigger("Normal");
    }

    public void ReloadConfig()
    {
        if (config == null)
            return;

        axieIcon.sprite = config.axieConfig.icon;
        textAxieName.text = config.axieConfig.axieName;
        textSpeed.text = "" + config.axieStats.speed;
        textHp.text = "" + config.axieStats.health;
        textBomb.text = "" + config.axieStats.bombMagazine;
        textLength.text = "" + config.axieStats.bombExplosionRadius;
    }
}
