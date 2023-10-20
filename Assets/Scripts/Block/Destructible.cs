using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public int hitpoints = 1;

    public delegate void OnHit(int damage);
    public event OnHit onHit;

    private Animator animator;

    public void RaiseOnHit(int damage)
    {
        hitpoints -= damage;
        if (hitpoints <= 0)
        {
            Destroy(gameObject);
            return;
        }

        animator.SetTrigger("Hit");

        onHit?.Invoke(damage);
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
}
