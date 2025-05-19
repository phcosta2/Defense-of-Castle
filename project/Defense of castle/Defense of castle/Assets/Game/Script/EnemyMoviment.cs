using UnityEngine;

public class EnemyMovement : MonoBehaviour // Certifique-se que o nome da classe está correto
{
    [Header("Configurações de Movimento")]
    [SerializeField] public float moveSpeed = 4f; // Velocidade

    // --- Variáveis Internas ---
    private Transform[] waypoints;         // O caminho a seguir (será dado pelo GameManager)
    private int currentWaypointIndex = 0; // Índice do ponto atual no caminho
    private bool pathAssigned = false;     // O caminho já foi definido?

    void Update()
    {
        // 1. Segurança: Só mover se tiver um caminho válido
        if (!pathAssigned || waypoints == null || waypoints.Length == 0 || currentWaypointIndex >= waypoints.Length)
        {
            return; // Não faz nada se não há caminho ou já terminou
        }

        // 2. Pegar o alvo atual
        Transform targetWaypoint = waypoints[currentWaypointIndex];

        // 3. Segurança extra: Se o alvo for nulo (destruído?), para.
        if (targetWaypoint == null)
        {
            Debug.LogError($"Waypoint {currentWaypointIndex} é nulo para {gameObject.name}. Parando.");
            pathAssigned = false; // Invalida o caminho
            enabled = false;      // Desativa este script
            return;
        }

        // 4. Calcular a posição alvo (mantendo o Z original)
        Vector3 targetPosition = targetWaypoint.position;
        targetPosition.z = transform.position.z;

        // 5. Mover em direção ao alvo
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        // 6. Checar se chegou perto o suficiente do alvo
        if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
        {
            // Considera que chegou e avança para o próximo waypoint
            currentWaypointIndex++;

            // 7. Chegou ao fim do caminho?
            if (currentWaypointIndex >= waypoints.Length)
            {
                ReachEndOfPath();
            }
        }
    }

    // MÉTODO PÚBLICO: O GameManager vai chamar isso!
    public void SetPath(Transform[] pathWaypoints)
    {
        waypoints = pathWaypoints; // Recebe o array de pontos
        currentWaypointIndex = 0;  // Começa do primeiro ponto (índice 0)
        pathAssigned = false;      // Reseta, valida abaixo

        // Validação inicial
        if (waypoints != null && waypoints.Length > 0 && waypoints[0] != null)
        {
            // Posiciona o inimigo no PRIMEIRO waypoint do caminho recebido
            Vector3 startPos = waypoints[0].position;
            startPos.z = transform.position.z; // Mantém o Z
            transform.position = startPos;

            pathAssigned = true; // AGORA SIM, tem um caminho válido!
            enabled = true;      // Garante que o script está ativo
        }
        else
        {
            Debug.LogError($"Tentativa de definir caminho inválido (nulo, vazio ou primeiro ponto nulo) para {gameObject.name}. Inimigo será desativado.");
            enabled = false; // Desativa se o caminho for ruim
            Destroy(gameObject, 0.1f); // Destroi logo para não ficar parado na cena
        }
    }

    // Chamado quando chega ao último waypoint
    void ReachEndOfPath()
    {
        // Debug.Log($"{gameObject.name} chegou ao fim!");
        pathAssigned = false; // Não tem mais caminho

        // Causa dano ao jogador
        if (GameManager.instance != null)
        {
            GameManager.instance.PlayerTakesDamage(1); // Ou qualquer valor de dano que o inimigo deva causar
        }
        else
        {
            Debug.LogError("GameManager.instance não encontrado em EnemyMovement.ReachEndOfPath!");
        }

        Destroy(gameObject); // Destroi o inimigo
    }

    // Desenha linhas no Editor para visualizar o caminho (só quando selecionado)
    void OnDrawGizmosSelected()
    {
        if (pathAssigned && waypoints != null && waypoints.Length > 1)
        {
            Gizmos.color = Color.green; // Cor do caminho
            for (int i = 0; i < waypoints.Length - 1; i++)
            {
                if (waypoints[i] != null && waypoints[i+1] != null)
                {
                    Gizmos.DrawLine(waypoints[i].position, waypoints[i+1].position);
                }
            }
        }
    }
}