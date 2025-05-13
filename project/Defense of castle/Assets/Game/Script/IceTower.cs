using UnityEngine;

public class IceTower : MonoBehaviour
{
    public float range = 5f;              // Raio da torre
    public float fireRate = 1f;           // Intervalo entre os disparos
    public GameObject projectilePrefab;   // Prefab do projetil (floco de neve)
    private Transform firePoint;           // Ponto de disparo

    private float fireCooldown;

    void Awake()
    {
        // Cria o firePoint no centro da torre
        firePoint = new GameObject("FirePoint").transform;
        firePoint.SetParent(transform); // Torna o firePoint filho da torre
        firePoint.localPosition = Vector3.zero; // Coloca o firePoint no centro
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

    // Encontra o inimigo mais próximo dentro do alcance da torre
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

    // Dispara o projetil em direção ao inimigo
    void Shoot(GameObject target)
    {
        // Verifica se o target ainda existe antes de disparar
        if (target == null)
        {
            return;
        }

        // Instancia o projetil
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // Passa o alvo para o projetil
        IceProjectile iceProj = proj.GetComponent<IceProjectile>();
        if (iceProj != null)
        {
            iceProj.SetTarget(target.transform); // Passa o alvo para o projetil
        }
    }

    // Visualiza o alcance da torre no editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
