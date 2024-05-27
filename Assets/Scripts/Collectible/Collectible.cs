using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public CollectibleType Type;

    [ShowInInspector, HideInEditorMode]
    public int Amount { get; set; }
}
