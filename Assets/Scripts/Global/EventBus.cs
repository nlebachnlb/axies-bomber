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

    public static void RaiseOnBombFuse()
    {
        onBombFuse?.Invoke();
    }
    
    private static EventBus instance = null;
}
