using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 5f;

    private Transform target;
    private BasicEnemy targetEnemy;

    public void SetTarget(GameObject enemy)
    {
        if (enemy != null)
        {
            target = enemy.transform;
            targetEnemy = enemy.GetComponent<BasicEnemy>();
        }
    }

    void Update()
    {
        if (target == null || targetEnemy == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) < 0.2f)
        {
            targetEnemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
