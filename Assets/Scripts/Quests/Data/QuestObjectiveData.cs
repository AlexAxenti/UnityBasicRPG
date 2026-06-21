using System;
using UnityEngine;

[Serializable]
public class QuestObjectiveData
{
    public QuestEventType eventType;
    public string targetId;
    public int requiredAmount = 1;

    [TextArea]
    public string description;
}