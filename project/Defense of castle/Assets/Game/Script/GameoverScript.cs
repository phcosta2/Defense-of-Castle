using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOverScript : MonoBehaviour
{
    private GameObject gameOverButton;

    public void ShowGameOverButton()
    {
        if (gameOverButton != null)
            return; // já criado

        // Cria o Canvas, se não tiver nenhum na cena
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasGO = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvas = canvasGO.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }

        // Cria o botão
        gameOverButton = new GameObject("GameOverButton", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
        gameOverButton.transform.SetParent(canvas.transform, false);

        RectTransform rect = gameOverButton.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(200, 80);
        rect.anchoredPosition = Vector2.zero;

        // Fundo azul
        Image img = gameOverButton.GetComponent<Image>();
        img.color = new Color(0f, 0.3f, 1f, 1f); // azul

        // Botão
        Button btn = gameOverButton.GetComponent<Button>();
        btn.onClick.AddListener(OnButtonClicked);

        // Cria texto filho com TextMeshPro
        GameObject textGO = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
        textGO.transform.SetParent(gameOverButton.transform, false);

        RectTransform textRect = textGO.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        TextMeshProUGUI tmp = textGO.GetComponent<TextMeshProUGUI>();
        tmp.text = "Game Over! Voltar ao Menu";
        tmp.fontSize = 24;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
    }

    private void OnButtonClicked()
    {
        SceneManager.LoadScene("SelectMenu");
    }
}
