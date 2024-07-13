using UnityEngine;

public class BaseAbilityUI : MonoBehaviour
{
    public virtual void OnDispose() { }
}

public class AbilityUI<T> : BaseAbilityUI where T: AxieAbility
{
    protected T ability;
    protected AbilitySlot slot;
    
    public virtual void Init(AbilitySlot slot, T ability)
    {
        this.slot = slot;
        this.ability = ability;
    }
}
