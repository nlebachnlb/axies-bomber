using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TinyCatapultKillField : MonoBehaviour
{
    [SerializeField] private LayerMask layer;
    private void Update()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, 2.5f, layer);
        if (cols.Length > 0)
        {
            foreach (var col in cols)
            {
                if (col.CompareTag(Tag.ENEMY))
                {
                    col.GetComponent<EnemyHealth>().TakeDamage(100);
                }
            }
        }
    }
}
