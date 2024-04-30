using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Input Settting", menuName = "Config/Settings/Input")]
public class InputSetting : SerializedScriptableObject
{
    [Header("Movement")]
    public KeyCode up;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;

    [Header("Bomb spawn")]
    public KeyCode bomb;

    [Header("Switch Axie heros")]
    public KeyCode axie1;
    public KeyCode axie2;
    public KeyCode axie3;

    [Header("Skill")]
    [DictionaryDrawerSettings(KeyLabel = "Skill", ValueLabel = "Key Code")]
    public Dictionary<SkillType, KeyCode> skillDeploymentKeys;
}
