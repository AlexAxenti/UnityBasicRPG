using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData", menuName = "RPG/Dialogue/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    [Tooltip("The first node to open when this dialogue starts.")]
    public string startingNodeId = "Start";

    public List<DialogueNodeData> nodes = new();

    public DialogueNodeData GetNodeById(string nodeId)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].nodeId == nodeId)
            {
                return nodes[i];
            }
        }

        return null;
    }
}