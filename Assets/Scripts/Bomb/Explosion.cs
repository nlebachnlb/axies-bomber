using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float duration;
    public GameObject explosionFx;

    private void Start()
    {
        Destroy(gameObject, duration);
        Vector3 position = transform.position;
        position.y += 0.5f;
        Instantiate(explosionFx, position, Quaternion.identity);
    }
}
