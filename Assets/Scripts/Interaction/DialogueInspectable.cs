using UnityEngine;

public class DialogueInspectable : Inspectable
{
    [SerializeField] private CharacterInfo characterInfo;
    [Header("Dialogue")]
    [SerializeField] private DialogueData dialogueData;

    private void Awake()
    {
        if (characterInfo == null)
        {
            characterInfo = GetComponent<CharacterInfo>();
            if (characterInfo == null)
            {
                Debug.LogWarning("No CharacterInfo found on DialogueInspectable.");
            }
        }
    }

  public override string GetDisplayText()
    {
        if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueOpen)
        {
            return "";
        }

        return CanInteract() ? $"{characterInfo.CharacterName}\n[{InteractionKey}]: Talk" : characterInfo.CharacterName;
    }

    public override void Interact(PlayerInteractor interactor)
    {
        if (DialogueManager.Instance == null)
        {
            Debug.LogWarning("No DialogueManager found in scene.");
            return;
        } else if (DialogueManager.Instance.IsDialogueOpen)
        {
            Debug.LogWarning("Tried to start dialogue, but a dialogue is already open.");
            return;
        }

        DialogueManager.Instance.StartDialogue(dialogueData);
    }
}