using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxieHeroHUD : MonoBehaviour
{
    [SerializeField] private AxieSlotHUD axieSlotHUD;

    public void InitHUD(List<AxieHeroData> axieHeroDatas, List<KeyCode> inputMap)
    {
        for (int i = 0; i < axieHeroDatas.Count; ++i)
        {
            AxieHeroData axieHeroData = axieHeroDatas[i];
            AxieSlotHUD hud = Instantiate(axieSlotHUD, transform);
            hud.SetAxieIcon(axieHeroData.axieConfig.icon);
            hud.SetKeyInput(inputMap[i]);
            axieHeroData.onInfoChanged += (int health, int bombsRemaining) =>
            {
                hud.SetInfo(bombsRemaining, health);
            };
        }
    }

    private void OnInfoChanged(int health, int bombsRemaining)
    {

    }
}
