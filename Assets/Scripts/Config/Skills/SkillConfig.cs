using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SkillConfig : ScriptableObject
{
    public string skillName;
    [TextArea]
    public string description;
    [TextArea]
    public string minorDescription;
    public bool isAbility = false;

    [Title("Axie")]
    [PreviewField]
    public Sprite targetAxie;
    public AxieConfig ownerAxie;
    public AxieIdentity axieIdentity;
    public int level = 0;

    public virtual string GenerateDescription()
    {
        return "";
    }

    public virtual string GenerateMinorDescription()
    {
        return "";
    }
}
