using UnityEngine;
using TMPro;

public class LifeManager : MonoBehaviour
{
    public int lives = 10;
    [SerializeField] private TMP_Text livesText;

    private int currentLivesDisplayed = -1;

    void Start()
    {
        if (livesText == null)
            livesText = GameObject.Find("VidasTexto")?.GetComponent<TMP_Text>();

        UpdateLivesUI();
    }

    public void LoseLife()
    {
        if (lives <= 0) return;
        lives--;
        UpdateLivesUI();

        if (lives <= 0)
        {
            GameOver();
        }
    }

    private void UpdateLivesUI()
    {
        if (livesText == null) return;

        if (currentLivesDisplayed != lives)
        {
            livesText.text = $"Vidas: {lives}";
            currentLivesDisplayed = lives;
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
        // Chame o gerenciador de cena para carregar a cena de derrota aqui
        UnityEngine.SceneManagement.SceneManager.LoadScene("Derrota");
    }
}
