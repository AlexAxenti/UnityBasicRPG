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
    private DialogueInspectable currentDialogueSource;

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

    public void StartDialogue(DialogueData dialogueData, DialogueInspectable dialogueSource)
    {
        if (dialogueData == null)
        {
            Debug.LogWarning("Tried to start dialogue, but DialogueData was null.");
            return;
        }

        currentDialogue = dialogueData;
        currentDialogueSource = dialogueSource;
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

        ExecuteChoiceAction(choice);

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
        currentDialogueSource = null;
        currentNode = null;

        if (dialoguePanelUI != null)
        {
            dialoguePanelUI.Hide();
        }

        InventoryWindowController inventoryWindowController = FindAnyObjectByType<InventoryWindowController>();

        if (inventoryWindowController != null)
        {
            // Debug.LogWarning("No InventoryWindowController found in scene.");
            inventoryWindowController.CloseAll();
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

    private void ExecuteChoiceAction(DialogueChoiceData choice)
    {
        switch (choice.actionType)
        {
            case DialogueActionType.None:
                break;

            case DialogueActionType.OpenShop:
                OpenShopFromCurrentSource();
                break;

            case DialogueActionType.StartQuest:
                QuestManager.Instance?.StartQuest(choice.questId);
                break;

            case DialogueActionType.EmitQuestEvent:
                QuestEventBus.Raise(new QuestEvent(
                    choice.questEventType,
                    choice.questEventTargetId,
                    choice.questEventAmount
                ));
                break;

        }
    }

    private void OpenShopFromCurrentSource()
    {
        if (currentDialogueSource == null)
        {
            Debug.LogWarning("Tried to open shop, but current dialogue source was null.");
            return;
        }

        ShopService shopService = currentDialogueSource.GetComponent<ShopService>();

        if (shopService == null)
        {
            Debug.LogWarning($"Dialogue source '{currentDialogueSource.name}' has no ShopService.");
            return;
        }

        InventoryWindowController inventoryWindowController = FindAnyObjectByType<InventoryWindowController>();

        if (inventoryWindowController == null)
        {
            Debug.LogWarning("No InventoryWindowController found in scene.");
            return;
        }

        // EndDialogue();
        inventoryWindowController.OpenShop(shopService);
    }
}