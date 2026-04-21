using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueNodeData
{
    [Tooltip("Unique ID for this node, such as Start, WhoAreYou, Goodbye.")]
    public string nodeId;

    public string speakerName;

    [TextArea(3, 8)]
    public string dialogueText;

    public List<DialogueChoiceData> choices = new();
}