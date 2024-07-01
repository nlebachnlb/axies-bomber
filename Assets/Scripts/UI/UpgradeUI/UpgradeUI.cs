using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private Button backButton;
    [SerializeField] private AxieExpBoard axieExpBoard;
    [SerializeField] private AxieStatsBoard axieStatsBoard;
    [SerializeField] private Transform axieCardList;
    [SerializeField] private AxieCardUpgrade axieCardPrefab;

    private int currentAxieId = -1;
    private AxieCardUpgrade currentSelectedCard;

    private void Awake()
    {
        backButton.onClick.AddListener(OnSelectBack);
    }

    private void Start()
    {
        List<int> ownedAxies = AppRoot.Instance.UserDataModel.User.ownedAxieIds;

        foreach (var id in ownedAxies)
        {
            var card = Instantiate(axieCardPrefab, axieCardList);
            card.gameObject.SetActive(true);
            AxiePackedConfig axiePackedConfig = AppRoot.Instance.Config.availableAxies.GetAxiePackedConfigById(id);
            card.Init(axiePackedConfig, onSelected: () =>
            {
                currentAxieId = card.Id;
                if (currentSelectedCard != null)
                    currentSelectedCard.Deselect();
                currentSelectedCard = card;
                Load();
            });
            card.Load();
        }

        currentAxieId = ownedAxies.First();
        currentSelectedCard = axieCardList.GetChild(0).GetComponent<AxieCardUpgrade>();
        Load();
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    private void OnSelectBack()
    {
        Close();
    }

    private void Load()
    {
        currentSelectedCard.Select();
        axieExpBoard.Load(currentAxieId);
        axieStatsBoard.Load(currentAxieId, AppRoot.Instance.UserDataModel.GetAxieStatsLevel(currentAxieId));
    }
}
