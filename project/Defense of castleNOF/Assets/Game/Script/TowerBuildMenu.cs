using UnityEngine;

public class TowerBuildMenu : MonoBehaviour {
    public static TowerBuildMenu Instance;

    private Slots currentSlot;

    void Awake() {
        Instance = this;
    }

    public void SetCurrentSlot(Slots slot) {
        currentSlot = slot;
    }

    public void OnClickBuildFire() {
        currentSlot.BuildTower(TowerType.Fire);
    }

    public void OnClickBuildWater() {
        currentSlot.BuildTower(TowerType.Water);
    }

    public void OnClickBuildRock() {
        currentSlot.BuildTower(TowerType.Rock);
    }

    public void OnClickBuildIce() {
        currentSlot.BuildTower(TowerType.Ice);
    }


}
