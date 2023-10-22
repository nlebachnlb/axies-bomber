using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine.Collections;

public class AxieSlot : MonoBehaviour
{
    [SerializeField] private GameObject occuppiedGroup;
    [SerializeField] private GameObject emptyGroup;
    [SerializeField] private SkeletonGraphic skin;
    [SerializeField] int slotIndex;

    private AxiePackedConfig chosenAxie = null;

    private void Awake()
    {
        chosenAxie = null;
        EventBus.onPickAxie += OnPickAxie;
    }

    private void OnDestroy()
    {
        EventBus.onPickAxie -= OnPickAxie;
    }

    private void Start()
    {
        Reload();
    }

    private void OnPickAxie(int slot, AxiePackedConfig config)
    {
        if (slot != slotIndex)
            return;

        chosenAxie = config;
        Reload();
    }

    private void Reload()
    {
        if (chosenAxie == null)
        {
            occuppiedGroup.SetActive(false);
            emptyGroup.SetActive(true);
            return;
        }

        occuppiedGroup.SetActive(true);
        emptyGroup.SetActive(false);

        skin.skeletonDataAsset = chosenAxie.axieConfig.skinData;
        skin.Initialize(true);
    }
}
