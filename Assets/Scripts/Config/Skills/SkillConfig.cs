using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillConfig : ScriptableObject
{
    public string skillName;
    [TextArea]
    public string description;

    [TextArea]
    public string minorDescription;
    public bool isAbility = false;
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
