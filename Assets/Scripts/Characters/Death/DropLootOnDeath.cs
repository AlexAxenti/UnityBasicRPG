using System.Collections.Generic;
using UnityEngine;

public class DropLootOnDeath : MonoBehaviour
{
    [SerializeField] private LootTable lootTable;
    [SerializeField] private LootContainer lootContainerPrefab;
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
        if (lootTable == null || lootContainerPrefab == null) return;

        List<InventorySlot> droppedItems = lootTable.RollLoot();
        if (droppedItems.Count == 0) return;

        LootContainer pouch = Instantiate(
            lootContainerPrefab,
            transform.position + spawnOffset,
            Quaternion.identity
        );

        pouch.Initialize(droppedItems);

        LootInspectable inspectable = pouch.GetComponent<LootInspectable>();
        CharacterInfo characterInfo = GetComponent<CharacterInfo>();

        if (inspectable != null && characterInfo != null)
        {
            inspectable.SetLabel($"Loot {characterInfo.CharacterName}");
        }
    }
}