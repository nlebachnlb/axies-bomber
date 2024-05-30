using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public GameObject deathFxPrefab;
    public float hp = 1f;
    public float recoverTime = 0.5f;

    private float recoveryTimer = 0f;
    private EnemyController enemyController;
    private float startHp;

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0f)
        {
            enemyController.OnDeath();
            Instantiate(deathFxPrefab, transform.position, Quaternion.identity);

            if (enemyController.Coin > 0)
            {
                GameObject collectibleGameObject = Instantiate(AppRoot.Instance.Config.coinPrefab, transform.position, Quaternion.identity);
                Collectible collectible = collectibleGameObject.GetComponent<Collectible>();
                collectible.Amount = enemyController.Coin;
            }
            AppRoot.Instance.SoundManager.PlayAudio(SoundManager.AudioType.Slime);
        }
        Debug.Log($"Took damage: {damage}");
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
        startHp = hp;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (recoveryTimer > 0)
            return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            Explosion explosion = other.GetComponent<Explosion>();

            if (startHp != 0 && hp / startHp < explosion.criticalThreshold)
            {
                // Critical
                TakeDamage(hp);
            }
            else
            {
                TakeDamage(explosion.damage);
            }
            other.enabled = false;
        }

        if (other.CompareTag("KillField"))
        {
            TakeDamage(100f);
        }
    }
}
