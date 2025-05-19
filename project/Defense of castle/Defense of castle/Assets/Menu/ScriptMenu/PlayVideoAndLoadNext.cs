using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video; // Necessário para o VideoPlayer

[RequireComponent(typeof(VideoPlayer))]
public class PlayVideoAndLoadNext : MonoBehaviour
{
    public string nextSceneName; // Nome da cena a ser carregada após o vídeo (ex: "Lv1")
    private VideoPlayer videoPlayer;

    void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }

    void Start()
    {
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer não encontrado neste GameObject.");
            return;
        }

        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogError("O nome da próxima cena (nextSceneName) não foi definido no Inspector.");
            return;
        }

        // Garante que o vídeo não está em loop para que o evento loopPointReached funcione como "fim do vídeo"
        videoPlayer.isLooping = false;

        // Inscreve-se no evento que é chamado quando o vídeo termina (ou atinge o ponto de loop)
        videoPlayer.loopPointReached += OnVideoEnd;

        // Inicia o vídeo
        videoPlayer.Play();
        Debug.Log($"Iniciando vídeo: {videoPlayer.clip.name}. Próxima cena: {nextSceneName}");
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log($"Vídeo {vp.clip.name} terminou. Carregando cena: {nextSceneName}");
        SceneManager.LoadScene(nextSceneName);
    }

    // É uma boa prática remover o listener quando o objeto é destruído
    void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoEnd;
        }
    }
}