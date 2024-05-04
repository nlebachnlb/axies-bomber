using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Ability.Component
{
    public class Cooldown : MonoBehaviour
    {
        [SerializeField] private float cooldown;

        private float timer = 0;

        private void Awake()
        {
            timer = cooldown;
        }

        private void Update()
        {
            if (timer <= 0)
                return;

            timer -= Time.deltaTime;
        }
    }
}
