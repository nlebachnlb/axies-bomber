using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine.Collections;

public enum AxieClass
{
    Damage,
    Tanker,
    Assassin
}

[CreateAssetMenu(fileName = "New Axie Config", menuName = "Config/Axie")]
public class AxieConfig : ScriptableObject
{
    [Header("Animations")]
    public SkeletonDataAsset skinData;
    public AnimationReferenceAsset idle;
    public AnimationReferenceAsset run;
    public List<AnimationReferenceAsset> randomIdles;

    [Header("Skin")]
    public Sprite bombSprite;
    public Color auraColor;

    [Header("Information")]
    public string axieName;
    public AxieClass axieClass;
    public string description;
}
