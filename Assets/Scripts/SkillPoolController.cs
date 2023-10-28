using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SkillPoolController : MonoBehaviour
{
    private List<SkillConfig> skillPool;
    private List<List<SkillConfig>> abilities;
    private List<int> abilityPickTimes;
    private List<int> skillPickTimes;
    private GameplayController gameplay;

    private void Awake()
    {
        gameplay = GetComponent<GameplayController>();
        skillPool = new List<SkillConfig>();
        EventBus.onPickSkill += OnPickSkill;
    }

    private void OnDestroy()
    {
        EventBus.onPickSkill -= OnPickSkill;
    }

    private void Start()
    {
        skillPool = AppRoot.Instance.UserDataModel.GetAllSkillsFromPickedAxies();
        abilities = AppRoot.Instance.UserDataModel.GetPickedAxieAbilities();

        abilityPickTimes = new List<int>();
        skillPickTimes = new List<int>();

        for (int i = 0; i < abilities.Count; ++i)
            abilityPickTimes.Add(0);

        for (int i = 0; i < skillPool.Count; ++i)
            skillPickTimes.Add(0);
    }

    public T ObtainAbility<T>(SkillConfig ability) where T : SkillConfig
    {
        return (T)ability;
    }

    public List<SkillConfig> GetRandomizedSkillPool(bool abilityPool = false)
    {
        if (abilityPool)
        {
            List<SkillConfig> generatedAbilityPool = new List<SkillConfig>();
            for (int i = 0; i  < abilities.Count; ++i)
            {
                List<SkillConfig> ability = abilities[i];
                foreach (var data in gameplay.slots)
                {
                    if (data.identity == ability[0].axieIdentity)
                    {
                        if (data.ability == null)
                            generatedAbilityPool.Add(ability[0]);
                        else
                        {
                            int level = data.ability.level + 1;
                            if (level >= ability.Count)
                            {
                                level = ability.Count - 1;
                                SkillConfig maxConfig = Instantiate(ability[level]);
                                maxConfig.level = -1;
                                generatedAbilityPool.Add(maxConfig);
                            }
                            else
                                generatedAbilityPool.Add(ability[level]);
                        }
                        break;
                    }
                }
            }

            return generatedAbilityPool;
        }

        int sumPicks = 0;
        foreach (var pick in skillPickTimes)
            sumPicks += pick;

        if (sumPicks < skillPool.Count)
            sumPicks = skillPool.Count;

        int rand = UnityEngine.Random.Range(0, sumPicks);

        List<int> indices = new List<int>();
        for (int i = 0; i < skillPickTimes.Count; ++i)
            indices.Add(i);

        List<int> randomized = indices.OrderBy(x => (skillPickTimes[x] + rand)).ToList();
        List<SkillConfig> result = new List<SkillConfig>();
        for (int i = 0; i < Mathf.Min(randomized.Count, 3); ++i)
            result.Add(skillPool[randomized[i]]);

        return result;
    }

    private void OnPickSkill(SkillConfig skill)
    {
        int index = skillPool.FindIndex(x => x == skill);
        if (index >= 0)
        {
            skillPickTimes[index]++;
            Debug.Log(skillPickTimes[index]);
        }
    }
}
