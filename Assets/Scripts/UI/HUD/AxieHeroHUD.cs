using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxieHeroHUD : MonoBehaviour
{
    [SerializeField] private AxieSlotHUD axieSlotHUD;

    private List<AxieSlotHUD> huds;

    public void InitHUD(List<AxieHeroData> axieHeroDatas, List<KeyCode> inputMap)
    {
        huds = new List<AxieSlotHUD>();
        for (int i = 0; i < axieHeroDatas.Count; ++i)
        {
            AxieHeroData axieHeroData = axieHeroDatas[i];
            AxieSlotHUD hud = Instantiate(axieSlotHUD, transform);
            hud.SetAxieIcon(axieHeroData.axieConfig.icon);
            hud.SetKeyInput(inputMap[i]);
            axieHeroData.onInfoChanged += (AxieHeroData.InfoPacket info) =>
            {
                hud.SetInfo(info);
                hud.SetEnabled(info.health > 0);
            };
            huds.Add(hud);
        }
    }

    public void SelectSlot(int index)
    {
        for (int i = 0; i < huds.Count; ++i)
            huds[i].SetSelect(i == index);
    }
}
