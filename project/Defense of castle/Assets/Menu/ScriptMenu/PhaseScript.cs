using UnityEngine;
using UnityEngine.SceneManagement; // Adicione isso se for carregar cenas
// using UnityEngine.UI; // Descomente se precisar interagir com componentes UI específicos via script
public class PhaseScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Fase1()
    {
        Debug.Log("Botão Fase 1 Clicado!");
        // TODO: Coloque aqui a lógica para iniciar um novo jogo
        // Exemplo: SceneManager.LoadScene("NomeDaSuaCenaDeJogo");
        //Quando criar um novo jogo o mapa deve ser carregado e resetado
        //Implementar lógica de resetar o mapa
        SceneManager.LoadScene("Lv1");
    }

    public void Fase2()
    {
        Debug.Log("Botão Fase 2 Clicado!");
        // TODO: Coloque aqui a lógica para iniciar um novo jogo
        // Exemplo: SceneManager.LoadScene("NomeDaSuaCenaDeJogo");
        //Quando criar um novo jogo o mapa deve ser carregado e resetado
        //Implementar lógica de resetar o mapa
        SceneManager.LoadScene("Lv2");
    }
    public void Fase3()
    {
        Debug.Log("Botão Fase 3 Clicado!");
        // TODO: Coloque aqui a lógica para iniciar um novo jogo
        // Exemplo: SceneManager.LoadScene("NomeDaSuaCenaDeJogo");
        //Quando criar um novo jogo o mapa deve ser carregado e resetado
        //Implementar lógica de resetar o mapa
        SceneManager.LoadScene("Lv3");
    }
    public void Fase4()
    {
        Debug.Log("Botão Fase 4 Clicado!");
        // TODO: Coloque aqui a lógica para iniciar um novo jogo
        // Exemplo: SceneManager.LoadScene("NomeDaSuaCenaDeJogo");
        //Quando criar um novo jogo o mapa deve ser carregado e resetado
        //Implementar lógica de resetar o mapa
        SceneManager.LoadScene("Lv4");
    }

    public void Fase5()
    {
        Debug.Log("Botão Fase 5 Clicado!");
        // TODO: Coloque aqui a lógica para iniciar um novo jogo
        // Exemplo: SceneManager.LoadScene("NomeDaSuaCenaDeJogo");
        //Quando criar um novo jogo o mapa deve ser carregado e resetado
        //Implementar lógica de resetar o mapa
        SceneManager.LoadScene("Lv5");
    }


}
