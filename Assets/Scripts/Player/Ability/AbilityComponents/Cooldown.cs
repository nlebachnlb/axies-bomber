using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Ability.Component
{
    public class Cooldown : MonoBehaviour
    {
        public event Action OnStartCountdown;
        public event Action OnCooldownFinished;

        [SerializeField] private float cooldown;

        private float timer = 0;

        public bool IsAvailable => timer <= 0;
        public float CooldownValue => cooldown;
        public float RemainingTime => timer;
        public float RemainingTimeAsPercentage => timer / cooldown;

        public void StartCountdown()
        {
            timer = cooldown;
            OnStartCountdown?.Invoke();
        }

        private void Update()
        {
            if (timer <= 0)
                return;

            timer -= Time.deltaTime;
            if (timer <= 0)
                OnCooldownFinished?.Invoke();
        }
    }
}
