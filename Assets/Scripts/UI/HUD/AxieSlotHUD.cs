using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AxieSlotHUD : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI textBomb;
    [SerializeField] private RectTransform healthFill;
    [SerializeField] private Image axieIcon;
    [SerializeField] private Image buttonKey;
    [SerializeField] private TMPro.TextMeshProUGUI textKey;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private Image select;
    [SerializeField] private Image healthUnit;

    private int healthLimit = 0;
    private List<Image> healthUnits;

    private void Awake()
    {
        healthUnits = new List<Image>();
    }

    public void SetSelect(bool selected)
    {
        select.gameObject.SetActive(selected);
    }

    public void SetInfo(AxieHeroData.InfoPacket info)
    {
        textBomb.text = "" + info.bombsRemaining + "/" + info.bombMagazine;

        if (healthLimit != info.healthLimit)
        {
            foreach (var unit in healthUnits)
            {
                Destroy(unit.gameObject);
            }

            healthUnits.Clear();
            for (int i = 0; i < info.healthLimit; ++i)
            {
                Image unit = Instantiate(healthUnit, healthFill);
                healthUnits.Add(unit);
            }
        }

        foreach (var unit in healthUnits)
            unit.enabled = false;

        for (int i = 0; i < info.health; ++i)
            healthUnits[i].enabled = true;
    }

    public void SetAxieIcon(Sprite axie)
    {
        axieIcon.sprite = axie;
    }

    public void SetKeyInput(KeyCode key)
    {
        textKey.text = key.ToString().Replace("Alpha", "");
    }

    public void SetEnabled(bool enabled)
    {
        Color color = enabled ? Color.white : new Color(0.4f, 0.4f, 0.4f);
        axieIcon.CrossFadeColor(color, 0.2f, true, true);
        buttonKey.CrossFadeColor(color, 0.2f, true, true);
        infoPanel.SetActive(enabled);
    }
}
