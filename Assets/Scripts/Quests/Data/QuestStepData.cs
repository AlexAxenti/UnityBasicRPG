using System;
using UnityEngine;

[Serializable]
public class QuestStepData
{
    public string stepId;

    [TextArea]
    public string description;

    public QuestObjectiveData objective;
}