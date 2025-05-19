// Assets/Game/Script/TowerPlacementSlot.cs

using UnityEngine;
using UnityEngine.EventSystems; 

public class TowerPlacementSlot : MonoBehaviour
{
    private SpriteRenderer slotSpriteRenderer;
    private GameObject currentTowerInstance = null; 
    private Tower currentTowerScript = null; 

    // public Color hoverColor; 
    // private Color originalColor; 

    void Start()
    {
        slotSpriteRenderer = GetComponent<SpriteRenderer>();
        if (slotSpriteRenderer == null)
        {
            slotSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (slotSpriteRenderer == null)
            {
                Debug.LogWarning($"TowerPlacementSlot '{name}': SpriteRenderer not found on self or children. Flag might not be visible/controllable.");
            }
        }
        // if (slotSpriteRenderer != null)
        // {
        //     originalColor = slotSpriteRenderer.color;
        // }
    }

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (UIManager.instance == null)
        {
            Debug.LogError("TowerPlacementSlot: UIManager.instance is null.");
            return;
        }

        if (IsOccupied())
        {
            // Debug.Log($"Slot {gameObject.name} occupied. Opening upgrade panel for {currentTowerInstance.name}.");
            UIManager.instance.HandleOccupiedSlotClick(this, currentTowerScript);
        }
        else
        {
            // Debug.Log($"Slot {gameObject.name} empty. Opening tower selection panel.");
            UIManager.instance.HandleEmptySlotClick(this);
        }
    }

    public bool BuildTower(GameObject towerPrefabToBuild) // Return bool for success
    {
        if (IsOccupied())
        {
            Debug.LogWarning($"TowerPlacementSlot '{name}': Attempt to build in an already occupied slot.");
            return false;
        }

        if (towerPrefabToBuild == null)
        {
            Debug.LogWarning("TowerPlacementSlot: No tower prefab provided to build.");
            return false;
        }

        Tower towerComponent = towerPrefabToBuild.GetComponent<Tower>();
        if (towerComponent == null)
        {
            Debug.LogError($"TowerPlacementSlot: Prefab '{towerPrefabToBuild.name}' lacks 'Tower' component.");
            return false;
        }
        
        if (GameManager.instance == null)
        {
            Debug.LogError("TowerPlacementSlot: GameManager instance not found. Cannot check/spend currency.");
            return false;
        }

        if (GameManager.instance.CanAfford(towerComponent.cost))
        {
            GameManager.instance.SpendCurrency(towerComponent.cost); // Spend first

            currentTowerInstance = Instantiate(towerPrefabToBuild, transform.position, Quaternion.identity);
            currentTowerScript = currentTowerInstance.GetComponent<Tower>(); 
            currentTowerInstance.name = $"{towerPrefabToBuild.name}_on_{gameObject.name}";

            Debug.Log($"Tower '{currentTowerInstance.name}' built on slot '{gameObject.name}' for {towerComponent.cost} currency.");

            if (slotSpriteRenderer != null)
            {
                slotSpriteRenderer.enabled = false;
            }
            return true;
        }
        else
        {
            // UIManager will be notified by GameManager.SpendCurrency if it fails due to cost
            // Debug.LogWarning($"TowerPlacementSlot: Cannot build '{towerPrefabToBuild.name}'. Insufficient funds.");
            // UIManager.instance.ShowTemporaryMessage("Moedas insuficientes!"); // GameManager.SpendCurrency does this now
            return false;
        }
    }

    public void SellTower()
    {
        if (!IsOccupied() || currentTowerScript == null)
        {
            Debug.LogWarning($"TowerPlacementSlot '{name}': Cannot sell. Slot is empty or tower script missing.");
            return;
        }
        if (GameManager.instance == null)
        {
            Debug.LogError("TowerPlacementSlot: GameManager instance not found. Cannot add currency for selling.");
            return;
        }

        int sellValue = currentTowerScript.GetSellValue(); // Assumes Tower script has GetSellValue()
        
        GameManager.instance.AddCurrency(sellValue);
        Debug.Log($"Tower '{currentTowerInstance.name}' sold for {sellValue} from slot '{name}'.");

        Destroy(currentTowerInstance);
        currentTowerInstance = null;
        currentTowerScript = null;

        if (slotSpriteRenderer != null)
        {
            slotSpriteRenderer.enabled = true; 
        }
    }

    public bool IsOccupied()
    {
        return currentTowerInstance != null;
    }

    public Tower GetTowerScript()
    {
        return currentTowerScript;
    }

    // Optional:
    // void OnMouseEnter()
    // {
    //     if (slotSpriteRenderer != null && !EventSystem.current.IsPointerOverGameObject())
    //     {
    //         if (!IsOccupied()) slotSpriteRenderer.color = hoverColor;
    //     }
    // }

    // void OnMouseExit()
    // {
    //     if (slotSpriteRenderer != null)
    //     {
    //         slotSpriteRenderer.color = originalColor;
    //     }
    // }
}