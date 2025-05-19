// IntroDialogueTrigger.cs
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; // Para o SceneManager.LoadScene de fallback

public class IntroDialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Content (Assign in Inspector)")]
    public Dialogue introDialogue;

    [Header("Scene Transition (Assign in Inspector)")]
    public string nextSceneName;

    private bool dialogueTriggered = false;

    void Start()
    {
        StartCoroutine(DelayedStartRoutine());
    }

    IEnumerator DelayedStartRoutine()
    {
        yield return null; // Espera um frame para garantir que outros Awakes foram chamados

        // Verifica se as instâncias dos managers estão disponíveis
        bool dialogueManagerReady = DialogueManager.instance != null;
        bool sceneTransitionManagerReady = SceneTransitionManager.instance != null;

        if (dialogueManagerReady)
        {
            if (!sceneTransitionManagerReady)
            {
                Debug.LogWarning("IntroDialogueTrigger: SceneTransitionManager não encontrado. A transição de cena usará o método direto como fallback.");
            }
            TriggerDialogue();
        }
        else
        {
            Debug.LogError("IntroDialogueTrigger: DialogueManager instance not found after delay! Cannot start intro dialogue.");
        }
    }

    public void TriggerDialogue()
    {
        if (dialogueTriggered)
        {
            return;
        }

        if (DialogueManager.instance == null) // Dupla checagem, caso DelayedStartRoutine tenha problemas
        {
            Debug.LogError("IntroDialogueTrigger: DialogueManager instance is null at TriggerDialogue. Cannot trigger dialogue.");
            return;
        }
        if (introDialogue == null || introDialogue.lines == null || introDialogue.lines.Count == 0)
        {
            Debug.LogWarning("IntroDialogueTrigger: Sem linhas de diálogo atribuídas. Pulando para a transição de cena, se configurada.");
            OnDialogueFinished(); // Chama o final imediatamente se não houver diálogo
            return;
        }

        dialogueTriggered = true;
        Debug.Log("IntroDialogueTrigger: Iniciando diálogo '" + (string.IsNullOrEmpty(introDialogue.dialogueName) ? "Sem Título" : introDialogue.dialogueName) + "'.");
        DialogueManager.instance.StartDialogue(introDialogue, OnDialogueFinished);
    }

    void OnDialogueFinished()
    {
        Debug.Log("IntroDialogueTrigger: Callback de diálogo finalizado recebido.");

        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogWarning("IntroDialogueTrigger: Nome da Próxima Cena não definido, nenhuma transição de cena ocorrerá.");
            return;
        }

        Debug.Log("IntroDialogueTrigger: Transicionando para a cena: " + nextSceneName);
        if (SceneTransitionManager.instance != null)
        {
            SceneTransitionManager.instance.TransitionToScene(nextSceneName);
        }
        else
        {
            Debug.LogError("IntroDialogueTrigger: SceneTransitionManager instance is null. Tentando carregamento direto da cena como fallback.");
            SceneManager.LoadScene(nextSceneName); // Fallback
        }
    }
}