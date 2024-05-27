using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BindTextWithCurrency : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    
    private void OnEnable()
    {
        text.text = AppRoot.Instance.UserDataModel.User.Currency1.ToString();
        EventBus.onCurrency1Changed += OnCurrency1Changed;
    }

    private void OnDisable()
    {
        EventBus.onCurrency1Changed -= OnCurrency1Changed;
    }

    private void OnCurrency1Changed(int value)
    {
        text.text = value.ToString();
    }
}
