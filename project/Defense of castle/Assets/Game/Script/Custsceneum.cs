using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Custsceneum : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    private string[] lines = {
        "Herói: Ufa! Consegui proteger o vilarejo daqueles monstros terríveis.",
        "Herói: Mas... que barulho é esse? Parece vir do reino.",
        "Herói: Estou vendo chamas ao longe... Preciso investigar o que está acontecendo lá.",
        "Aldeão: Obrigado por me salvar, jovem! Eu estava com tanto medo...",
        "Aldeão: Por favor, aceite isso como lembrança — meu distintivo de quando eu era mais novo.",
        "Herói: Uma condecoração de cavaleiro? Fico honrado, senhor!",
        "Herói: Mas agora não há tempo a perder. O que me espera lá no reino, só o destino dirá."
    };

    private int currentLine = 0;

    void Start()
    {
        DisplayLine();
    }

public void DisplayLine()
{
    if (currentLine < lines.Length)
    {
        string line = lines[currentLine];
        int colonIndex = line.IndexOf(':');
        if (colonIndex != -1)
        {
            nameText.text = line.Substring(0, colonIndex);
            dialogueText.text = line.Substring(colonIndex + 1).Trim();
        }
        else
        {
            nameText.text = "";
            dialogueText.text = line;
        }
        currentLine++;
    }
    else
    {
        // Fim do diálogo, carrega próxima cena
        SceneManager.LoadScene("MainMenu");
    }
}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DisplayLine();
        }
    }
}
