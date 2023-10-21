using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBus
{
    public static EventBus Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EventBus();
            }

            return instance;
        }
    }

    public delegate void OnBombFuse();
    public static event OnBombFuse onBombFuse;

    public delegate void OnSwitchAxieHero(AxieHeroData axieHeroData);
    public static event OnSwitchAxieHero onSwitchAxieHero;

    public delegate void OnAxieHeroDeath(AxieHeroData axieHeroData);
    public static event OnAxieHeroDeath onAxieHeroDeath;

    public static void RaiseOnBombFuse()
    {
        onBombFuse?.Invoke();
    }

    public static void RaiseOnSwitchAxieHero(AxieHeroData axieHeroData)
    {
        onSwitchAxieHero?.Invoke(axieHeroData);
    }

    public static void RaiseOnAxieHeroDeath(AxieHeroData axieHeroData)
    {
        onAxieHeroDeath?.Invoke(axieHeroData);
    }

    private static EventBus instance = null;
}
