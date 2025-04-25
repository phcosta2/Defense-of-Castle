using UnityEngine;
using System.Collections;

// Gerencia o estado do jogo, spawn de inimigos e ondas,
// permitindo diferentes prefabs para diferentes caminhos.
public class GameManager : MonoBehaviour
{
    [Header("Referências de Prefabs")]
    [Tooltip("Arraste o Prefab do inimigo que seguirá o Caminho 1 (Esquerda)")]
    [SerializeField] private GameObject enemyPrefabPath1; // Prefab para o Caminho 1

    [Tooltip("Arraste o Prefab do inimigo que seguirá o Caminho 2 (Direita)")]
    [SerializeField] private GameObject enemyPrefabPath2; // Prefab para o Caminho 2

    [Header("Configurações dos Caminhos (Paths)")]
    [Tooltip("Arraste os Transforms dos Waypoints do Caminho 1 (Esquerda) aqui, NA ORDEM CORRETA")]
    [SerializeField] private Transform[] path1Waypoints;

    [Tooltip("Arraste os Transforms dos Waypoints do Caminho 2 (Direita) aqui, NA ORDEM CORRETA")]
    [SerializeField] private Transform[] path2Waypoints;

    [Header("Configurações da Wave")]
    [SerializeField] private int enemiesPerWave = 10;          // Quantidade TOTAL de inimigos por onda (considerando ambos os tipos)
    [SerializeField] private float timeBetweenEnemies = 1.0f;  // Tempo entre o spawn de cada inimigo na onda
    [SerializeField] private float timeBeforeFirstWave = 3.0f; // Tempo antes da primeira onda começar

    private int enemiesSpawnedThisWave = 0;
    private bool spawningWave = false;

    // Cache para validação dos caminhos
    private bool canUsePath1 = false;
    private bool canUsePath2 = false;

    void Start()
    {
        // Validações críticas no início
        bool hasPrefab1 = enemyPrefabPath1 != null;
        bool hasPrefab2 = enemyPrefabPath2 != null;
        canUsePath1 = path1Waypoints != null && path1Waypoints.Length >= 2;
        canUsePath2 = path2Waypoints != null && path2Waypoints.Length >= 2;

        if (!hasPrefab1 && !hasPrefab2) {
            Debug.LogError("!!! GAME MANAGER: Nenhum prefab de inimigo foi atribuído no Inspector! Desativando GameManager.", this);
            enabled = false; return;
        }
        if (!canUsePath1 && !canUsePath2) {
             Debug.LogError("!!! GAME MANAGER: Nenhum caminho válido definido (ambos precisam de >= 2 waypoints)! Abortando.", this);
             enabled = false; return;
        }

        // Avisos se um prefab está faltando mas o caminho existe, ou vice-versa
        if (hasPrefab1 && !canUsePath1) Debug.LogWarning("!!! GAME MANAGER: Prefab para Caminho 1 atribuído, mas Caminho 1 está incompleto/inválido.", this);
        if (!hasPrefab1 && canUsePath1) Debug.LogWarning("!!! GAME MANAGER: Caminho 1 válido, mas nenhum Prefab para Caminho 1 foi atribuído.", this);
        if (hasPrefab2 && !canUsePath2) Debug.LogWarning("!!! GAME MANAGER: Prefab para Caminho 2 atribuído, mas Caminho 2 está incompleto/inválido.", this);
        if (!hasPrefab2 && canUsePath2) Debug.LogWarning("!!! GAME MANAGER: Caminho 2 válido, mas nenhum Prefab para Caminho 2 foi atribuído.", this);

        // Inicia o processo da primeira onda após um delay inicial
        StartCoroutine(StartFirstWaveTimer());
    }

    IEnumerator StartFirstWaveTimer()
    {
        yield return new WaitForSeconds(timeBeforeFirstWave);
        StartNewWave();
    }

    void StartNewWave()
    {
        if (!spawningWave)
        {
             enemiesSpawnedThisWave = 0;
             StartCoroutine(SpawnWaveCoroutine());
        } else {
             Debug.LogWarning("GameManager: Tentativa de iniciar nova wave enquanto uma já está ativa.", this);
        }
    }

