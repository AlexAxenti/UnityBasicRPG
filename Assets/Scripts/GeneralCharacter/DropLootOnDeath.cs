using System.Collections.Generic;
using UnityEngine;

public class DropLootOnDeath : MonoBehaviour
{
    [SerializeField] private LootTable lootTable;
    [SerializeField] private LootPouch lootPouchPrefab;
    [SerializeField] private Vector3 spawnOffset = new Vector3(0f, 0.5f, 0f);

    private CharacterHealth characterHealth;

    private void Awake()
    {
        characterHealth = GetComponent<CharacterHealth>();

        if (lootTable == null)
            lootTable = GetComponent<LootTable>();
    }

    private void OnEnable()
    {
        if (characterHealth != null)
            characterHealth.OnDied += HandleDied;
    }

    private void OnDisable()
    {
        if (characterHealth != null)
            characterHealth.OnDied -= HandleDied;
    }

    private void HandleDied(CharacterHealth health)
    {
        if (lootTable == null || lootPouchPrefab == null) return;

        List<InventorySlot> droppedItems = lootTable.RollLoot();
        if (droppedItems.Count == 0) return;

        LootPouch pouch = Instantiate(
            lootPouchPrefab,
            transform.position + spawnOffset,
            Quaternion.identity
        );

        pouch.Initialize(droppedItems);
    }
}