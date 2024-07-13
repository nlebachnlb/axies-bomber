using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float duration;
    public GameObject explosionFx;

    [HideInEditorMode]
    public float damage { get; set; }

    private void Start()
    {
        Destroy(gameObject, duration);
        Vector3 position = transform.position;
        position.y += 0.5f;
        Instantiate(explosionFx, position, Quaternion.identity);
    }
}
