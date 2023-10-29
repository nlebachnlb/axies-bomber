using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class AxieCard : MonoBehaviour
{
    [SerializeField] private RectTransform root;
    [SerializeField] private CanvasGroup actionGroup;
    [SerializeField] private Image bgSelect;
    [Header("Figure")]
    [SerializeField] private Image axieIcon;
    [SerializeField] private TMPro.TextMeshProUGUI textAxieName;
    [SerializeField] private Sprite classTank;
    [SerializeField] private Sprite classAssassin;
    [SerializeField] private Sprite classDps;
    [SerializeField] private Image imgClass;
    [SerializeField] private GameObject explain;
    [SerializeField] private TMPro.TextMeshProUGUI textExplain;

    [Header("Basic Stats")]
    [SerializeField] private TMPro.TextMeshProUGUI textSpeed;
    [SerializeField] private TMPro.TextMeshProUGUI textHp;
    [SerializeField] private TMPro.TextMeshProUGUI textBomb;
    [SerializeField] private TMPro.TextMeshProUGUI textLength;

    [Header("Action")]
    [SerializeField] private Button buttonSlot1;
    [SerializeField] private Button buttonSlot2;
    [SerializeField] private Button buttonSlot3;
    [SerializeField] private Button button;

    private Animator animator;

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

        button.onClick.AddListener(() =>
        {
            onSelect?.Invoke(this);
            ShowAction();
        });

        EventBus.onPickAxie += OnPickAxie;
    }

    private void OnDestroy()
    {
        EventBus.onPickAxie -= OnPickAxie;
    }

    private void Start()
    {
        HideAction();
        HideExplain();
    }

    private void OnPickAxie(int slot, AxiePackedConfig config)
    {
        UserDataModel model = AppRoot.Instance.UserDataModel;
        bgSelect.gameObject.SetActive(model.IsAxiePicked(this.config.id));
        HideAction();
    }

    public void ShowAction()
    {
        actionGroup.gameObject.SetActive(true);
        actionGroup.DOFade(1f, 0.15f);
        actionGroup.interactable = true;
    }

    public void HideAction()
    {
        actionGroup.gameObject.SetActive(false);
        actionGroup.DOFade(0f, 0.15f);
        actionGroup.interactable = false;
    }

    public void ReloadConfig()
    {
        if (config == null)
            return;

        bgSelect.gameObject.SetActive(false);
        axieIcon.sprite = config.axieConfig.icon;
        textAxieName.text = config.axieConfig.axieName;
        textSpeed.text = "" + config.axieStats.speed;
        textHp.text = "" + config.axieStats.health;
        textBomb.text = "" + config.axieStats.bombMagazine;
        textLength.text = "" + config.axieStats.bombExplosionRadius;

        switch (config.axieConfig.axieClass)
        {
            case AxieClass.Damage:
                imgClass.sprite = classDps;
                break;
            case AxieClass.Assassin:
                imgClass.sprite = classAssassin;
                break;
            case AxieClass.Tanker:
                imgClass.sprite = classTank;
                break;
        }
    }

    public void ShowExplain(string msg)
    {
        explain.SetActive(true);
        textExplain.text = msg;
    }

    public void HideExplain()
    {
        explain.SetActive(false);
    }

    public void OnPointerEnter(BaseEventData data)
    {
        Debug.Log("Mouse enter");
        root.DOLocalMoveY(32f, 0.3f).SetEase(Ease.OutCirc);

        ShowExplain("Speed: " + config.axieStats.speed + "\nHitpoints: " + config.axieStats.health + "\nBomb magazine: " + config.axieStats.bombMagazine + "\nBomb length: " + config.axieStats.bombExplosionRadius);
    }

    public void OnPointerExit(BaseEventData data)
    {
        Debug.Log("Mouse exit");
        root.DOLocalMoveY(0f, 0.3f).SetEase(Ease.OutCirc);

        HideExplain();
    }
}
