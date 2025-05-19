using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static TowerManager instance;

    [Header("Prefabs das Torres")]
    public GameObject fireTowerPrefab;
    public GameObject iceTowerPrefab;
    public GameObject earthTowerPrefab;
    // Adicione aqui prefabs de torres evoluídas se necessário

    private GameObject selectedTowerPrefab; // Torre selecionada pela UI para construir

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

    public GameObject GetSelectedTowerPrefab()
    {
        return selectedTowerPrefab;
    }

    // Estes métodos serão chamados pelos botões da UI
    public void SelectFireTower()
    {
        selectedTowerPrefab = fireTowerPrefab;
        Debug.Log($"Torre '{fireTowerPrefab?.name}' selecionada para construção.");
    }

    public void SelectIceTower()
    {
        selectedTowerPrefab = iceTowerPrefab;
        Debug.Log($"Torre '{iceTowerPrefab?.name}' selecionada para construção.");
    }

    public void SelectEarthTower()
    {
        selectedTowerPrefab = earthTowerPrefab;
        Debug.Log($"Torre '{earthTowerPrefab?.name}' selecionada para construção.");
    }

    public void ClearSelection()
    {
        selectedTowerPrefab = null;
    }

    public int GetTowerCost(GameObject towerPrefab)
    {
        if (towerPrefab != null)
        {
            Tower towerScript = towerPrefab.GetComponent<Tower>();
            if (towerScript != null)
            {
                return towerScript.cost;
            }
        }
        Debug.LogWarning("Tentativa de obter custo de um prefab de torre nulo ou sem script Tower.");
        return int.MaxValue; // Retorna um valor alto para indicar erro/indisponibilidade
    }

    public int GetFireTowerCost()
    {
        return GetTowerCost(fireTowerPrefab);
    }

    public int GetIceTowerCost()
    {
        return GetTowerCost(iceTowerPrefab);
    }

    public int GetEarthTowerCost()
    {
        return GetTowerCost(earthTowerPrefab);
    }
}