using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AxieExpBoard : MonoBehaviour
{
    [SerializeField] private Image axieSkin;
    [SerializeField] private Image expBar;
    [SerializeField] private TMP_Text axieName;
    [SerializeField] private TMP_Text percentText;

    public void Load(int axieId)
    {
        AxiePackedConfig axiePackedConfig = AppRoot.Instance.Config.availableAxies.GetAxiePackedConfigById(axieId);
        AxieConfig axieConfig = axiePackedConfig.axieConfig;
        
        axieSkin.sprite = axieConfig.icon;
        axieName.text = axieConfig.axieName;
    }
}
