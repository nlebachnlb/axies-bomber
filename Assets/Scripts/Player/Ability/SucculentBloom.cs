using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SucculentBloom : AxieAbility<SucculentBloomStats>
{
    [SerializeField] private ParticleSystem effect;

    private void OnEnable()
    {
        EventBus.onRoomClear += OnRoomClear;
    }

    private void OnDisable()
    {
        EventBus.onRoomClear -= OnRoomClear;
    }

    private void OnRoomClear()
    {
        if (UnityEngine.Random.Range(0f, 1f) < Stats.Rate)
        {
            Debug.Log("TODO: Buff HP");
        }
    }
}
