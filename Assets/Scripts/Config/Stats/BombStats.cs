using UnityEngine;

[CreateAssetMenu(fileName = "New Bomb Stats", menuName = "Stats/Bomb")]
public class BombStats : ScriptableObject
{
    public int length = 1;
    public float explosionDuration = 0.5f;
    public float bombFuseTime = 3f;
}
