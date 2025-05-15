using UnityEngine;

public class BuildMenuController : MonoBehaviour
{
    void Update()
    {
        // Fecha o menu ao pressionar ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseMenu();
        }
    }

    public void CloseMenu()
    {
        gameObject.SetActive(false);
    }
}
