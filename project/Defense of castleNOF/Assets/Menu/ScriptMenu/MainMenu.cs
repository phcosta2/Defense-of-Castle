using UnityEngine;
using UnityEngine.SceneManagement; // Adicione isso se for carregar cenas
// using UnityEngine.UI; // Descomente se precisar interagir com componentes UI específicos via script

public class MainMenu : MonoBehaviour
{
    // Função para o botão "New Game" (Novo Jogo)
    public void NewGame()
    {
        Debug.Log("Botão Novo Jogo Clicado!");
        // TODO: Coloque aqui a lógica para iniciar um novo jogo
        // Exemplo: SceneManager.LoadScene("NomeDaSuaCenaDeJogo");
        //Quando criar um novo jogo o mapa deve ser carregado e resetado
        //Implementar lógica de resetar o mapa
        SceneManager.LoadScene("SelectMenu");
    }

    // Função para o botão "Load Game" (Carregar Jogo)
    public void LoadGame()
    {
        Debug.Log("Botão Carregar Jogo Clicado!");
        // TODO: Coloque aqui a lógica para carregar um jogo salvo
        // Exemplo: Mostrar um menu de saves ou carregar a última cena salva
    }

    // Função para o botão "Settings" (Configurações)
    public void Settings() // Ou talvez OpenSettings() se preferir mais clareza
    {
        Debug.Log("Botão Configurações Clicado!");
        // TODO: Coloque aqui a lógica para abrir o painel/menu de configurações
        // Exemplo: Ativar/Desativar um painel de UI de configurações
    }

    // Função para o botão "Credits" (Créditos)
    public void Credits() // Ou talvez ShowCredits()
    {
        Debug.Log("Botão Créditos Clicado!");
        // TODO: Coloque aqui a lógica para mostrar os créditos
        // Exemplo: Carregar uma cena de créditos ou ativar um painel de UI
    }

    // Função para o botão "Quit" (Sair)
    public void Quit() // Ou talvez QuitGame()
    {
        Debug.Log("Botão Sair Clicado!");
        // TODO: Coloque aqui a lógica para fechar o jogo

        // Lógica padrão para sair:
        #if UNITY_EDITOR
            // Se estiver rodando no Editor Unity
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // Se for uma build compilada do jogo
            Application.Quit();
        #endif
    }

    // Você pode remover Start() e Update() se não precisar deles agora.
    // void Start()
    // {
    //
    // }

    // void Update()
    // {
    //
    // }
}