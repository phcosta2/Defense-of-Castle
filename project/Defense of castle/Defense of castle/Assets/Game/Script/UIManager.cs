using UnityEngine;
using UnityEngine.UI;
using TMPro; // Para TextMeshPro
using System.Collections.Generic; // Para List
//using System.Linq; // Se necessário para FirstOrDefault em outras partes

// Commented out old top-level UIFlagSlot, will use nested one
// [System.Serializable]
// public class UIFlagSlot
// {
//    public Button uiButton;
//    public Transform worldBuildSpot;
//    public SpriteRenderer worldFlagSprite;
//    [HideInInspector] public bool isOccupied = false;
//    [HideInInspector] public GameObject builtTowerInstance = null;
// }

public class UIManager : MonoBehaviour
{
    // Nested UIFlagSlot class definition
    [System.Serializable]
    public class UIFlagSlot
    {
        public Button uiButton;             // O botão da UI (FlagButton0)
        public Transform worldBuildSpot;    // O PontoConstrucao0 onde a torre será construída
        public SpriteRenderer worldFlagSprite; // Sprite da flag no local de construção
        [HideInInspector] public bool isOccupied = false;
        [HideInInspector] public GameObject builtTowerInstance = null;
    }

    public static UIManager instance;

    [Header("Painéis")]
    public GameObject towerSelectionPanel;
    public GameObject towerUpgradeSellPanel; // Novo painel
    public GameObject gameOverPanel;

    [Header("Botões de Seleção de Torre (UI)")]
    public Button fireTowerButton;
    public Button iceTowerButton;
    public Button earthTowerButton;

    [Header("Textos de Custo das Torres (UI)")]
    public TextMeshProUGUI fireTowerCostText;
    public TextMeshProUGUI iceTowerCostText;
    public TextMeshProUGUI earthTowerCostText;

    [Header("Elementos do Painel de Upgrade/Venda")]
    public TextMeshProUGUI towerLevelText;
    public TextMeshProUGUI upgradeCostText;
    public TextMeshProUGUI sellValueText;
    public Button upgradeButton;
    public Button sellButton;
    public Button closeUpgradePanelButton; // Para fechar o painel de upgrade/venda

    [Header("Display de Status do Jogador (UI)")]
    public TextMeshProUGUI playerCurrencyText;
    public TextMeshProUGUI playerLivesText;
    public TextMeshProUGUI waveInfoText; // Para mostrar a wave atual/próxima

    [Header("Outros Elementos UI")]
    public Button nextWaveButton;
    public TextMeshProUGUI temporaryMessageText; // Para feedback (ex: "Dinheiro insuficiente")
    private float messageTimer = 0f;

    [Header("Flag Slots Management")] // Added for clarity
    public List<UIFlagSlot> flagSlots = new List<UIFlagSlot>(); // For FlagButton logic
    private UIFlagSlot currentActiveSlot; // For FlagButton logic, distinct from currentSelectedSlot
    
    private TowerPlacementSlot currentSelectedSlot; // Slot de construção ativo (vazio) - for direct world clicks
    private Tower currentSelectedTowerForUpgrade;   // Torre selecionada para upgrade/venda

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // Esconder painéis no início
        if (towerSelectionPanel != null) towerSelectionPanel.SetActive(false);
        if (towerUpgradeSellPanel != null) towerUpgradeSellPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (temporaryMessageText != null) temporaryMessageText.gameObject.SetActive(false);

        // Configurar listeners para os botões de torre (para construir)
        SetupBuildButtonListeners();

        // Configurar listeners para os botões do painel de upgrade/venda
        SetupUpgradeSellPanelListeners();

        // Configurar listeners para os botões de flag
        SetupFlagButtonListeners(); // Added call

