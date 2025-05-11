using UnityEngine;
using UnityEngine.EventSystems;

public class CloseMenuOnClickOutside : MonoBehaviour
{
    void Update()
    {
        // Detecta clique do mouse
        if (Input.GetMouseButtonDown(0))
        {
            // Se o clique NÃO foi sobre UI
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                gameObject.SetActive(false); // Esconde o menu
            }
        }
    }
}