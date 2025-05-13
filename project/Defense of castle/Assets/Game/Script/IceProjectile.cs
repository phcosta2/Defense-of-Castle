using UnityEngine;

public class IceProjectile : MonoBehaviour
{
    public float speed = 10f;   // A velocidade do projetil
    public float damage = 10f;  // Dano que o projetil causará
    public float slowAmount = 0.5f;  // Quanto o inimigo vai ficar mais lento (50% de redução)
    public float slowDuration = 3f;  // Duração do efeito de lentidão

    private Transform target;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();  // Obtém o Rigidbody2D
    }

    // Define o alvo para o qual o projetil vai
    public void SetTarget(Transform enemy)
    {
        target = enemy;
    }

void Update()
{
    // Verifica se o target ainda está vivo
    if (target == null)
    {
        Destroy(gameObject); // Se o alvo foi destruído, destrua o projetil
        return;
    }

    // Calcula a direção do projetil
    Vector3 direction = (target.position - transform.position).normalized;

    // Move o projetil na direção do alvo
    transform.position += direction * speed * Time.deltaTime;

    // Verifica se o projetil atingiu o inimigo
    if (Vector3.Distance(transform.position, target.position) < 0.5f)
    {
        // Aplica dano ao inimigo
        if (target != null)
        {
            BasicEnemy enemyScript = target.GetComponent<BasicEnemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(damage);
                ApplySlowEffect(target.GetComponent<EnemyMovement>());
            }
        }

        Destroy(gameObject); // Destrói o projetil após atingir o inimigo
    }
}

    // Aplica o efeito de lentidão ao inimigo
    void ApplySlowEffect(EnemyMovement enemyMovement)
    {
        if (enemyMovement != null)
        {
            // Aplica a lentidão, mas não pode ser mais de 50% de redução
            float newSpeed = Mathf.Max(enemyMovement.GetMoveSpeed() * (1 - slowAmount), enemyMovement.GetMoveSpeed() * 0.5f);
            enemyMovement.SetSpeed(newSpeed);

            // A lentidão dura um tempo e depois retorna à velocidade original
            StartCoroutine(RemoveSlowEffect(enemyMovement));
        }
    }

    // Remove o efeito de lentidão após um certo tempo
    private System.Collections.IEnumerator RemoveSlowEffect(EnemyMovement enemyMovement)
    {
        yield return new WaitForSeconds(slowDuration);
        enemyMovement.RestoreSpeed();  // Restaura a velocidade original
    }
}
