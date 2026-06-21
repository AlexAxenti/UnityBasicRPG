using UnityEngine;

public class QuestEventOnDeath : MonoBehaviour
{
    [SerializeField] private QuestEventType questEventType = QuestEventType.EnemyKilled;
    [SerializeField] private string questEventTargetId = "";
    [SerializeField] private int questEventAmount = 1;

    private CharacterHealth characterHealth;

    private void Awake()
    {
        characterHealth = GetComponent<CharacterHealth>();

        if (characterHealth == null)
        {
            Debug.LogWarning("QuestEventOnDeath requires CharacterHealth on the same object.", this);
        }
    }

    private void OnEnable()
    {
        if (characterHealth != null)
        {
            characterHealth.OnDied += HandleDied;
        }
    }

    private void OnDisable()
    {
        if (characterHealth != null)
        {
            characterHealth.OnDied -= HandleDied;
        }
    }

    private void HandleDied(CharacterHealth health)
    {
        QuestEventBus.Raise(new QuestEvent(
            questEventType,
            questEventTargetId,
            questEventAmount
        ));

        Debug.Log($"Quest death event emitted: {questEventTargetId}");
    }
}