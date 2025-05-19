using UnityEngine;
using UnityEngine.UI; // Necessário para interagir com componentes de UI

public class ControladorImagemInicio : MonoBehaviour
{
    // Este método será chamado quando a imagem (botão) for clicada
    public void EsconderImagem()
    {
        gameObject.SetActive(false); // Desativa o GameObject ao qual este script está anexado

        // Opcional: Se você pausou o jogo enquanto a imagem estava visível, despause aqui.
        // Time.timeScale = 1f;
        Debug.Log("Imagem de início de nível escondida.");
    }

    void Start()
    {
        // Garante que a imagem esteja visível quando o objeto é ativado
        gameObject.SetActive(true);

        // Opcional: Pausar o jogo enquanto esta imagem está visível.
        // Time.timeScale = 0f;
        // Debug.Log("Imagem de início de nível mostrada. Jogo pausado.");
    }
}