        UpdateTowerCostsUI();
        if (GameManager.instance != null)
        {
            UpdateCurrencyDisplay(GameManager.instance.currentPlayerCurrency);
            UpdateLivesDisplay(GameManager.instance.playerLives);
            if (GameManager.instance.waves != null && GameManager.instance.waves.Count > 0) // Check waves list
            {
                UpdateWaveInfo(0, GameManager.instance.waves.Count); // Exemplo inicial
            }
            else
            {
                UpdateWaveInfo(0,0); // Handle case with no waves defined
            }
        }
    }
    
        void Update()
        {
            if (messageTimer > 0)
            {
                messageTimer -= Time.deltaTime;
                if (messageTimer <= 0)
                {
                    if (temporaryMessageText != null) temporaryMessageText.gameObject.SetActive(false);
                }
            }
        }
    
    
        void SetupBuildButtonListeners()
        {
            if (fireTowerButton != null && TowerManager.instance != null)
            {
                fireTowerButton.onClick.AddListener(() => {
                    AttemptToBuildSelectedTower(TowerManager.instance.fireTowerPrefab);
                });
            }
            if (iceTowerButton != null && TowerManager.instance != null)
            {
                iceTowerButton.onClick.AddListener(() => {
                    AttemptToBuildSelectedTower(TowerManager.instance.iceTowerPrefab);
                });
            }
            if (earthTowerButton != null && TowerManager.instance != null)
            {
                earthTowerButton.onClick.AddListener(() => {
                    AttemptToBuildSelectedTower(TowerManager.instance.earthTowerPrefab);
                });
            }
        }
    
        void SetupUpgradeSellPanelListeners()
        {
            if (upgradeButton != null)
            {
                upgradeButton.onClick.AddListener(() => {
                    if (currentSelectedTowerForUpgrade != null)
                    {
                        currentSelectedTowerForUpgrade.UpgradeTower();
                        // UpdateUpgradePanel(currentSelectedTowerForUpgrade); // O próprio Tower.UpgradeTower pode chamar isso
                    }
                });
            }
            if (sellButton != null)
            {
                sellButton.onClick.AddListener(() => {
                    if (currentSelectedSlot != null && currentSelectedTowerForUpgrade != null) // Verifica se o slot original também é conhecido
                    {
                        currentSelectedSlot.SellTower(); // TowerPlacementSlot agora tem SellTower
                        HideTowerUpgradeSellPanel();
                    }
                });
            }
            if (closeUpgradePanelButton != null)
            {
                closeUpgradePanelButton.onClick.AddListener(HideTowerUpgradeSellPanel);
            }
        }
    
        // Chamado por TowerPlacementSlot quando um slot VAZIO é clicado
        public void HandleEmptySlotClick(TowerPlacementSlot slot)
        {
            currentSelectedSlot = slot;
            currentSelectedTowerForUpgrade = null; // Garante que não há torre selecionada para upgrade
            ShowTowerSelectionPanel();
            HideTowerUpgradeSellPanel(); // Esconde o outro painel se estiver aberto
        }
    
        // Chamado por TowerPlacementSlot quando um slot OCUPADO é clicado
        public void HandleOccupiedSlotClick(TowerPlacementSlot slot, Tower towerScript)
        {
            currentSelectedSlot = slot; // Guarda o slot para o caso de venda
            currentSelectedTowerForUpgrade = towerScript;
            UpdateUpgradePanel(towerScript);
            ShowTowerUpgradeSellPanel();
            HideTowerSelectionPanel(); // Esconde o outro painel se estiver aberto
        }
    
        void SetupFlagButtonListeners()
        {
            foreach (var flagSlot in flagSlots)
            {
                if (flagSlot.uiButton != null)
                {
                    // Remove existing listeners to prevent duplicates if called multiple times
                    flagSlot.uiButton.onClick.RemoveAllListeners(); 
                    flagSlot.uiButton.onClick.AddListener(() => HandleFlagButtonClick(flagSlot));
                }
            }
        }
    
        void HandleFlagButtonClick(UIFlagSlot slot)
        {
            if (slot.isOccupied)
            {
                // Se já tem uma torre, seleciona para upgrade/venda
                if (slot.builtTowerInstance != null)
                {
                    Tower towerScript = slot.builtTowerInstance.GetComponent<Tower>();
                    if (towerScript != null)
                    {
                        currentSelectedTowerForUpgrade = towerScript; // Set for upgrade panel
                        currentSelectedSlot = null; // Clear direct world slot selection
                        UpdateUpgradePanel(towerScript);
                        ShowTowerUpgradeSellPanel();
                        HideTowerSelectionPanel();
                    }
                }
            }
            else
            {
                // Se está vazio, mostra painel de seleção de torre
                currentActiveSlot = slot; // Set for building via flag button
                currentSelectedSlot = null; // Clear direct world slot selection
                ShowTowerSelectionPanel();
                HideTowerUpgradeSellPanel();
            }
        }
    
        // This AttemptToBuildSelectedTower is called by SetupBuildButtonListeners
        // It needs to know whether it's building via a direct world click (currentSelectedSlot)
        // or a flag button click (currentActiveSlot).
        // The version from your file seems to be for currentActiveSlot (UIFlagSlot).
        void AttemptToBuildSelectedTower(GameObject towerPrefab)
        {
            // Logic for building via FlagButton (currentActiveSlot is UIFlagSlot)
            if (currentActiveSlot != null && !currentActiveSlot.isOccupied)
            {
                if (towerPrefab == null)
                {
                    Debug.LogWarning("UIManager: No tower prefab selected for building.");
                    HideTowerSelectionPanel();
                    return;
                }
                int cost = TowerManager.instance.GetTowerCost(towerPrefab);
                if (GameManager.instance != null && !GameManager.instance.CanAfford(cost))
                {
                    ShowTemporaryMessage("Moedas insuficientes!");
                    return;
                }
    
                Vector3 buildPosition = currentActiveSlot.worldBuildSpot.position;
                GameObject newTower = Instantiate(towerPrefab, buildPosition, Quaternion.identity);
                
                currentActiveSlot.isOccupied = true;
                currentActiveSlot.builtTowerInstance = newTower;
                
                if (GameManager.instance != null)
                {
                    GameManager.instance.SpendCurrency(cost);
                }
                if(currentActiveSlot.worldFlagSprite != null) currentActiveSlot.worldFlagSprite.enabled = false;
    
                HideTowerSelectionPanel();
                // currentActiveSlot = null; // Keep it active if you want to build multiple towers without re-clicking flag
                                     // Or set to null if one build per flag click.
            }
            // Logic for building via direct world click (currentSelectedSlot is TowerPlacementSlot)
            else if (currentSelectedSlot != null && !currentSelectedSlot.IsOccupied())
            {
                if (towerPrefab == null)
                {
                    Debug.LogWarning("UIManager: No tower prefab selected for building.");
                    HideTowerSelectionPanel();
                    return;
                }
                int cost = TowerManager.instance.GetTowerCost(towerPrefab);
                if (GameManager.instance != null && !GameManager.instance.CanAfford(cost))
                {
                    ShowTemporaryMessage("Moedas insuficientes!");
                    return;
                }
                currentSelectedSlot.BuildTower(towerPrefab); // TowerPlacementSlot handles instantiation and cost
                HideTowerSelectionPanel();
                // currentSelectedSlot = null; // TowerPlacementSlot might handle this or UIManager does
            }
            else
            {
                Debug.LogWarning("UIManager: No valid build slot selected (neither FlagSlot nor WorldSlot).");
                ShowTemporaryMessage("Selecione um local válido primeiro!");
                HideTowerSelectionPanel();
                return;
            }
        }
    
        public void UpdateUpgradePanel(Tower tower)
        {
            if (tower == null || towerUpgradeSellPanel == null) return;
    
            currentSelectedTowerForUpgrade = tower; // Atualiza a referência
    
            if (towerLevelText != null) towerLevelText.text = $"Nível: {tower.currentLevel}";
    
            bool canUpgrade = tower.CanUpgrade();
            if (upgradeButton != null) upgradeButton.interactable = canUpgrade;
    
            if (upgradeCostText != null)
            {
                if (canUpgrade)
                {
                    int cost = tower.GetUpgradeCost();
                    upgradeCostText.text = $"Upgrade: {cost} Moedas";
                    if (GameManager.instance != null && !GameManager.instance.CanAfford(cost))
                    {
                         upgradeButton.interactable = false; // Desabilita se não pode pagar
                    }
                }
                else
                {
                    upgradeCostText.text = "Nível Máximo";
                }
            }
    
            if (sellValueText != null)
            {
                // Custo base da torre * porcentagem de venda
                int sellValue = Mathf.FloorToInt(tower.cost * 0.75f); // Exemplo: 75% do custo inicial
                // Você pode querer adicionar uma parte do custo dos upgrades já feitos
                // int totalUpgradeCost = 0;
                // for (int i = 0; i < tower.currentLevel - 1; i++) totalUpgradeCost += tower.upgradeCosts[i];
                // sellValue += Mathf.FloorToInt(totalUpgradeCost * 0.5f); // Ex: 50% do custo dos upgrades
                sellValueText.text = $"Vender: {sellValue} Moedas";
            }
        }
    
    
        public void ShowTowerSelectionPanel()
        {
            if (towerSelectionPanel != null)
            {
                towerSelectionPanel.SetActive(true);
                UpdateTowerCostsUI();
            }
        }
    
        public void HideTowerSelectionPanel()
        {
            if (towerSelectionPanel != null) towerSelectionPanel.SetActive(false);
            // Não limpe currentSelectedSlot aqui, pois pode ser necessário se o jogador fechar o painel sem construir
        }
    
        public void ShowTowerUpgradeSellPanel()
        {
            if (towerUpgradeSellPanel != null) towerUpgradeSellPanel.SetActive(true);
        }
    
        public void HideTowerUpgradeSellPanel()
        {
            if (towerUpgradeSellPanel != null) towerUpgradeSellPanel.SetActive(false);
            currentSelectedTowerForUpgrade = null;
            currentSelectedSlot = null; // Limpa slot selecionado ao fechar painel de upgrade/venda
        }
    
        public void UpdateTowerCostsUI()
        {
            if (TowerManager.instance == null) return;
    
            if (fireTowerCostText != null)
                fireTowerCostText.text = $"{TowerManager.instance.GetFireTowerCost()}";
            if (iceTowerCostText != null)
                iceTowerCostText.text = $"{TowerManager.instance.GetIceTowerCost()}";
            if (earthTowerCostText != null)
                earthTowerCostText.text = $"{TowerManager.instance.GetEarthTowerCost()}";
        }
    
        public void UpdateCurrencyDisplay(int newAmount)
        {
            if (playerCurrencyText != null)
            {
                playerCurrencyText.text = $"Moedas: {newAmount}";
            }
        }
    
        public void UpdateLivesDisplay(int lives)
        {
            if (playerLivesText != null)
            {
                playerLivesText.text = $"Vidas: {lives}";
            }
        }
        
        public void UpdateWaveInfo(int currentWaveNum, int totalWaves) // Chamado pelo GameManager
        {
            if (waveInfoText != null)
            {
                if (currentWaveNum < totalWaves)
                {
                     waveInfoText.text = $"Wave: {currentWaveNum + 1}/{totalWaves}";
                } else {
                     waveInfoText.text = "Todas as Waves Concluídas!";
                }
            }
            if (nextWaveButton != null)
            {
                nextWaveButton.interactable = (currentWaveNum < totalWaves -1); // Desabilita se for a última wave
                 // Ou se uma wave estiver em progresso (GameManager.spawningWave)
                if (GameManager.instance != null && GameManager.instance.spawningWave) {
                    // nextWaveButton.interactable = false; // Comentado para permitir chamar wave enquanto outra spawna
                }
            }
        }
    
    
        public void ShowGameOverScreen()
        {
            if (gameOverPanel != null) gameOverPanel.SetActive(true);
            if (nextWaveButton != null) nextWaveButton.interactable = false; // Desabilita botão de próxima wave
        }
    
        public void ShowTemporaryMessage(string message, float duration = 2f)
        {
            if (temporaryMessageText != null)
            {
                temporaryMessageText.text = message;
                temporaryMessageText.gameObject.SetActive(true);
                messageTimer = duration;
            }
        }
    
        public Tower GetSelectedTowerForUpgrade()
        {
            return currentSelectedTowerForUpgrade;
        }
}

        