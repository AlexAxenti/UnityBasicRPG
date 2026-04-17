using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler
{
    //TODO should this be found on Awake instead
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private GameObject equippedHighlight;
    [SerializeField] private Image slotBackground;

    private InventoryUI ownerUI;
    private int slotIndex;

    public void Initialize(InventoryUI ownerUI, int index)
    {
        this.ownerUI = ownerUI;
        slotIndex = index;
        Debug.Log($"Initialized slot {slotIndex}");
    }

    public void SetEmpty()
    {
        itemIcon.enabled = false;
        itemIcon.sprite = null;
        quantityText.text = "";
        equippedHighlight.SetActive(false);
    }

    public void SetItem(Sprite icon, int quantity, bool isEquipped)
    {
        if (icon != null)
        {
            itemIcon.enabled = true;
            itemIcon.sprite = icon;
            itemIcon.color = Color.white;
        }
        else
        {
            itemIcon.enabled = false;
            itemIcon.sprite = null;
        }

        // quantityText.text = quantity > 1 ? quantity.ToString() : "";
        quantityText.text = quantity.ToString();
        equippedHighlight.SetActive(isEquipped);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Clicked slot UI {slotIndex}");

        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        ownerUI?.OnSlotClicked(slotIndex);
    }
}