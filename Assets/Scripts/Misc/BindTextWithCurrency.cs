using TMPro;
using UnityEngine;

public class BindTextWithCurrency : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    private void OnEnable()
    {
        text.text = AppRoot.Instance.UserDataModel.User.Gear.ToString();
        EventBus.onGearChanged += OnGearChanged;
    }

    private void OnDisable()
    {
        EventBus.onGearChanged -= OnGearChanged;
    }

    private void OnGearChanged(int value)
    {
        text.text = value.ToString();
    }
}
