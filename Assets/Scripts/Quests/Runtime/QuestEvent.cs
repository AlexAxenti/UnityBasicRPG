public class QuestEvent
{
    public QuestEventType EventType { get; }
    public string TargetId { get; }
    public int Amount { get; }

    public QuestEvent(QuestEventType eventType, string targetId, int amount = 1)
    {
        EventType = eventType;
        TargetId = targetId;
        Amount = amount;
    }
}