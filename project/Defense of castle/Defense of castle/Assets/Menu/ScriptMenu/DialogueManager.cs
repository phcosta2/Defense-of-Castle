// DialogueManager.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic; // Required for Queue

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [Header("UI Elements")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;
    public Button continueButton;

    [Header("Dialogue Settings")]
    public float typingSpeed = 0.05f;

    private Queue<DialogueLine> sentencesQueue;
    private System.Action onDialogueCompleteCallback;

    private Coroutine typingCoroutine;
    private bool isCurrentlyTyping = false;
    private DialogueLine currentLine; // This is of type DialogueLine defined in DialogueData.cs

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        sentencesQueue = new Queue<DialogueLine>();
    }

    void Start()
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(DisplayNextSentence);
        }
    }

    // Parameter 'dialogue' is of type Dialogue defined in DialogueData.cs
    public void StartDialogue(Dialogue dialogue, System.Action onComplete = null)
    {
        if (dialoguePanel == null || speakerNameText == null || dialogueText == null)
        {
            Debug.LogError("DialogueManager: UI elements not assigned!");
            onComplete?.Invoke();
            return;
        }

        onDialogueCompleteCallback = onComplete;
        dialoguePanel.SetActive(true);

        sentencesQueue.Clear();

        // 'line' is of type DialogueLine
        foreach (DialogueLine line in dialogue.lines)
        {
            sentencesQueue.Enqueue(line);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (isCurrentlyTyping)
        {
            CompleteSentence();
            return;
        }

        if (sentencesQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        currentLine = sentencesQueue.Dequeue(); // currentLine is DialogueLine

        if (speakerNameText != null) speakerNameText.text = currentLine.speakerName;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeSentence(currentLine.sentence));
    }

    void CompleteSentence()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        if (dialogueText != null && currentLine != null)
        {
            dialogueText.text = currentLine.sentence;
        }
        isCurrentlyTyping = false;
    }

    IEnumerator TypeSentence(string sentence)
    {
        isCurrentlyTyping = true;
        if (dialogueText != null) dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            if (dialogueText != null) dialogueText.text += letter;
            // For faster testing, can reduce or remove wait
            if (typingSpeed > 0) yield return new WaitForSeconds(typingSpeed);
            else yield return null; // yield one frame if typingSpeed is 0 for instant text
        }
        isCurrentlyTyping = false;
        typingCoroutine = null;
    }

    void EndDialogue()
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        Debug.Log("Dialogue finished.");
        onDialogueCompleteCallback?.Invoke();
        onDialogueCompleteCallback = null;
    }
}