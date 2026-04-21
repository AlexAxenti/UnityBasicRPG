using System;
using UnityEngine;

[Serializable]
public class DialogueChoiceData
{
    [TextArea]
    public string choiceText;

    [Tooltip("ID of the node this choice leads to. Leave empty if this choice ends the dialogue.")]
    public string nextNodeId;

    [Tooltip("If true, dialogue ends after selecting this choice.")]
    public bool endsDialogue;
}