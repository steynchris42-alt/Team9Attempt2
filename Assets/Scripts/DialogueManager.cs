
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [Header("NPC Settings (Single Speaker)")]
    public string npcName = "Scientist";
    public Sprite npcPortrait;

    [Header("Linked Components")]
    public TextMeshProUGUI nameBox;
    public TextMeshProUGUI textBox;
    public Image speakerImage;
    public GameObject dialogueGameObject;

    [Header("Text Configuration")]
    public float typingSpeed = 0.05f;

    [Header("Dialogue Status")]
    public bool isTyping = false;
    public bool dialogueFinished = true;

    [Header("Dialogue Content")]
    public DialogueLine[] dialogueLines;

    #region PRIVATE VARIABLES
    private int currentIndex = 0;
    private Coroutine typingCoroutine;
    private bool justStarted = false;
    #endregion

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Triggers the dialogue automatically when the scene starts
        if (dialogueLines != null && dialogueLines.Length > 0)
        {
            StartDialogue(dialogueLines);
        }
    }

    private void Update()
    {
        if (dialogueFinished) return;

        if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire1"))
        {
            if (justStarted)
            {
                justStarted = false;
                return;
            }

            if (isTyping)
            {
                if (typingCoroutine != null) StopCoroutine(typingCoroutine);
                ShowFullLine(dialogueLines[currentIndex]);
                isTyping = false;
            }
            else
            {
                currentIndex++;

                if (currentIndex < dialogueLines.Length)
                {
                    if (typingCoroutine != null) StopCoroutine(typingCoroutine);
                    typingCoroutine = StartCoroutine(TypeLine(dialogueLines[currentIndex]));
                }
                else
                {
                    EndDialogue();
                }
            }
        }
    }

    public void StartDialogue(DialogueLine[] newLines)
    {
        if (dialogueGameObject != null) dialogueGameObject.SetActive(true);
        dialogueFinished = false;
        dialogueLines = newLines;
        currentIndex = 0;
        justStarted = true;

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeLine(dialogueLines[currentIndex]));
    }

    IEnumerator TypeLine(DialogueLine line)
    {
        isTyping = true;

        textBox.text = "";

        // Set the NPC Name and Portrait once
        if (nameBox != null) nameBox.text = npcName;
        if (speakerImage != null) speakerImage.sprite = npcPortrait;

        foreach (char c in line.dialogueText.ToCharArray())
        {
            textBox.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private void ShowFullLine(DialogueLine line)
    {
        textBox.text = line.dialogueText;
        if (nameBox != null) nameBox.text = npcName;
        if (speakerImage != null) speakerImage.sprite = npcPortrait;
    }

    public void EndDialogue()
    {
        // Reset text and UI fields
        if (textBox != null) textBox.text = "";
        if (nameBox != null) nameBox.text = "";
        if (speakerImage != null) speakerImage.sprite = null;

        // Update status flags
        dialogueFinished = true;
        isTyping = false;

        // Hide dialogue UI box
        if (dialogueGameObject != null) dialogueGameObject.SetActive(false);

        // TODO: Enable player movement / start game here!
    }
}