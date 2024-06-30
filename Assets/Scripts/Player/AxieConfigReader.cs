using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine.Collections;

public class AxieConfigReader : MonoBehaviour
{
    public AxieConfig Axie { get; private set; }
    public SkeletonAnimation Skin { get => skin; }

    [SerializeField] private SkeletonAnimation skin;
    [SerializeField] private Light aura;
    [SerializeField] private ParticleSystem spawnRayLight;

    public void Load(AxieConfig axie)
    {
        Axie = axie;

        skin.skeletonDataAsset = axie.skinData;
        skin.Initialize(true);

        aura.color = axie.auraColor;
        spawnRayLight.Play();
    }
}
