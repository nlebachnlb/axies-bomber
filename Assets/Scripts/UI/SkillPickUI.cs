using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPickUI : MonoBehaviour
{
    public List<SkillConfig> skills;

    [SerializeField] private List<SkillItem> skillItems;
    private Animator animator;

    private void Awake()
    {
        EventBus.onPickSkill += OnPickSkill;

        animator = GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        EventBus.onPickSkill -= OnPickSkill;
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < 3; ++i)
        {
            skillItems[i].Config = skills[i];
            skillItems[i].Reload();
        }
    }

    private void OnPickSkill(SkillConfig skill)
    {
        StartCoroutine(PickSkillProcess(skill));
    }

    private IEnumerator PickSkillProcess(SkillConfig skill)
    {
        animator.SetTrigger("Exit");
        yield return new WaitForSeconds(0.8f);
        Destroy(gameObject);
    }
}
