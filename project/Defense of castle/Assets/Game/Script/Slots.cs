using UnityEngine;

public enum TowerType {
    Fire,
    Water,
    Rock,
    Ice
}

public class Slots : MonoBehaviour
{
    public GameObject fireTowerPrefab;
    public GameObject waterTowerPrefab;
    public GameObject rockTowerPrefab;
    public GameObject iceTowerPrefab;

    private bool isOccupied = false;

void OnMouseDown()
{
    if (isOccupied) return;

    if (TowerBuildMenu.Instance == null)
    {
        Debug.LogError("TowerBuildMenu.Instance está null!");
        return;
    }

    Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position);

    RectTransform menuRect = TowerBuildMenu.Instance.GetComponent<RectTransform>();

    Vector2 anchoredPos;
    RectTransformUtility.ScreenPointToLocalPointInRectangle(
        menuRect.parent as RectTransform, screenPos, null, out anchoredPos);

    menuRect.anchoredPosition = anchoredPos;

    TowerBuildMenu.Instance.SetCurrentSlot(this);
}

public void BuildTower(TowerType type)
{
    if (isOccupied)
    {
        Debug.Log("[Slots] Tentativa de construir torre em slot ocupado.");
        return;
    }

    MoneyManager moneyManager = FindObjectOfType<MoneyManager>();
    if (moneyManager == null)
    {
        Debug.LogError("[Slots] MoneyManager não encontrado!");
        return;
    }

    int cost = 0;

    switch (type)
    {
        case TowerType.Fire:
            cost = 50;
            break;
        // adicione os custos das outras torres aqui
        case TowerType.Water:
            cost = 40; // exemplo
            break;
        case TowerType.Rock:
            cost = 30; // exemplo
            break;
        case TowerType.Ice:
            cost = 45; // exemplo
            break;
    }

    if (!moneyManager.SpendMoney(cost))
    {
        Debug.Log("[Slots] Dinheiro insuficiente para construir torre.");
        return;
    }

    GameObject prefabToBuild = null;

    switch (type)
    {
        case TowerType.Fire:
            prefabToBuild = fireTowerPrefab;
            break;
        case TowerType.Water:
            prefabToBuild = waterTowerPrefab;
            break;
        case TowerType.Rock:
            prefabToBuild = rockTowerPrefab;
            break;
        case TowerType.Ice:
            prefabToBuild = iceTowerPrefab;
            break;
    }

    if (prefabToBuild != null)
    {
        Instantiate(prefabToBuild, transform.position, Quaternion.identity);
        isOccupied = true;

        // Desativa visual do slot
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.enabled = false;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
    }
    else
    {
        Debug.LogError("[Slots] prefabToBuild é null!");
    }
}
}
