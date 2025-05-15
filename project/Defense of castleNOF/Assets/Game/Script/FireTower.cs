using UnityEngine;

public class FireTower : MonoBehaviour
{
    public float range = 5f;
    public float fireRate = 0.5f;
    public GameObject projectilePrefab;

    private Transform firePoint;
    private float fireCooldown;

    void Awake()
    {
        // Cria um objeto vazio como firePoint no centro da torre
        GameObject fp = new GameObject("FirePoint");
        fp.transform.SetParent(transform);
        fp.transform.localPosition = Vector3.zero; // Posição central
        firePoint = fp.transform;
    }

    void Update()
    {
        fireCooldown -= Time.deltaTime;

        GameObject target = FindNearestEnemyInRange();

        if (target != null && fireCooldown <= 0f)
        {
            Shoot(target);
            fireCooldown = fireRate;
        }
    }

    GameObject FindNearestEnemyInRange()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearest = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance <= range && distance < shortestDistance)
            {
                shortestDistance = distance;
                nearest = enemy;
            }
        }

        return nearest;
    }

    void Shoot(GameObject target)
    {
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        FireProjectile fireProj = proj.GetComponent<FireProjectile>();
        if (fireProj != null)
        {
            fireProj.SetTarget(target);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
