using UnityEngine;

public class FireTower : Tower
{
    [Header("Atributos Torre de Fogo")]
    public float baseDamage = 20f; // Dano base que o projétil desta torre causará
    // public float damageMultiplier = 1.3f; // Buff inicial - pode ser aplicado ao dano do projétil

    protected override void Shoot()
    {
        if (currentTarget == null || projectilePrefab == null || firePoint == null)
        {
            // Debug.LogWarning($"{name}: CurrentTarget, ProjectilePrefab ou FirePoint não configurado.");
            return;
        }

        // Instancia o projétil
        GameObject projectileGO = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Projectile projectileScript = projectileGO.GetComponent<Projectile>();

        if (projectileScript != null)
        {
            float totalDamage = baseDamage; // Você pode adicionar multiplicadores aqui
            projectileScript.Seek(currentTarget, totalDamage);
            Debug.Log($"{name} disparou projétil em {currentTarget.name} com {totalDamage} de dano potencial.");
        }
        else
        {
            Debug.LogError($"Prefab do projétil em {name} não possui o script 'Projectile'.");
            Destroy(projectileGO); // Destroi o GameObject instanciado se não tiver o script
        }
        // Adicionar efeito sonoro de tiro de fogo aqui, se desejar
    }

    protected override void ApplyUpgradeStats()
    {
        base.ApplyUpgradeStats(); // Chama o upgrade base (range, fireRate)
        baseDamage *= 1.2f; // Aumenta o dano base em 20%
        Debug.Log($"Fire Tower Upgraded: Level {currentLevel}, Damage: {baseDamage}");
    }
}