using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialoguePanelUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject rootPanel;
    [SerializeField] private TMP_Text speakerNameText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Transform choicesContainer;
    [SerializeField] private DialogueChoiceButtonUI choiceButtonPrefab;

    private readonly List<DialogueChoiceButtonUI> spawnedButtons = new();

    public void ShowNode(DialogueNodeData node, DialogueManager dialogueManager)
    {
        if (node == null)
        {
            Debug.LogWarning("Tried to show a null dialogue node.");
            return;
        }

        if (rootPanel != null)
        {
            rootPanel.SetActive(true);
        }

        if (speakerNameText != null)
        {
            speakerNameText.text = node.speakerName;
        }

        if (dialogueText != null)
        {
            dialogueText.text = node.dialogueText;
        }

        ClearChoices();

        for (int i = 0; i < node.choices.Count; i++)
        {
            DialogueChoiceButtonUI button = Instantiate(choiceButtonPrefab, choicesContainer);
            button.Setup(node.choices[i], dialogueManager);
            spawnedButtons.Add(button);
        }
    }

    public void Hide()
    {
        ClearChoices();

        if (rootPanel != null)
        {
            rootPanel.SetActive(false);
        }
    }

    private void ClearChoices()
    {
        for (int i = 0; i < spawnedButtons.Count; i++)
        {
            if (spawnedButtons[i] != null)
            {
                Destroy(spawnedButtons[i].gameObject);
            }
        }

        spawnedButtons.Clear();
    }
}