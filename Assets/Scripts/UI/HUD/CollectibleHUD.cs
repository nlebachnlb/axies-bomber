using TMPro;
using UnityEngine;

public class CollectibleHUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    private void Awake()
    {
        EventBus.onOilChanged += OnOilChanged;
    }

    private void OnDestroy()
    {
        EventBus.onOilChanged -= OnOilChanged;
    }

    public void SetAmount(int amount)
    {
        text.text = $"{amount} <sprite index=1>";
    }

    private void OnOilChanged(int amount)
    {
        SetAmount(amount);
    }
}
