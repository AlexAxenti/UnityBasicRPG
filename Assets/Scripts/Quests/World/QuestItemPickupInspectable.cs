using UnityEngine;

public class QuestItemPickupInspectable : Inspectable
{
    [Header("Pickup")]
    [SerializeField] private ItemData item;
    [SerializeField] private int quantity = 1;

    [Header("Quest Event")]
    [SerializeField] private QuestEventType questEventType = QuestEventType.ItemPickedUp;
    [SerializeField] private string questEventTargetId;
    [SerializeField] private int questEventAmount = 1;

    public override string GetDisplayText()
    {
        if (item == null)
            return base.GetDisplayText();

        return $"Loot: {item.itemName}\n[{InteractionKey}]: Pick Up";
    }

    public override void Interact(PlayerInteractor interactor)
    {
        if (item == null)
        {
            Debug.LogWarning("Quest pickup has no item assigned.", this);
            return;
        }

        PlayerInventory playerInventory = interactor.GetComponent<PlayerInventory>();

        if (playerInventory == null)
        {
            Debug.LogWarning("PlayerInteractor has no PlayerInventory on same object.", this);
            return;
        }

        bool added = playerInventory.AddItem(item, quantity);

        if (!added)
        {
            Debug.Log("Could not pick up item. Inventory may be full.");
            return;
        }

        QuestEventBus.Raise(new QuestEvent(
            questEventType,
            questEventTargetId,
            questEventAmount
        ));

        Debug.Log($"Picked up quest item: {item.itemName}");

        gameObject.SetActive(false);
    }
}