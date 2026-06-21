using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    [SerializeField] private List<QuestData> availableQuests = new();

    private readonly Dictionary<string, QuestState> questStates = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        foreach (QuestData quest in availableQuests)
        {
            if (quest == null || string.IsNullOrWhiteSpace(quest.questId))
                continue;

            questStates[quest.questId] = new QuestState(quest);
        }
    }

    private void OnEnable()
    {
        QuestEventBus.OnQuestEvent += HandleQuestEvent;
    }

    private void OnDisable()
    {
        QuestEventBus.OnQuestEvent -= HandleQuestEvent;
    }

    public bool CanStartQuest(string questId)
    {
        if (!questStates.TryGetValue(questId, out QuestState state))
            return false;

        if (state.Status == QuestStatus.NotStarted)
            return true;

        return state.Status == QuestStatus.Completed && state.QuestData.isRepeatable;
    }

    public void StartQuest(string questId)
    {
        if (!questStates.TryGetValue(questId, out QuestState state))
        {
            Debug.LogWarning($"Quest not found: {questId}");
            return;
        }

        if (!CanStartQuest(questId))
        {
            Debug.Log($"Quest cannot be started: {questId}");
            return;
        }

        state.Start();

        Debug.Log($"Quest started: {state.QuestData.questTitle}");
        LogCurrentStep(state);

        QuestEventBus.Raise(new QuestEvent(
            QuestEventType.QuestStateChanged,
            questId
        ));
    }

    public QuestStatus GetQuestStatus(string questId)
    {
        return questStates.TryGetValue(questId, out QuestState state)
            ? state.Status
            : QuestStatus.NotStarted;
    }

    public int GetQuestStepIndex(string questId)
    {
        return questStates.TryGetValue(questId, out QuestState state)
            ? state.CurrentStepIndex
            : -1;
    }

    private void HandleQuestEvent(QuestEvent questEvent)
    {
        foreach (QuestState state in questStates.Values)
        {
            if (state.Status != QuestStatus.InProgress)
                continue;

            QuestStepData step = state.CurrentStep;

            if (step == null || step.objective == null)
                continue;

            QuestObjectiveData objective = step.objective;

            if (objective.eventType != questEvent.EventType)
                continue;

            if (objective.targetId != questEvent.TargetId)
                continue;

            state.AddProgress(questEvent.Amount);

            Debug.Log($"Quest progress: {state.QuestData.questTitle} - {step.description} ({state.CurrentProgress}/{objective.requiredAmount})");

            if (state.IsCurrentStepComplete())
            {
                state.AdvanceStep();

                if (state.Status == QuestStatus.Completed)
                {
                    CompleteQuest(state);
                }
                else
                {
                    LogCurrentStep(state);
                }
            }
        }
    }

    private void CompleteQuest(QuestState state)
    {
        Debug.Log($"Quest completed: {state.QuestData.questTitle}");
        Debug.Log($"Reward: {state.QuestData.goldReward} gold, {state.QuestData.experienceReward} XP");

        PlayerInventory playerInventory = FindAnyObjectByType<PlayerInventory>();
        if (playerInventory != null)
        {
            playerInventory.AddGold(state.QuestData.goldReward);
        }

        PlayerProgression playerProgression = FindAnyObjectByType<PlayerProgression>();
        if (playerProgression != null)
        {
            playerProgression.GainExperience(state.QuestData.experienceReward);
        }
    }

    private void LogCurrentStep(QuestState state)
    {
        QuestStepData step = state.CurrentStep;

        if (step == null)
            return;

        Debug.Log($"New quest step: {step.description}");
    }
}