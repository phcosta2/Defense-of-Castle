// Conteúdo original que você tinha para PhaseScript.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhaseScript : MonoBehaviour // Certifique-se que o nome da classe corresponde ao nome do arquivo
{
    public void Fase1()
    {
        Debug.Log("Botão Fase 1 Clicado! Carregando cena de vídeo VideoFase1...");
        SceneManager.LoadScene("VideoFase1"); // Alterado de "Lv1"
    }

    public void Fase2()
    {
        Debug.Log("Botão Fase 2 Clicado! Carregando cena de vídeo VideoFase2...");
        SceneManager.LoadScene("VideoFase2"); // Alterado de "Lv2"
    }
    public void Fase3()
    {
        Debug.Log("Botão Fase 3 Clicado!");
        // Se você tiver uma cena de vídeo para a Fase 3, por exemplo "VideoFase3":
        // SceneManager.LoadScene("VideoFase3");
        // Caso contrário, se for direto para o nível:
        SceneManager.LoadScene("Lv3");
    }
    public void Fase4()
    {
        Debug.Log("Botão Fase 4 Clicado!");
        // Similar à Fase 3, adapte conforme necessário
        SceneManager.LoadScene("Lv4");
    }

    public void Fase5()
    {
        Debug.Log("Botão Fase 5 Clicado!");
        // Similar à Fase 3, adapte conforme necessário
        SceneManager.LoadScene("Lv5");
    }
}