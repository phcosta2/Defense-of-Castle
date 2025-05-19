using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

// Estruturas para definir as waves de forma mais flexível
[System.Serializable]
public class EnemySpawnInfo
{
    public GameObject enemyPrefab; // Prefab do inimigo específico (Goblin, Bandit, Shaman)
    public int count;              // Quantidade deste inimigo na wave
}

[System.Serializable]
public class WaveDefinition
{
    public string waveName; // Nome da wave para fácil identificação (ex: "Rodada 1: Goblins")
    public List<EnemySpawnInfo> enemiesToSpawn; // Lista de grupos de inimigos para esta wave
    public float timeBetweenSpawns = 1.0f;    // Tempo entre cada inimigo DENTRO desta wave
    public float timeAfterWaveCompleted = 5.0f; // Tempo de espera após esta wave antes da próxima poder ser chamada (opcional, mais para UI)
}

// Gerencia o estado do jogo, spawn de inimigos e ondas,
// permitindo diferentes prefabs para diferentes caminhos.
public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Public static instance for Singleton

    [Header("Referências de Prefabs de Inimigos Específicos")]
    [Tooltip("Prefab do inimigo Goblin")]
    [SerializeField] private GameObject goblinPrefab;
    [Tooltip("Prefab do inimigo Bandit")]
    [SerializeField] private GameObject banditPrefab;
    [Tooltip("Prefab do inimigo Shaman")]
    [SerializeField] private GameObject shamanPrefab;

    // Os campos enemyPrefabPath1 e enemyPrefabPath2 podem ser removidos se não forem mais usados
    // ou mantidos como fallback/opções adicionais se necessário.
    // Por ora, vamos focar nos prefabs específicos definidos acima e nas WaveDefinitions.
    // [SerializeField] private GameObject enemyPrefabPath1;
    // [SerializeField] private GameObject enemyPrefabPath2; 

    [Header("Configurações dos Caminhos (Paths)")]
    [Tooltip("Arraste os Transforms dos Waypoints do Caminho 1 (Esquerda) aqui, NA ORDEM CORRETA")]
    [SerializeField] private Transform[] path1Waypoints;

    [Tooltip("Arraste os Transforms dos Waypoints do Caminho 2 (Direita) aqui, NA ORDEM CORRETA")]
    [SerializeField] private Transform[] path2Waypoints;

    [Header("Configuração das Rodadas (Waves)")]
    [Tooltip("Defina aqui todas as waves para este nível")]
    [SerializeField] public List<WaveDefinition> waves; // Changed private to public
    private int currentWaveIndex = -1; // Começa em -1, a primeira chamada de CallNextWaveFromButton() irá para 0

    // Variáveis de estado da wave - Removidas ou ajustadas:
    // [SerializeField] private int enemiesPerWave = 10; // Substituído pela contagem em WaveDefinition
    // [SerializeField] private float timeBetweenEnemies = 1.0f; // Substituído por timeBetweenSpawns em WaveDefinition
    // [SerializeField] private float timeBeforeFirstWave = 3.0f; // Primeira wave será chamada por botão

    [Header("Player Stats")]
    public int playerLives = 25;
    private int startingPlayerLives; // Para resetar

    [Header("Game State")]
    public int currentPlayerCurrency = 0;
    public bool isGameOver = false;

    // private int enemiesSpawnedThisWave = 0; // Não mais necessário globalmente, gerenciado por SpawnWaveCoroutine
    public bool spawningWave = false; // Changed private to public
    private int totalEnemiesThisWave = 0;
    private int enemiesEffectivelySpawnedThisWave = 0;

    // Cache para validação dos caminhos
    private bool canUsePath1 = false;
    private bool canUsePath2 = false;

    [Header("Configuração de Cena")]
    [Tooltip("Nome da cena para carregar após todas as waves serem concluídas")]
    [SerializeField] private string nextSceneName;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.LogWarning("GameManager: Another instance of GameManager already exists. Destroying this one.", this);
            Destroy(gameObject);
            return;
        }
        // DontDestroyOnLoad(gameObject); // Descomente se quiser que o GameManager persista entre cenas (não usual para tower defense simples)
        startingPlayerLives = playerLives; // Armazena as vidas iniciais
    }

    void Start()
    {
        // Validações críticas no início
        canUsePath1 = path1Waypoints != null && path1Waypoints.Length >= 2;
        canUsePath2 = path2Waypoints != null && path2Waypoints.Length >= 2;

        if (!canUsePath1 && !canUsePath2)
        {
            Debug.LogError("!!! GAME MANAGER: Nenhum caminho válido definido (ambos precisam de >= 2 waypoints)! Abortando.", this);
            enabled = false; return;
        }

        // Validação dos prefabs principais (opcional, mas bom ter)
        if (goblinPrefab == null || banditPrefab == null || shamanPrefab == null)
        {
            Debug.LogWarning("!!! GAME MANAGER: Um ou mais prefabs de inimigos específicos (Goblin, Bandit, Shaman) não foram atribuídos no Inspector. Algumas waves podem falhar.", this);
        }

        if (waves == null || waves.Count == 0)
        {
            Debug.LogError("!!! GAME MANAGER: Nenhuma wave definida na lista 'waves'! O jogo não poderá iniciar waves.", this);
            enabled = false; return;
        }

        currentWaveIndex = -1; // Garante que a primeira chamada inicie a wave 0
        // Removido: StartCoroutine(StartFirstWaveTimer()); A primeira wave agora é por botão.
        Debug.Log("GameManager iniciado. Aguardando o botão para a primeira wave.");
    }

    // >>>>>> ADIÇÃO AQUI <<<<<<
    void Update()
    {
        // Verifica se a tecla 'R' foi pressionada
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame(); // Chama o método de reinício
        }
    }
    // >>>>>> FIM DA ADIÇÃO <<<<<<


    // MÉTODO PÚBLICO PARA SER CHAMADO PELO BOTÃO DA UI
    public void CallNextWaveFromButton()
    {
        if (spawningWave)
        {
            Debug.LogWarning("GameManager: Tentativa de iniciar nova wave enquanto uma já está ativa. Aguarde.", this);
            return;
        }

        currentWaveIndex++;
        if (currentWaveIndex < waves.Count)
        {
            StartNewWave();
        }
        else
        {
            Debug.Log("GameManager: Todas as waves foram concluídas! Parabéns!", this);
            // Carregar a próxima cena se o nome da cena estiver definido
            if (!string.IsNullOrEmpty(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.LogWarning("GameManager: Nome da próxima cena não definido. Por favor, defina no Inspector.");
            }
        }
    }

    // IEnumerator StartFirstWaveTimer() // Removido ou comentado, pois a wave agora é por botão
    // {
    //     yield return new WaitForSeconds(timeBeforeFirstWave);
    //     CallNextWaveFromButton(); // Ou diretamente StartNewWave() se a primeira for automática
    // }

    void StartNewWave()
    {
        if (currentWaveIndex < 0 || currentWaveIndex >= waves.Count)
        {
            Debug.LogError($"GameManager: Tentativa de iniciar wave com índice inválido: {currentWaveIndex}", this);
            return;
        }

        if (!spawningWave)
        {
            // enemiesSpawnedThisWave = 0; // Resetado dentro do SpawnWaveCoroutine
            WaveDefinition currentWaveData = waves[currentWaveIndex];
            Debug.Log($"GameManager: Preparando para iniciar a wave '{currentWaveData.waveName}' (Índice: {currentWaveIndex}).");
            StartCoroutine(SpawnWaveCoroutine(currentWaveData));
        }
        else
        {
            Debug.LogWarning("GameManager: Tentativa de iniciar nova wave enquanto uma já está ativa (StartNewWave).", this);
        }
    }

    IEnumerator SpawnWaveCoroutine(WaveDefinition waveData)
    {
        spawningWave = true;
        enemiesEffectivelySpawnedThisWave = 0;
        totalEnemiesThisWave = 0;
        foreach (var group in waveData.enemiesToSpawn)
        {
            totalEnemiesThisWave += group.count;
        }

        Debug.Log($"GameManager: Iniciando spawn da wave '{waveData.waveName}'. Total de inimigos a spawnar: {totalEnemiesThisWave}.");

        bool usePath1 = canUsePath1; // Revalidar caminhos (embora já feito no Start)
        bool usePath2 = canUsePath2;

        if (!usePath1 && !usePath2)
        {
            Debug.LogError("!!! GAME MANAGER: Nenhum caminho válido disponível para spawn! Abortando wave.", this);
            spawningWave = false;
            yield break;
        }

        int enemySpawnCounterInWave = 0; // Para alternar caminhos

        foreach (EnemySpawnInfo enemyGroup in waveData.enemiesToSpawn)
        {
            if (enemyGroup.enemyPrefab == null)
            {
                Debug.LogWarning($"Wave Spawner: Prefab nulo no grupo da wave '{waveData.waveName}'. Pulando este grupo.", this);
                continue;
            }

            for (int i = 0; i < enemyGroup.count; i++)
            {
                GameObject prefabToSpawn = enemyGroup.enemyPrefab;
                Transform[] chosenPath = null;
                // string pathName = "N/A"; // Removed: Variable pathName was assigned but its value never used

                bool tryPath1First = (enemySpawnCounterInWave % 2 == 0);

                if (tryPath1First)
                {
                    if (usePath1) { chosenPath = path1Waypoints; /* pathName = "Path 1"; */ } // Removed assignment to pathName
                    else if (usePath2) { chosenPath = path2Waypoints; /* pathName = "Path 2 (Fallback)"; */ } // Removed assignment to pathName
                }
                else
                {
                    if (usePath2) { chosenPath = path2Waypoints; /* pathName = "Path 2"; */ } // Removed assignment to pathName
                    else if (usePath1) { chosenPath = path1Waypoints; /* pathName = "Path 1 (Fallback)"; */ } // Removed assignment to pathName
                }

                if (chosenPath == null)
                {
                    Debug.LogWarning($"Wave Spawner ({waveData.waveName}): Não foi possível determinar um caminho válido para {prefabToSpawn.name}. Pulando este inimigo.", this);
                    continue;
                }

                enemiesEffectivelySpawnedThisWave++;
                enemySpawnCounterInWave++;

                GameObject newEnemyObject = Instantiate(prefabToSpawn, Vector3.zero, Quaternion.identity);
                newEnemyObject.name = $"{prefabToSpawn.name}_{currentWaveIndex}_{enemiesEffectivelySpawnedThisWave}";

                EnemyMovement enemyMoveScript = newEnemyObject.GetComponent<EnemyMovement>();
                if (enemyMoveScript != null)
                {
                    enemyMoveScript.SetPath(chosenPath);
                    // Debug.Log($"Inimigo {newEnemyObject.name} spawnado em {pathName} (Wave: {waveData.waveName})"); // pathName would cause an error here if not removed
                }
                else
                {
                    Debug.LogError($"!!! Prefab do inimigo ({prefabToSpawn.name}) está faltando o script EnemyMovement! Destruindo instância {newEnemyObject.name}.", newEnemyObject);
                    Destroy(newEnemyObject);
                    enemiesEffectivelySpawnedThisWave--;
                    continue;
                }
                yield return new WaitForSeconds(waveData.timeBetweenSpawns);
            }
        }

        Debug.Log($"GameManager: Spawn da wave '{waveData.waveName}' concluído. {enemiesEffectivelySpawnedThisWave}/{totalEnemiesThisWave} inimigos spawnados efetivamente.");
        spawningWave = false;

        // Aqui você pode adicionar uma espera baseada em waveData.timeAfterWaveCompleted
        // se quiser um delay automático antes que o botão "Próxima Wave" seja "re-habilitado" visualmente
        // ou se as waves fossem automáticas. Como é por botão, essa lógica fica mais a cargo da UI.
    }

    public void PlayerTakesDamage(int damageAmount)
    {
        if (isGameOver) return;

        playerLives -= damageAmount;
        Debug.Log($"Jogador perdeu {damageAmount} vida(s). Vidas restantes: {playerLives}");

        if (UIManager.instance != null)
        {
            UIManager.instance.UpdateLivesDisplay(playerLives);
        }

        if (playerLives <= 0)
        {
            playerLives = 0; // Garante que não seja negativo no display
            HandleGameOver();
        }
    }

    void HandleGameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f; // Pausa o jogo
        Debug.LogError("GAME OVER! Pressione R para reiniciar.");

        if (UIManager.instance != null)
        {
            UIManager.instance.ShowGameOverScreen();
        }
    }

    public void RestartGame() // Chamado por um botão de UI ou pela tecla 'R'
    {
        Debug.Log("Reiniciando o jogo...");
        Time.timeScale = 1f; // Retoma o tempo do jogo
        isGameOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Carrega a cena atual novamente
    }

    public void AddCurrency(int amount)
    {
        currentPlayerCurrency += amount;
        Debug.Log($"Player currency updated: {currentPlayerCurrency}");
        if (UIManager.instance != null)
        {
            UIManager.instance.UpdateCurrencyDisplay(currentPlayerCurrency);
        }
    }

    public bool CanAfford(int amount)
    {
        return currentPlayerCurrency >= amount;
    }

    public void SpendCurrency(int amount)
    {
        if (CanAfford(amount))
        {
            currentPlayerCurrency -= amount;
            Debug.Log($"Spent {amount}. Player currency updated: {currentPlayerCurrency}");
            if (UIManager.instance != null)
            {
                UIManager.instance.UpdateCurrencyDisplay(currentPlayerCurrency);
            }
        }
        else
        {
            Debug.LogWarning($"Not enough currency to spend {amount}. Current: {currentPlayerCurrency}");
        }
    }
}