using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public int currencyValue = 10; // Quanto de dinheiro o jogador ganha ao derrotar

    [Header("Efeitos Visuais")] // Novo ou modificado Header
    public GameObject deathEffectPrefab; // Antigo dieiagePrefab
    public float deathEffectDuration = 2f; // Antigo dieiageDuration

    private EnemyMovement enemyMovement; // Referência ao seu script de movimento
    private float originalSpeed;
    private bool isSlowed = false;
    private bool isStunned = false;

    void Awake()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            originalSpeed = enemyMovement.moveSpeed;
        }
        else
        {
            Debug.LogError("EnemyMovement script not found on " + gameObject.name);
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Adicionar lógica de morte (efeitos visuais, som, etc.)
        if (GameManager.instance != null) // Adicionado verificação de nulo para segurança
        {
            GameManager.instance.AddCurrency(currencyValue);
        }

        if (deathEffectPrefab != null)
        {
            GameObject effectInstance = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effectInstance, deathEffectDuration);
        }

        // Opcional: Desabilitar movimento antes de destruir, se necessário
        if (enemyMovement != null)
        {
            enemyMovement.enabled = false;
        }

        Destroy(gameObject);
    }

    public void ApplySlow(float slowFactor, float duration)
    {
        if (enemyMovement != null && !isSlowed && !isStunned) // Não acumula slow se já estiver stunado ou slow
        {
            StartCoroutine(SlowCoroutine(slowFactor, duration));
        }
    }

    private IEnumerator SlowCoroutine(float slowFactor, float duration)
    {
        isSlowed = true;
        // Certifique-se que originalSpeed foi inicializada em Awake()
        float slowedSpeed = originalSpeed * (1f - slowFactor);
        enemyMovement.moveSpeed = slowedSpeed;
        yield return new WaitForSeconds(duration);
        if (!isStunned) // Só restaura se não estiver stunado
        {
            enemyMovement.moveSpeed = originalSpeed;
        }
        isSlowed = false;
    }

    public void ApplyStun(float duration)
    {
        if (enemyMovement != null && !isStunned)
        {
            StartCoroutine(StunCoroutine(duration));
        }
    }

    private IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;
        // float previousSpeed = enemyMovement.moveSpeed; // Salva a velocidade atual (pode estar slowed)
        // Não precisamos mais de previousSpeed se restaurarmos corretamente com originalSpeed e isSlowed
        
        // Salva a velocidade antes do stun para restaurar corretamente
        float speedBeforeStun = enemyMovement.moveSpeed;
        
        enemyMovement.moveSpeed = 0;
        // enemyMovement.enabled = false; // Pausa o script de movimento - pode ser muito abrupto
                                       // apenas zerar a velocidade pode ser suficiente.
                                       // Se enemyMovement.Update() tiver outras lógicas, desabilitar pode ser melhor.

        yield return new WaitForSeconds(duration);

        // enemyMovement.enabled = true; // Reativa o script de movimento se foi desabilitado

        // Restaura a velocidade corretamente considerando se estava 'slowed' antes do 'stun'
        if (isSlowed)
        {
            // Se estava 'slowed', a velocidade a ser restaurada é a 'slowedSpeed'
            // que foi calculada no SlowCoroutine. Precisamos garantir que 'originalSpeed'
            // e o 'slowFactor' atual (caso mude dinamicamente) sejam usados para recalcular
            // ou que 'speedBeforeStun' seja a velocidade 'slowed'.
            // Assumindo que 'speedBeforeStun' já é a velocidade correta (com slow, se aplicável)
            // Esta parte pode precisar de um ajuste se o slow for removido enquanto o stun está ativo.
            // Para simplicidade, vamos restaurar para a originalSpeed se não estiver mais slowed.
            // E se estiver slowed, o SlowCoroutine deve ter deixado a velocidade correta.
            // O problema é se o slow termina DURANTE o stun.

            // Se o slow ainda estiver ativo (isSlowed = true), não fazemos nada aqui,
            // pois a velocidade será controlada pelo SlowCoroutine quando ele terminar.
            // Apenas restauramos para originalSpeed se NÃO estiver mais slowed.
            
            // Se ainda estiver 'slowed', não restauramos para originalSpeed, 
            // deixamos a velocidade que estava antes do stun (que já era a velocidade com slow).
            // Mas ao final do slow, ele tentará restaurar para originalSpeed.
            // Se o slow acabar e o stun não, a velocidade será originalSpeed (após o stun).
            // Se o stun acabar e o slow não, a velocidade deve ser a slowedSpeed.

            // Correção: Se ainda estiver slowed, restaurar para a velocidade de slow.
            // Caso contrário, para a original.
            // float currentSlowFactor = ... ; // precisaria saber qual slow está ativo
            // enemyMovement.moveSpeed = originalSpeed * (1f - currentSlowFactor);
            // Isso está se tornando complexo. Uma forma mais simples:
            // Apenas restauramos para originalSpeed se não estiver slowed.
            // Se estiver slowed, o SlowCoroutine cuidará de definir a velocidade correta.
        }

        if (!isSlowed) {
            enemyMovement.moveSpeed = originalSpeed;
        } else {
            // Se ainda está slowed, a velocidade correta já deve estar sendo gerenciada
            // pelo SlowCoroutine, ou foi salva em speedBeforeStun.
            // Se o SlowCoroutine ainda não terminou, ele irá resetar para originalSpeed.
            // Se o SlowCoroutine terminou durante o stun, isSlowed seria false.
            // A lógica aqui é que se o slow ainda estiver marcado como ativo,
            // significa que a velocidade reduzida ainda deve ser aplicada.
            // O problema é qual 'slowFactor' usar.
            // Manteremos a velocidade como estava antes do stun (que já era a slowed speed).
            enemyMovement.moveSpeed = speedBeforeStun;
        }
        isStunned = false;
    }
}