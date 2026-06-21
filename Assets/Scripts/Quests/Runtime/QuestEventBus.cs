using System;

public static class QuestEventBus
{
    public static event Action<QuestEvent> OnQuestEvent;

    public static void Raise(QuestEvent questEvent)
    {
        OnQuestEvent?.Invoke(questEvent);
    }
}