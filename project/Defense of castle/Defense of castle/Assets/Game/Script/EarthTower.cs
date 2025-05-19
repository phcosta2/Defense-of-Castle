using UnityEngine;

public class EarthTower : Tower
{
    [Header("Atributos Torre de Terra")]
    // 'new' keyword removed as 'damage' no longer exists in the base Tower class.
    // This 'damage' is now uniquely defined for EarthTower.
    public float damage = 15f;
    [Range(0f, 1f)]
    public float stunChance = 0.2f;
    public float stunDuration = 1.5f;

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
            // Uses the 'damage' defined in this EarthTower script
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

        // Now, EarthTower specific upgrades
        this.damage *= 1.2f;
        stunChance = Mathf.Min(stunChance * 1.2f, 0.9f);
        stunDuration *= 1.2f;

        // Log includes all relevant stats for this tower type
        Debug.Log($"Earth Tower Upgraded: Level {currentLevel}, Damage: {this.damage}, Stun: {stunChance * 100}% for {stunDuration}s. Range: {range}, FireRate: {fireRate}");
    }
}