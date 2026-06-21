using UnityEngine;

public class QuestWorldActivator : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private string questId;
    [SerializeField] private QuestStatus requiredStatus = QuestStatus.InProgress;
    [SerializeField] private int requiredStepIndex = -1;

    private void OnEnable()
    {
        Refresh();
        QuestEventBus.OnQuestEvent += HandleQuestEvent;
    }

    private void OnDisable()
    {
        QuestEventBus.OnQuestEvent -= HandleQuestEvent;
    }

    private void Start()
    {
        Refresh();
    }

    private void HandleQuestEvent(QuestEvent questEvent)
    {
        Refresh();
    }

    public void Refresh()
    {
        if (targetObject == null)
        {
            Debug.LogWarning("QuestWorldActivator has no target object assigned.", this);
            return;
        }

        if (QuestManager.Instance == null)
            return;

        QuestStatus status = QuestManager.Instance.GetQuestStatus(questId);
        int stepIndex = QuestManager.Instance.GetQuestStepIndex(questId);

        bool shouldBeActive = status == requiredStatus;

        if (requiredStepIndex >= 0)
        {
            shouldBeActive = shouldBeActive && stepIndex == requiredStepIndex;
        }

        targetObject.SetActive(shouldBeActive);
    }
}