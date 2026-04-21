using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueChoiceButtonUI : MonoBehaviour
{
    [SerializeField] private TMP_Text choiceText;
    [SerializeField] private Button button;

    private DialogueChoiceData choiceData;
    private DialogueManager dialogueManager;

    public void Setup(DialogueChoiceData choice, DialogueManager manager)
    {
        choiceData = choice;
        dialogueManager = manager;

        if (choiceText != null)
        {
            choiceText.text = choice.choiceText;
        }

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClicked);
        }
    }

    private void OnClicked()
    {
        if (dialogueManager == null)
        {
            Debug.LogWarning("DialogueManager missing on DialogueChoiceButtonUI.");
            return;
        }

        dialogueManager.SelectChoice(choiceData);
    }
}