using UnityEngine;

public class TowerBuildMenu : MonoBehaviour
{
    public static TowerBuildMenu Instance;

    private Slots currentSlot;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("[TowerBuildMenu] Instância criada: " + gameObject.name);
        }
        else if (Instance != this)
        {
            Debug.LogWarning("[TowerBuildMenu] Instância duplicada detectada. Destruindo objeto extra.");
            Destroy(gameObject);
        }
        // Não fechamos o menu aqui, pois pode não funcionar se o objeto já estiver ativo na cena
    }

    void Start()
    {
        // Fecha o menu no início da cena, mesmo que o GameObject esteja ativo no Hierarchy
        gameObject.SetActive(false);
    }

    public void SetCurrentSlot(Slots slot)
    {
        currentSlot = slot;
        Debug.Log($"[TowerBuildMenu] SetCurrentSlot chamado para slot: {slot.gameObject.name}, menu ativado.");
        gameObject.SetActive(true); // Abre o menu
    }

    public void OnClickBuildFire()
    {
        Debug.Log("[TowerBuildMenu] OnClickBuildFire chamado.");
        if (currentSlot == null)
        {
            Debug.LogWarning("[TowerBuildMenu] currentSlot é null no OnClickBuildFire!");
            return;
        }
        currentSlot.BuildTower(TowerType.Fire);
        CloseMenu();
    }

    public void OnClickBuildWater()
    {
        Debug.Log("[TowerBuildMenu] OnClickBuildWater chamado.");
        if (currentSlot == null)
        {
            Debug.LogWarning("[TowerBuildMenu] currentSlot é null no OnClickBuildWater!");
            return;
        }
        currentSlot.BuildTower(TowerType.Water);
        CloseMenu();
    }

    public void OnClickBuildRock()
    {
        Debug.Log("[TowerBuildMenu] OnClickBuildRock chamado.");
        if (currentSlot == null)
        {
            Debug.LogWarning("[TowerBuildMenu] currentSlot é null no OnClickBuildRock!");
            return;
        }
        currentSlot.BuildTower(TowerType.Rock);
        CloseMenu();
    }

    public void OnClickBuildIce()
    {
        Debug.Log("[TowerBuildMenu] OnClickBuildIce chamado.");
        if (currentSlot == null)
        {
            Debug.LogWarning("[TowerBuildMenu] currentSlot é null no OnClickBuildIce!");
            return;
        }
        currentSlot.BuildTower(TowerType.Ice);
        CloseMenu();
    }

    public void CloseMenu()
    {
        Debug.Log("[TowerBuildMenu] Fechando menu.");
        gameObject.SetActive(false);
        currentSlot = null;
    }
}
