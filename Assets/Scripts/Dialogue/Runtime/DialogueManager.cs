using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private DialoguePanelUI dialoguePanelUI;

    [Header("State")]
    [SerializeField] private bool isDialogueOpen;

    private DialogueData currentDialogue;
    private DialogueNodeData currentNode;

    public bool IsDialogueOpen => isDialogueOpen;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (dialoguePanelUI != null)
        {
            dialoguePanelUI.Hide();
        }
    }

    public void StartDialogue(DialogueData dialogueData)
    {
        if (dialogueData == null)
        {
            Debug.LogWarning("Tried to start dialogue, but DialogueData was null.");
            return;
        }

        currentDialogue = dialogueData;
        currentNode = currentDialogue.GetNodeById(currentDialogue.startingNodeId);

        if (currentNode == null)
        {
            Debug.LogWarning($"Could not find starting node '{currentDialogue.startingNodeId}' in dialogue '{currentDialogue.name}'.");
            return;
        }

        isDialogueOpen = true;
        SetGameplayLocked(true);
        ShowCurrentNode();
    }

    public void SelectChoice(DialogueChoiceData choice)
    {
        if (choice == null)
        {
            Debug.LogWarning("Selected choice was null.");
            return;
        }

        if (choice.endsDialogue)
        {
            EndDialogue();
            return;
        }

        if (string.IsNullOrWhiteSpace(choice.nextNodeId))
        {
            Debug.LogWarning("Choice had no next node and did not end dialogue. Ending dialogue as fallback.");
            EndDialogue();
            return;
        }

        DialogueNodeData nextNode = currentDialogue.GetNodeById(choice.nextNodeId);

        if (nextNode == null)
        {
            Debug.LogWarning($"Could not find next node '{choice.nextNodeId}' in dialogue '{currentDialogue.name}'.");
            EndDialogue();
            return;
        }

        currentNode = nextNode;
        ShowCurrentNode();
    }

    public void EndDialogue()
    {
        isDialogueOpen = false;
        currentDialogue = null;
        currentNode = null;

        if (dialoguePanelUI != null)
        {
            dialoguePanelUI.Hide();
        }

        SetGameplayLocked(false);
    }

    private void ShowCurrentNode()
    {
        if (dialoguePanelUI == null)
        {
            Debug.LogWarning("DialoguePanelUI reference is missing on DialogueManager.");
            return;
        }

        dialoguePanelUI.ShowNode(currentNode, this);
    }

    //TODO create a seperate manager for this instead of directly controlling player and camera from dialogue manager
    private void SetGameplayLocked(bool locked)
    {
        PlayerController playerController = FindAnyObjectByType<PlayerController>();
        if (playerController != null)
        {
            playerController.SetInputBlocked(locked);
        }

        CameraFollow cameraFollow = FindAnyObjectByType<CameraFollow>();
        if (cameraFollow != null)
        {
            cameraFollow.SetInputBlocked(locked);
        }

        Cursor.lockState = locked ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = locked;
    }
}