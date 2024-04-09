using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectibleHUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    public void SetAmount(int amount)
    {
        text.text = $"{amount} <sprite index=0>";
    }
}
