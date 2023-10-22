using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxieList : MonoBehaviour
{
    [SerializeField] private GameObject content;
    [SerializeField] private AxieCard axieCard;

    private void Start()
    {
        Reload();
    }

    public void Reload()
    {
        AxieCard[] comp = GetComponentsInChildren<AxieCard>();
        List<int> availableAxies = AppRoot.Instance.UserDataModel.User.ownedAxieIds;

        foreach (AxieCard card in comp)
            card.gameObject.SetActive(false);

        for (int i = 0; i < availableAxies.Count; ++i)
        {
            AxiePackedConfig config = AppRoot.Instance.Config.availableAxies.GetAxiePackedConfigById(availableAxies[i]);
            if (i >= comp.Length)
            {
                AxieCard card = Instantiate(axieCard, content.transform);
                card.config = config;
                card.ReloadConfig();
                card.onSelect += (AxieCard card) =>
                {
                    AxieCard[] cards = GetComponentsInChildren<AxieCard>();
                    foreach (var c in cards)
                        if (c != card)
                            card.HideAction();
                };
            }
            else
            {
                comp[i].gameObject.SetActive(true);
                comp[i].config = config;
                comp[i].ReloadConfig();
            }
        }
    }
}
