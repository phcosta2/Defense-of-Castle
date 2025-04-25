using UnityEngine;

// Garante que o script EnemyMovement também esteja presente
[RequireComponent(typeof(EnemyMovement))]
public class BasicEnemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float health = 20f;

    // Referência ao script de movimento (se precisar interagir com ele)
    private EnemyMovement movementScript;

    void Awake()
    {
        // Pega o componente no mesmo GameObject
        movementScript = GetComponent<EnemyMovement>();
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Lógica de morte: Efeitos, som, dar dinheiro/pontos
        // Debug.Log($"{gameObject.name} derrotado!");
        // Futuro: GameManager.Instance.AddMoney(10);
        Destroy(gameObject);
    }

    // Você pode adicionar mais coisas específicas deste inimigo aqui depois.
}