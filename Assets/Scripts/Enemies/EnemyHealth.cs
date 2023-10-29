using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float hp = 1f;
    public float recoverTime = 0.5f;

    private float recoveryTimer = 0f;

    private EnemyController enemyController;

    [SerializeField] private GameObject deathFxPrefab;

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0f)
        {
            enemyController.OnDeath();
            Instantiate(deathFxPrefab, transform.position, Quaternion.identity);
        }
    }

    private void Update()
    {
        if (recoveryTimer > 0)
        {
            recoveryTimer -= Time.deltaTime;

            if (recoveryTimer < 0)
            {
                recoveryTimer = 0f;
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(10);
        }
    }

    private void Awake()
    {
        enemyController = GetComponent<EnemyController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (recoveryTimer > 0)
            return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            TakeDamage(1f);
            other.enabled = false;
        }

        if (other.CompareTag("KillField"))
        {
            TakeDamage(100f);
        }
    }
}
