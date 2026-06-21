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

    [Header("Optional Action")]
    public DialogueActionType actionType = DialogueActionType.None;

    [Header("Quest Action")]
    public string questId;
    public QuestEventType questEventType;
    public string questEventTargetId;
    public int questEventAmount = 1;

    [Header("Quest Condition")]
    public QuestConditionType questConditionType = QuestConditionType.Always;
    public string conditionQuestId;
    public int requiredStepIndex = -1;

    public bool CanShow()
    {
        if (questConditionType == QuestConditionType.Always)
            return true;

        if (QuestManager.Instance == null)
            return false;

        QuestStatus status = QuestManager.Instance.GetQuestStatus(conditionQuestId);
        int stepIndex = QuestManager.Instance.GetQuestStepIndex(conditionQuestId);

        switch (questConditionType)
        {
            case QuestConditionType.CanStartQuest:
                return QuestManager.Instance.CanStartQuest(conditionQuestId);

            case QuestConditionType.QuestInProgressAtStep:
                return status == QuestStatus.InProgress && stepIndex == requiredStepIndex;

            case QuestConditionType.QuestCompleted:
                return status == QuestStatus.Completed;

            case QuestConditionType.QuestNotCompleted:
                return status != QuestStatus.Completed;

            default:
                return true;
        }
    }
}