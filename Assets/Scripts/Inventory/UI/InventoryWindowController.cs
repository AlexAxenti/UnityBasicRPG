using UnityEngine;

public class InventoryWindowController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private InventoryUI playerPanel;
    [SerializeField] private InventoryUI otherPanel;

    [Header("References")]
    [SerializeField] private PlayerInventory playerInventory;

    [Header("Titles")]
    [SerializeField] private string playerPanelTitle = "Inventory";
    [SerializeField] private string lootPanelTitle = "Loot";

    private LootContainer activeLootContainer;

    public bool IsLootOpen => activeLootContainer != null;
    public LootContainer ActiveLootContainer => activeLootContainer;

    private void Awake()
    {
        if (playerPanel != null && otherPanel != null)
        {
            playerPanel.SetLinkedPanel(otherPanel);
            otherPanel.SetLinkedPanel(playerPanel);
        }
    }

    public void OpenPlayerOnly()
    {
        if (playerPanel == null || playerInventory == null)
        {
            Debug.LogWarning("InventoryWindowController is missing player panel or player inventory.", this);
            return;
        }

        playerPanel.BindPlayerInventory(playerInventory, playerPanelTitle);
        playerPanel.SetMode(InventoryPanelMode.Player);
        playerPanel.Show();

        if (otherPanel != null)
        {
            otherPanel.Hide();
        }
    }

    public void OpenLoot(LootContainer loot)
    {
        if (loot == null)
        {
            Debug.LogWarning("Tried to open loot window, but loot was null.", this);
            return;
        }

        if (playerPanel == null || otherPanel == null || playerInventory == null)
        {
            Debug.LogWarning("InventoryWindowController is missing required references.", this);
            return;
        }

        activeLootContainer = loot;

        playerPanel.BindPlayerInventory(playerInventory, playerPanelTitle);
        playerPanel.SetMode(InventoryPanelMode.PlayerWhileLooting);

        otherPanel.BindLoot(loot, playerInventory, lootPanelTitle);
        otherPanel.SetMode(InventoryPanelMode.LootSource);

        playerPanel.Show();
        otherPanel.Show();
    }

    public void CloseAll()
    {
        activeLootContainer = null;

        if (playerPanel != null)
        {
            playerPanel.Hide();
        }

        if (otherPanel != null)
        {
            otherPanel.Hide();
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}