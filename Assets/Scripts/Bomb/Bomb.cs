using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float bombFuseTime = 3f;
    public int explosionLength;
    public System.Action OnBombFuse;
    public Explosion explosionPrefab;
    public GameObject lightFx;
    public LayerMask explosionLayerMask;
    public Color color;

    [SerializeField] private SpriteRenderer spriteRenderer;

    public void LoadSkin(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    private void Start()
    {
        Destroy(gameObject, bombFuseTime);
    }

    private void OnDestroy()
    {
        Vector3 position = transform.position;
        var light = Instantiate(lightFx, position, Quaternion.identity);
        light.GetComponentInChildren<Light>().color = color;

        // Explode at the center first
        Instantiate(explosionPrefab, position, Quaternion.identity);

        // Then explode to 4 directions
        Explode(position, Vector3.left, explosionLength);
        Explode(position, Vector3.right, explosionLength);
        Explode(position, Vector3.forward, explosionLength);
        Explode(position, Vector3.back, explosionLength);

        OnBombFuse?.Invoke();

        EventBus.RaiseOnBombFuse();
    }

    /// <summary>
    /// Generate explosion from a point toward one direction
    /// That means to create a 4-direction explosion, we need to call this method 4 times to 4 directions
    /// </summary>
    /// <param name="position"></param>
    /// <param name="direction"></param>
    /// <param name="length"></param>
    private void Explode(Vector3 position, Vector3 direction, int length)
    {
        if (length <= 0)
        {
            return;
        }

        position += direction;

        Collider[] colliders = Physics.OverlapBox(new Vector3(position.x, position.y + 0.5f, position.z), Vector3.one * 0.25f, Quaternion.identity, explosionLayerMask);
        Debug.Log(colliders.Length);
        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.CompareTag("Destructible"))
                {
                    collider.GetComponent<Destructible>().RaiseOnHit(1);
                }
            }
            return;
        }

        Instantiate(explosionPrefab, position, Quaternion.identity);
        Explode(position, direction, length - 1);
    }
}
