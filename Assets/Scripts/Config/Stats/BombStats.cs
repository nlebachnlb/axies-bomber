using UnityEngine;

[CreateAssetMenu(fileName = "New Bomb Stats", menuName = "Stats/Bomb")]
public class BombStats : ScriptableObject
{
    public float damage = 1f;
    public int length = 1;
    public float explosionDuration = 0.5f;
    public float bombFuseTime = 3f;

    public UpgradeBuff upgradeBuff { get; private set; } = new();

    public void AddUpgradeBuff(UpgradeBuff upgradeBuff)
    {
        this.upgradeBuff = upgradeBuff;
    }

    public BombStats Calculate()
    {
        var result = Instantiate(this);
        result.damage += upgradeBuff.bombDamage;
        return result;
    }
}