    IEnumerator SpawnWaveCoroutine()
    {
        spawningWave = true;
        Debug.Log($"GameManager: Iniciando spawn da wave (Total: {enemiesPerWave} inimigos)."); // Movido log para início da wave

        // Re-validar aqui caso algo tenha mudado dinamicamente (improvável neste setup)
        bool usePath1 = enemyPrefabPath1 != null && canUsePath1;
        bool usePath2 = enemyPrefabPath2 != null && canUsePath2;

        if (!usePath1 && !usePath2) {
             Debug.LogError("!!! GAME MANAGER: Nenhum combinação Prefab/Caminho válida disponível para spawn! Abortando wave.", this);
             spawningWave = false;
             yield break; // Sai da coroutine
        }

        for (int i = 0; i < enemiesPerWave; i++)
        {
            GameObject prefabToSpawn = null;
            Transform[] chosenPath = null;
            string pathName = "N/A"; // Declarada para uso no Debug.Log abaixo

            // --- LÓGICA DE ESCOLHA DE PREFAB E CAMINHO ---
            bool tryPath1First = (i % 2 == 0); // Tenta o caminho/prefab 1 para índices pares

            if (tryPath1First)
            {
                if (usePath1) // Tenta usar o Prefab/Caminho 1
                {
                    prefabToSpawn = enemyPrefabPath1;
                    chosenPath = path1Waypoints;
                    pathName = "Path 1";
                }
                else if (usePath2) // Se o 1 não puder, usa o 2 como fallback
                {
                    prefabToSpawn = enemyPrefabPath2;
                    chosenPath = path2Waypoints;
                    pathName = "Path 2 (Fallback)";
                     Debug.LogWarning($"Spawner: Usando fallback Path 2 para índice {i}"); // Mantém aviso de fallback
                }
            }
            else // Índices ímpares: Tenta o caminho/prefab 2 primeiro
            {
                 if (usePath2) // Tenta usar o Prefab/Caminho 2
                {
                    prefabToSpawn = enemyPrefabPath2;
                    chosenPath = path2Waypoints;
                    pathName = "Path 2";
                }
                else if (usePath1) // Se o 2 não puder, usa o 1 como fallback
                {
                    prefabToSpawn = enemyPrefabPath1;
                    chosenPath = path1Waypoints;
                    pathName = "Path 1 (Fallback)";
                    Debug.LogWarning($"Spawner: Usando fallback Path 1 para índice {i}"); // Mantém aviso de fallback
                }
            }

            // Se, mesmo após o fallback, não foi possível escolher, pula este inimigo
            if (prefabToSpawn == null || chosenPath == null)
            {
                Debug.LogWarning($"Wave Spawner: Não foi possível determinar um par Prefab/Caminho válido para o inimigo {i+1}. Pulando spawn.");
                continue; // Pula para a próxima iteração do loop
            }
            // -------------------------------------

            enemiesSpawnedThisWave++; // Incrementa SÓ SE for spawnar

            // 1. Instanciar o Prefab do Inimigo SELECIONADO
            GameObject newEnemyObject = Instantiate(prefabToSpawn, Vector3.zero, Quaternion.identity);
            newEnemyObject.name = $"{prefabToSpawn.name}_{enemiesSpawnedThisWave}"; // Nome inclui tipo e número

            // 2. Obter o script de movimento
            EnemyMovement enemyMoveScript = newEnemyObject.GetComponent<EnemyMovement>();

            // 3. Atribuir o caminho CORRESPONDENTE ao inimigo
            if (enemyMoveScript != null)
            {
                enemyMoveScript.SetPath(chosenPath);
                // *** LINHA DESCOMENTADA ABAIXO PARA USAR pathName ***
                Debug.Log($"Inimigo {newEnemyObject.name} spawnado em {pathName}");
            }
            else
            {
                // Erro crítico se o prefab não tiver o script necessário
                Debug.LogError($"!!! Prefab do inimigo ({prefabToSpawn.name}) está faltando o script EnemyMovement! Destruindo instância {newEnemyObject.name}.", newEnemyObject);
                Destroy(newEnemyObject);
                enemiesSpawnedThisWave--; // Decrementa pois o spawn falhou
                continue;
            }

            // 4. Esperar o tempo definido antes de spawnar o próximo inimigo
            yield return new WaitForSeconds(timeBetweenEnemies);
        }

        Debug.Log("GameManager: Spawn da wave concluído."); // Mantido log de fim de wave
        spawningWave = false;

        // Lógica futura para próximas ondas...
    }
}