using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    //TODO should this be found on Awake instead
    [Header("References")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private GameObject equippedHighlight;
    [SerializeField] private Image slotBackground;

    [Header("Hover Colors")]
    [SerializeField] private Color normalBackgroundColor = Color.white;
    [SerializeField] private Color hoverBackgroundColor = new Color(1f, 1f, 1f, 0.85f);

    private InventoryUI ownerUI;
    private int slotIndex;

    public void Initialize(InventoryUI ownerUI, int index)
    {
        this.ownerUI = ownerUI;
        slotIndex = index;
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
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            ownerUI?.OnSlotLeftClicked(slotIndex);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            ownerUI?.OnSlotRightClicked(slotIndex);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetHovered(true);
        ownerUI?.OnSlotHovered(slotIndex);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetHovered(false);
        ownerUI?.OnSlotHoverExited(slotIndex);
    }

    private void SetHovered(bool isHovered)
    {
        if (slotBackground != null)
        {
            slotBackground.color = isHovered ? hoverBackgroundColor : normalBackgroundColor;
        }
    }
}