using UnityEngine;

// Garante que o script EnemyMovement também esteja presente
[RequireComponent(typeof(EnemyMovement))]
public class BasicEnemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float health = 20f;
    [SerializeField] private GameObject dieiagePrefab; // Prefab da imagem de goblin morto
    [SerializeField] private float dieiageDuration = 2f; // Tempo que a imagem ficará visível


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
    if (movementScript != null)
    {
        movementScript.enabled = false; // Desabilita o script de movimento
    }

    // Dá dinheiro para o jogador
    MoneyManager moneyManager = FindObjectOfType<MoneyManager>();
    if (moneyManager != null)
    {
        moneyManager.AddMoney(5);
    }
    else
    {
        Debug.LogWarning("MoneyManager não encontrado na cena!");
    }

    if (dieiagePrefab != null)
    {
        GameObject dieiage = Instantiate(dieiagePrefab, transform.position, Quaternion.identity);
        Destroy(dieiage, dieiageDuration); // Após 2 segundos, destrói a imagem
    }

    Destroy(gameObject);
}

    // Você pode adicionar mais coisas específicas deste inimigo aqui depois.
}