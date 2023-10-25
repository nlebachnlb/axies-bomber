using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPickUI : MonoBehaviour
{
    public List<SkillConfig> skills;

    [SerializeField] private List<SkillItem> skillItems;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < 3; ++i)
        {
            skillItems[i].Config = skills[i];
            skillItems[i].Reload();
        }
    }
}
