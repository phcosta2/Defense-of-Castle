using UnityEngine;

public enum TowerType {
    Fire,
    Water,
    Rock,
    Ice
}

public class Slots : MonoBehaviour {
    public GameObject buildMenuUI;
    public GameObject fireTowerPrefab;
    public GameObject waterTowerPrefab;
    public GameObject rockTowerPrefab;
    public GameObject iceTowerPrefab;

    private bool isOccupied = false;

    void Start() {
        // Garante que o menu começa desativado
        if (buildMenuUI != null)
            buildMenuUI.SetActive(false);
    }

    void OnMouseDown() {
        if (!isOccupied && buildMenuUI != null) {
            // Ativa o menu
            buildMenuUI.SetActive(true);

            // Converte a posição do slot no mundo para posição de tela
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            buildMenuUI.transform.position = screenPos;

            // Informa ao TowerBuildMenu qual slot está sendo usado
            TowerBuildMenu.Instance.SetCurrentSlot(this);
        }
    }

    public void BuildTower(TowerType type) {
        if (isOccupied) return;

        GameObject prefabToBuild = null;

        switch (type) {
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

        if (prefabToBuild != null) {
            Instantiate(prefabToBuild, transform.position, Quaternion.identity);
            isOccupied = true;

            // Esconde o menu após construir
            if (buildMenuUI != null)
                buildMenuUI.SetActive(false);
        }
    }
}
