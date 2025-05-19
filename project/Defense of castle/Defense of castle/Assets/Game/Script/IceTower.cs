using UnityEngine;

public class IceTower : Tower
{
    [Header("Atributos Torre de Gelo")]
    // 'new' keyword removed as 'damage' no longer exists in the base Tower class.
    // This 'damage' is now uniquely defined for IceTower.
    public float damage = 10f;
    public float slowAmount = 0.2f;
    public float slowDuration = 2f;

    protected override void Shoot()
    {
        if (currentTarget == null || projectilePrefab == null || firePoint == null)
        {
            return;
        }

        GameObject projectileGO = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Projectile projectileScript = projectileGO.GetComponent<Projectile>();

        if (projectileScript != null)
        {
            // Uses the 'damage' defined in this IceTower script
            projectileScript.Seek(currentTarget, damage);
            // Debug.Log($"{name} disparou projétil em {currentTarget.name} com {damage} de dano potencial.");
        }
        else
        {
            Debug.LogError($"Prefab do projétil em {name} não possui o script 'Projectile'.");
            Destroy(projectileGO);
        }
    }

    protected override void ApplyUpgradeStats()
    {
        base.ApplyUpgradeStats(); // Upgrades common stats (range, fireRate) and increments currentLevel from Tower class
        
        // Now, IceTower specific upgrades
        this.damage *= 1.2f;
        slowAmount = Mathf.Min(slowAmount * 1.2f, 0.9f);
        slowDuration *= 1.2f;
        
        // Log includes all relevant stats for this tower type
        Debug.Log($"Ice Tower Upgraded: Level {currentLevel}, Damage: {this.damage}, Slow: {slowAmount} for {slowDuration}s. Range: {range}, FireRate: {fireRate}");
    }
}