using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Ability.Component
{
    public class Timer : MonoBehaviour
    {
        public event Action OnStartCountdown;
        public event Action OnCooldownFinished;

        public bool IsAvailable => timer <= 0;
        public float CurrentCooldownValue => cooldown;
        public float RemainingTime => timer;
        public float RemainingTimeAsPercentage => cooldown == 0 ? 0 : timer / cooldown;

        private float cooldown;
        private float timer = 0;

        public void StartCountdown(float cooldown)
        {
            this.cooldown = cooldown;
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
