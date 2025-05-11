using UnityEngine;
using UnityEngine.EventSystems;

public class BuildMenuController : MonoBehaviour
{
    private bool justOpened;

    void OnEnable()
    {
        justOpened = true;
    }

    void Update()
    {
        // Ignora o primeiro clique que abriu o menu
        if (justOpened)
        {
            justOpened = false;
            return;
        }

        // Fecha ao clicar fora da UI
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            CloseMenu();
        }

        // Fecha ao pressionar ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseMenu();
        }
    }

    void CloseMenu()
    {
        gameObject.SetActive(false);
    }
}
