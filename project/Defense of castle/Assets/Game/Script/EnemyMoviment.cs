using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [SerializeField] private float moveSpeed = 4f; // Velocidade do inimigo
    private float originalSpeed;  // Armazena a velocidade original para restaurar depois

    private Transform[] waypoints;         
    private int currentWaypointIndex = 0; 
    private bool pathAssigned = false;     

    void Start()
    {
        originalSpeed = moveSpeed;  // Guarda a velocidade original ao iniciar
    }

    void Update()
    {
        if (!pathAssigned || waypoints == null || waypoints.Length == 0 || currentWaypointIndex >= waypoints.Length)
        {
            return;
        }

        Transform targetWaypoint = waypoints[currentWaypointIndex];

        if (targetWaypoint == null)
        {
            pathAssigned = false; // Invalida o caminho
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

    // Método para definir o caminho do inimigo
    public void SetPath(Transform[] pathWaypoints)
    {
        waypoints = pathWaypoints;
        currentWaypointIndex = 0;
        pathAssigned = false;

        if (waypoints != null && waypoints.Length > 0 && waypoints[0] != null)
        {
            Vector3 startPos = waypoints[0].position;
            startPos.z = transform.position.z;
            transform.position = startPos;

            pathAssigned = true;
        }
        else
        {
            pathAssigned = false;
            Destroy(gameObject);
        }
    }

    // Restaura a velocidade original
    public void RestoreSpeed()
    {
        moveSpeed = originalSpeed;
    }

    // Método para ajustar a velocidade
    public void SetSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

    // Método para obter a velocidade atual
    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    void ReachEndOfPath()
    {
        pathAssigned = false;
        Destroy(gameObject);  // Inimigo chega ao fim do caminho
    }
}
