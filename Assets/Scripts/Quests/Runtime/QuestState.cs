public enum QuestStatus
{
    NotStarted,
    InProgress,
    Completed
}

public class QuestState
{
    public QuestData QuestData { get; }
    public QuestStatus Status { get; private set; }
    public int CurrentStepIndex { get; private set; }
    public int CurrentProgress { get; private set; }

    public QuestState(QuestData questData)
    {
        QuestData = questData;
        Status = QuestStatus.NotStarted;
        CurrentStepIndex = 0;
        CurrentProgress = 0;
    }

    public QuestStepData CurrentStep
    {
        get
        {
            if (QuestData == null || QuestData.steps == null)
                return null;

            if (CurrentStepIndex < 0 || CurrentStepIndex >= QuestData.steps.Count)
                return null;

            return QuestData.steps[CurrentStepIndex];
        }
    }

    public void Start()
    {
        Status = QuestStatus.InProgress;
        CurrentStepIndex = 0;
        CurrentProgress = 0;
    }

    public void AddProgress(int amount)
    {
        CurrentProgress += amount;
    }

    public bool IsCurrentStepComplete()
    {
        QuestStepData step = CurrentStep;

        if (step == null || step.objective == null)
            return false;

        return CurrentProgress >= step.objective.requiredAmount;
    }

    public void AdvanceStep()
    {
        CurrentStepIndex++;
        CurrentProgress = 0;

        if (CurrentStepIndex >= QuestData.steps.Count)
        {
            Status = QuestStatus.Completed;
        }
    }
}