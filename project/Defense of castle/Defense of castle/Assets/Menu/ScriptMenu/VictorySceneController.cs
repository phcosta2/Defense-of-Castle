using UnityEngine;
using UnityEngine.Video; // Necessário para usar VideoPlayer
using TMPro;             // Necessário para usar TextMeshPro
using System.Collections; // Necessário para usar Coroutines

public class VictorySceneController : MonoBehaviour
{
    [Header("Configurações da Cena")]
    [Tooltip("Arraste o GameObject do Video Player aqui.")]
    public VideoPlayer videoPlayer;

    [Tooltip("Arraste o GameObject do Texto de Vitória aqui.")]
    public GameObject victoryTextObject;

    [Tooltip("Se verdadeiro, espera o vídeo terminar. Se falso, usa o 'delayAfterVideoStart'.")]
    public bool waitForVideoToEnd = true;

    [Tooltip("Tempo em segundos APÓS o vídeo começar a tocar antes do texto aparecer (usado se 'waitForVideoToEnd' for falso OU como fallback).")]
    public float delayAfterVideoStart = 5.0f; // Ex: 5 segundos

    private bool videoStarted = false;

    void Start()
    {
        // Garante que o texto está desativado no início
        if (victoryTextObject != null)
        {
            victoryTextObject.SetActive(false);
        }
        else
        {
            Debug.LogError("VictorySceneController: O objeto de texto de vitória (victoryTextObject) não foi atribuído!");
            return;
        }

        if (videoPlayer == null)
        {
            Debug.LogError("VictorySceneController: O Video Player não foi atribuído!");
            return;
        }

        // Configura um listener para quando o vídeo terminar (se waitForVideoToEnd for true)
        if (waitForVideoToEnd)
        {
            videoPlayer.loopPointReached += OnVideoFinished; // Adiciona o método OnVideoFinished ao evento de término
        }
        
        // Prepara e inicia o vídeo (PlayOnAwake também deve estar marcado no VideoPlayer)
        // Se PlayOnAwake não estiver marcado, você pode chamar videoPlayer.Play() aqui.
        // videoPlayer.Prepare(); // Opcional, Play() geralmente chama Prepare() se necessário

        StartCoroutine(MonitorVideoAndShowText());
    }

    IEnumerator MonitorVideoAndShowText()
    {
        // Espera um frame para garantir que o VideoPlayer começou (se PlayOnAwake está true)
        yield return null; 
        yield return new WaitForEndOfFrame(); // Mais uma garantia

        // Espera o vídeo começar a tocar de fato
        while (!videoPlayer.isPlaying && videoPlayer.frameCount > 0 && videoPlayer.isPrepared)
        {
            // Se o vídeo não começou e não é porque está no fim, espera.
            // Isso pode acontecer se o vídeo estiver demorando para carregar.
            if (videoPlayer.time >= videoPlayer.length - 0.1 && videoPlayer.length > 0) break; // Já está no fim
            yield return null;
        }
        
        videoStarted = videoPlayer.isPlaying;

        if (!videoStarted && videoPlayer.frameCount > 0) {
            Debug.LogWarning("Vídeo não começou a tocar, mas foi preparado. Verifique as configurações do VideoPlayer.");
            // Força a tentativa de tocar se não estiver em loop e PlayOnAwake falhou por algum motivo
            if (!videoPlayer.isLooping && !videoPlayer.playOnAwake) videoPlayer.Play();
            yield return new WaitUntil(() => videoPlayer.isPlaying || (videoPlayer.time >= videoPlayer.length - 0.1 && videoPlayer.length > 0));
            videoStarted = videoPlayer.isPlaying;
        }


        if (videoStarted)
        {
            Debug.Log("Vídeo iniciado.");
            if (!waitForVideoToEnd)
            {
                // Se não estamos esperando o vídeo terminar, usamos o delay manual
                yield return new WaitForSeconds(delayAfterVideoStart);
                ShowVictoryText();
            }
            // Se waitForVideoToEnd é true, o evento OnVideoFinished cuidará de chamar ShowVictoryText.
            // No entanto, como fallback, se o evento não disparar por algum motivo (ex: vídeo sem fim definido):
            else if (videoPlayer.length > 0) // Se o vídeo tem uma duração conhecida
            {
                // Espera um pouco mais que a duração do vídeo como fallback, caso o evento loopPointReached não dispare
                // Ou se o vídeo não for configurado para loop mas não tiver um ponto final claro para o evento
                float maxWaitTime = (float)videoPlayer.length + 1.0f; // Espera 1s a mais que a duração
                float waitedTime = 0f;
                while(videoPlayer.isPlaying && waitedTime < maxWaitTime) {
                    waitedTime += Time.deltaTime;
                    yield return null;
                }
                // Se o texto ainda não foi mostrado (OnVideoFinished não foi chamado)
                if (victoryTextObject != null && !victoryTextObject.activeSelf)
                {
                    Debug.Log("Fallback: Mostrando texto de vitória após o tempo máximo de espera do vídeo.");
                    ShowVictoryText();
                }
            }
            else // Vídeo sem duração definida (streaming, etc.) e waitForVideoToEnd é true
            {
                 Debug.LogWarning("Esperando o vídeo terminar, mas o vídeo não tem uma duração definida. Usando 'delayAfterVideoStart' como fallback.");
                 yield return new WaitForSeconds(delayAfterVideoStart);
                 ShowVictoryText();
            }
        }
        else
        {
            Debug.LogWarning("O vídeo não iniciou. Mostrando texto de vitória após delay padrão como fallback.");
            yield return new WaitForSeconds(delayAfterVideoStart); // Fallback se o vídeo não tocar
            ShowVictoryText();
        }
    }

    // Chamado quando o evento loopPointReached do VideoPlayer é disparado
    void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("Evento OnVideoFinished disparado.");
        if (waitForVideoToEnd)
        {
            ShowVictoryText();
            // Remove o listener para não ser chamado novamente se algo reativar o vídeo
            videoPlayer.loopPointReached -= OnVideoFinished;
        }
    }

    void ShowVictoryText()
    {
        if (victoryTextObject != null && !victoryTextObject.activeSelf) // Garante que só ativa uma vez
        {
            // Parar o vídeo se ainda estiver tocando
            if (videoPlayer != null && videoPlayer.isPlaying)
            {
                videoPlayer.Stop();
            }

            // **** ADICIONE ESTA LINHA ****
            // Desativa o GameObject do VideoPlayer para que ele pare de renderizar
            if (videoPlayer != null && videoPlayer.gameObject != null)
            {
                videoPlayer.gameObject.SetActive(false); // <<< IMPORTANTE
                Debug.Log("VideoPlayer GameObject desativado.");
            }

            victoryTextObject.SetActive(true);
            Debug.Log("Texto de vitória exibido!");
        }
    }


    // Desregistrar o evento ao destruir o objeto para evitar erros
    void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoFinished;
        }
    }
}