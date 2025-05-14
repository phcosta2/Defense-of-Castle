using UnityEngine;

public class FireTower : MonoBehaviour
{
    public float range = 5f;            // Raio de alcance da torre
    public float fireRate = 0.5f;       // Taxa de disparo
    public GameObject projectilePrefab; // Prefab do projetil (fogo)

    private Transform firePoint;        // Ponto de disparo
    private float fireCooldown;         // Tempo de recarga entre os disparos

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

        // Encontrar o inimigo mais próximo dentro do alcance
        GameObject target = FindNearestEnemyInRange();

        // Atirar caso haja um inimigo e o tempo de recarga tenha acabado
        if (target != null && fireCooldown <= 0f)
        {
            Shoot(target);
            fireCooldown = fireRate;
        }
    }

    // Encontra o inimigo mais próximo dentro do alcance
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
        if (target == null) return;

        // Instancia o projetil na posição do firePoint
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // Configura o projetil com o alvo
        FireProjectile fireProj = proj.GetComponent<FireProjectile>();
        if (fireProj != null)
        {
            fireProj.SetTarget(target); // Passa o alvo para o projetil
        }
    }

    // Visualiza o alcance da torre no editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
