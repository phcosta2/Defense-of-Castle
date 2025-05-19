using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [Header("Atributos Gerais da Torre")]
    [SerializeField] protected float range = 15f;
    [SerializeField] protected float fireRate = 1f; // Tiros por segundo
    protected float fireCountdown = 0f;
    public int cost = 100; // Custo para construir esta torre

    [Header("Referências")]
    [SerializeField] protected string enemyTag = "Enemy"; // Tag dos inimigos
    protected Transform currentTarget;

    [Header("Para Projéteis (Opcional)")]
    public GameObject projectilePrefab;
    public Transform firePoint;         // Crie um GameObject filho na torre e atribua aqui

    public int currentLevel = 1;
    private float upgradeCostMultiplier = 1.5f;

    public virtual int GetUpgradeCost()
    {
        return Mathf.RoundToInt(cost * upgradeCostMultiplier * currentLevel);
    }

    public virtual bool CanUpgrade()
    {
        return true; // Sempre pode fazer upgrade (as per your comment about unlimited levels)
    }

    protected virtual void ApplyUpgradeStats()
    {
        range *= 1.2f; // Aumenta o range em 20%
        fireRate *= 1.2f; // Aumenta a taxa de disparo em 20%
        currentLevel++;
        Debug.Log($"{name} upgraded to level {currentLevel}. New range: {range}, New fire rate: {fireRate}");
    }

    protected virtual void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.25f);
    }

    protected virtual void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        if (currentTarget != null && Vector3.Distance(transform.position, currentTarget.position) <= range)
        {
            Enemy enemyScript = currentTarget.GetComponent<Enemy>();
            if (enemyScript != null && enemyScript.currentHealth > 0)
            {
                return;
            }
        }

        foreach (GameObject enemyObject in enemies)
        {
            if (!enemyObject.activeInHierarchy) continue;

            float distanceToEnemy = Vector3.Distance(transform.position, enemyObject.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemyObject;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            currentTarget = nearestEnemy.transform;
        }
        else
        {
            currentTarget = null;
        }
    }

    protected virtual void Update()
    {
        if (currentTarget == null)
        {
            return;
        }

        LockOnTarget();

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }
        fireCountdown -= Time.deltaTime;
    }

    protected virtual void LockOnTarget()
    {
        Transform partToRotate = firePoint != null ? firePoint : transform;
        Vector3 dir = currentTarget.position - partToRotate.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        partToRotate.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    protected abstract void Shoot();

    public virtual void UpgradeTower()
    {
        if (CanUpgrade() && GameManager.instance != null && GameManager.instance.CanAfford(GetUpgradeCost()))
        {
            GameManager.instance.SpendCurrency(GetUpgradeCost());
            ApplyUpgradeStats();

            if (UIManager.instance != null && UIManager.instance.GetSelectedTowerForUpgrade() == this)
            {
                UIManager.instance.UpdateUpgradePanel(this);
            }
        }
        else
        {
            if (GameManager.instance == null)
            {
                Debug.LogError("GameManager instance not found for upgrade.");
            }
            else if (!CanUpgrade())
            {
                Debug.LogWarning($"Cannot upgrade {name}. Upgrade conditions not met (e.g., max level).");
            }
            else
            {
                Debug.LogWarning($"Cannot upgrade {name}. Not enough currency. Cost: {GetUpgradeCost()}, Have: {GameManager.instance.currentPlayerCurrency}");
            }
        }
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public int GetSellValue()
    {
        return cost / 2;
    }
}