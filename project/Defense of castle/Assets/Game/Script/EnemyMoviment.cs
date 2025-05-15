using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [SerializeField] private float moveSpeed = 4f;  // Velocidade padrão do inimigo

    private float originalSpeed;  // Para restaurar após lentidão
    private Coroutine slowCoroutine;

    private Transform[] waypoints;
    private int currentWaypointIndex = 0;
    private bool pathAssigned = false;
    private bool hasReachedEnd = false;

    private LifeManager lifeManager;  // Referência para gerenciar vidas

    private void Start()
    {
        originalSpeed = moveSpeed;
        lifeManager = GameObject.FindObjectOfType<LifeManager>();
        if (lifeManager == null)
        {
            Debug.LogError("LifeManager não encontrado na cena!");
        }
    }

    private void Update()
    {
        if (!pathAssigned || waypoints == null || currentWaypointIndex >= waypoints.Length)
            return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];

        if (targetWaypoint == null)
        {
            pathAssigned = false;
            Debug.LogWarning($"Waypoint nulo para {gameObject.name}, parando movimento.");
            return;
        }

        Vector3 targetPosition = targetWaypoint.position;
        targetPosition.z = transform.position.z;

        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypoints.Length)
            {
                ReachEndOfPath();
            }
        }
    }

    public void SetPath(Transform[] newWaypoints)
    {
        waypoints = newWaypoints;
        currentWaypointIndex = 0;

        if (waypoints != null && waypoints.Length > 0 && waypoints[0] != null)
        {
            transform.position = waypoints[0].position;
            pathAssigned = true;
            hasReachedEnd = false;
        }
        else
        {
            Debug.LogError("Caminho inválido atribuído a inimigo.");
            pathAssigned = false;
        }
    }

    private void ReachEndOfPath()
    {
        if (hasReachedEnd)
            return;

        hasReachedEnd = true;

        if (lifeManager != null)
            lifeManager.LoseLife();
        else
            Debug.LogError("LifeManager não está referenciado no EnemyMovement!");

        Destroy(gameObject);
    }

    // Ajusta a velocidade, usado para efeito de lentidão do projetil de gelo
    public void SetSpeed(float newSpeed)
    {
        moveSpeed = Mathf.Clamp(newSpeed, originalSpeed * 0.5f, originalSpeed); // Limita para no máximo originalSpeed e no mínimo 50% da original
        // Se quiser, pode cancelar a coroutine anterior para reiniciar o slow
        if (slowCoroutine != null)
            StopCoroutine(slowCoroutine);
    }

    // Restaura a velocidade original após o efeito acabar
    public void RestoreSpeed()
    {
        moveSpeed = originalSpeed;
        if (slowCoroutine != null)
        {
            StopCoroutine(slowCoroutine);
            slowCoroutine = null;
        }
    }

    // Coroutine para gerenciar duração da lentidão (opcional)
    public void ApplySlow(float duration)
    {
        if (slowCoroutine != null)
            StopCoroutine(slowCoroutine);
        slowCoroutine = StartCoroutine(SlowDurationCoroutine(duration));
    }

    private IEnumerator SlowDurationCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        RestoreSpeed();
    }

    // Método para obter a velocidade atual, útil para projetil de gelo
    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
}
