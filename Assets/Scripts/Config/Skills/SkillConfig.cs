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
    public Sprite visual;
    public Sprite targetAxie;

    public virtual string GenerateDescription(int level = 0)
    {
        return "";
    }

    public virtual string GenerateMinorDescription(int level = 0)
    {
        return "";
    }
}
