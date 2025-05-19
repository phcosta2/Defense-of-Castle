// SceneTransitionManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // Considere se DontDestroyOnLoad é necessário para sua estrutura de jogo.
            // Se este manager for usado apenas para a transição de introdução,
            // provavelmente não precisa persistir entre as cenas.
            // DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    /// <summary>
    /// Transiciona imediatamente para a cena especificada.
    /// </summary>
    /// <param name="sceneName">O nome da cena para carregar.</param>
    public void TransitionToScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("SceneTransitionManager: O nome da cena não pode estar vazio para a transição!");
            return;
        }

        Debug.Log($"SceneTransitionManager: Carregando cena '{sceneName}' imediatamente.");
        SceneManager.LoadScene(sceneName);
    }
}