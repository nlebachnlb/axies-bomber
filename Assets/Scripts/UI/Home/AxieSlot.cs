using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxieSlot : MonoBehaviour
{
    [SerializeField] private GameObject occuppiedGroup;
    [SerializeField] private GameObject emptyGroup;

    private AxiePackedConfig chosenAxie = null;

    private void Awake()
    {
        chosenAxie = null;
    }
}
