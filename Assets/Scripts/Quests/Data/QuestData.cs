using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestData", menuName = "RPG/Quests/Quest")]
public class QuestData : ScriptableObject
{
    public string questId;
    public string questTitle;

    [TextArea]
    public string description;

    public bool isRepeatable = false;

    public int goldReward;
    public int experienceReward;

    public List<QuestStepData> steps = new();
}