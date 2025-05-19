using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target;
    public float speed = 70f;
    public float damage = 50f; // O dano pode ser passado pela torre que o disparou
    // public GameObject impactEffect; // Efeito visual ao atingir o alvo

    public void Seek(Transform _target, float _damage)
    {
        target = _target;
        damage = _damage;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject); // Destroi o projétil se o alvo não existir mais
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        
        // Opcional: Fazer o projétil olhar para o alvo
        // float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        // transform.rotation = Quaternion.Euler(0f, 0f, angle); // Ajuste 'angle - 90' se o sprite do projétil estiver orientado para cima
    }

    void HitTarget()
    {
        // Debug.Log("Projétil atingiu o alvo!");
        // if (impactEffect != null)
        // {
        //     GameObject effectIns = Instantiate(impactEffect, transform.position, transform.rotation);
        //     Destroy(effectIns, 2f); // Destroi o efeito após 2 segundos
        // }

        Enemy enemyScript = target.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.TakeDamage(damage);
        }
        else
        {
            Debug.LogWarning("Alvo não possui componente Enemy.");
        }

        Destroy(gameObject); // Destroi o projétil
    }

    // Opcional: para projéteis que não seguem o alvo (tiro reto)
    // e colidem com qualquer inimigo no caminho usando RigidBody2D e Collider2D.
    /*
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy")) // Certifique-se que seus inimigos têm a tag "Enemy"
        {
            Enemy enemyScript = col.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(damage);
            }
            // if (impactEffect != null) { ... }
            Destroy(gameObject);
        }
    }
    */
}