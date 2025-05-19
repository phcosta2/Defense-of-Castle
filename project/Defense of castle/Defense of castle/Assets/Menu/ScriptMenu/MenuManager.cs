using UnityEngine;
using UnityEngine.SceneManagement; // Essencial para carregar cenas
using UnityEngine.Video; // Necessário para VideoClip

public class MenuManager : MonoBehaviour
{
    // --- Variáveis para Videoclipes ---
    // Estas variáveis de VideoClip não são mais necessárias aqui se cada cena de vídeo
    // gerencia seu próprio VideoPlayer e PlayVideoAndLoadNext.
    // public VideoClip videoFase1; 
    // public VideoClip videoFase2; 


    /// <summary>
    /// Carrega uma cena pelo nome.
    /// </summary>
    /// <param name="sceneName">O nome da cena a ser carregada (deve estar nas Build Settings).</param>
    public void LoadScene(string sceneName)
    {
        Debug.Log($"Tentando carregar a cena: {sceneName}");
        // Lógica para resetar o jogo/mapa se estiver carregando um nível
        // Isso é uma simplificação. O ideal é ter um GameManager para cuidar do estado do jogo.
        if (sceneName.StartsWith("Lv") || sceneName == "SuaCenaDeJogoPrincipal")
        {
            Debug.Log("Iniciando novo jogo/nível. Lógica de reset de mapa/estado do jogo deve ser aplicada aqui ou na cena carregada.");
            // Exemplo: PlayerPrefs.DeleteAll(); // Cuidado, isso apaga TODOS os PlayerPrefs
            // Ou chamar uma função de um GameManager: GameManager.Instance.ResetGameState();
        }
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Volta para o Menu Principal.
    /// </summary>
    public void GoToMainMenu()
    {
        // Certifique-se de que "MainMenu" é o nome exato da sua cena de menu principal
        LoadScene("MainMenu"); // Ou qualquer que seja o nome da sua cena de Menu Principal
    }

    /// <summary>
    /// Fecha o jogo.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Botão Sair Clicado!");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    // --- Funções do Menu Principal (Adaptadas do seu MainMenu.cs) ---

    public void NewGame_OpenLevelSelect() // Renomeado para clareza
    {
        Debug.Log("Botão Novo Jogo Clicado! Abrindo seleção de fases...");
        LoadScene("SelectMenu"); // Carrega a cena de seleção de fases
    }

    public void LoadSavedGame() // Renomeado para clareza
    {
        Debug.Log("Botão Carregar Jogo Clicado!");
        // TODO: Lógica para carregar um jogo salvo
        // Exemplo: LoadScene("NomeDaCenaCarregada");
        // Ou ativar um painel de seleção de saves.
    }

    public void OpenSettings()
    {
        Debug.Log("Botão Configurações Clicado!");
        // TODO: Lógica para abrir o painel/menu de configurações
        // Exemplo: if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    public void ShowCredits()
    {
        Debug.Log("Botão Créditos Clicado!");
        // TODO: Lógica para mostrar os créditos
        // Exemplo: if (creditsPanel != null) creditsPanel.SetActive(true);
        // Ou: LoadScene("CreditsScene");
    }

    // Função para carregar a Fase 1 diretamente do MainMenu (MODIFICADA)
    public void MainMenu_LoadFase1()
    {
        Debug.Log("Botão Fase 1 (MainMenu) Clicado! Carregando vídeo para Lv1...");
        // Em vez de VideoManager, carregamos a cena de vídeo diretamente.
        // O script PlayVideoAndLoadNext.cs na cena "VideoFase1" cuidará de tocar o vídeo
        // e depois carregar "Lv1".
        LoadScene("VideoFase1"); 
    }


    // --- Funções de Seleção de Fase (Adaptadas do seu PhaseScript.cs) ---
    // Essas funções seriam chamadas por botões na cena "SelectMenu"
    
    public void SelectFase1()
    {
        Debug.Log("Botão Fase 1 (SelectMenu) ");
        LoadScene("Dialogo1"); 
    }

    public void SelectFase2()
    {
        Debug.Log("Botão Fase 2 (SelectMenu) ");
        LoadScene("Dialogo2");
    }

    public void SelectFase3()
    {
        Debug.Log("Botão Fase 3 (SelectMenu) ");
        LoadScene("Lv3");
    }

    public void SelectFase4()
    {
        Debug.Log("Botão Fase 4 (SelectMenu) Clicado! Carregando Lv4...");
        LoadScene("Dialogo4");
    }


}