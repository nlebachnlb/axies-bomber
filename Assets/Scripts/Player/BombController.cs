using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombController : MonoBehaviour
{
    [Header("Bomb")]
    public KeyCode inputKey = KeyCode.Space;
    public Bomb bombPrefab;

    [Header("Explosion")]
    public LayerMask explosionLayerMask;

    //[Header("Destructible")]
    //public Tilemap destructibleTiles;
    //public Destructible destructiblePrefab;

    private AxieConfigReader config;
    private StatsModifier stats;
    private int bombsRemaining;

    private void Awake()
    {
        stats = GetComponent<StatsModifier>();
        config = GetComponent<AxieConfigReader>();
    }

    private void Start()
    {
        bombsRemaining = stats.axieStats.bombMagazine;
    }

    private void Update()
    {
        if (bombsRemaining > 0 && Input.GetKeyDown(inputKey))
        {
            PlaceBomb();
        }
    }

    private void PlaceBomb()
    {
        Vector3 position = transform.position;
        position.x = Mathf.Round(position.x);
        position.z = Mathf.Round(position.z);

        bombsRemaining--;

        Bomb bomb = Instantiate(bombPrefab, position, Quaternion.identity);
        bomb.OnBombFuse = () =>
        {
            bombsRemaining++;
        };
        bomb.bombFuseTime = stats.bombStats.bombFuseTime;
        bomb.explosionLength = stats.bombStats.length;
        bomb.LoadSkin(config.Axie.bombSprite);
        bomb.color = config.Axie.auraColor;
    }

    //private void ClearDestructible(Vector2 position)
    //{
    //    Vector3Int cell = destructibleTiles.WorldToCell(position);
    //    TileBase tile = destructibleTiles.GetTile(cell);

    //    if (tile != null)
    //    {
    //        Instantiate(destructiblePrefab, position, Quaternion.identity);
    //        destructibleTiles.SetTile(cell, null);
    //    }
    //}

    public void AddBomb()
    {
        bombsRemaining++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            other.isTrigger = false;
        }
    }
}
