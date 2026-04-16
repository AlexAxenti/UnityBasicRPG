using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private GameObject equippedHighlight;
    [SerializeField] private Image slotBackground;

    public void SetEmpty()
    {
        itemIcon.enabled = false;
        quantityText.text = "";
        equippedHighlight.SetActive(false);
    }

    public void SetItem(Sprite icon, int quantity, bool isEquipped)
    {
        itemIcon.enabled = true;
        itemIcon.sprite = icon;

        quantityText.text = quantity > 1 ? quantity.ToString() : "";
        equippedHighlight.SetActive(isEquipped);
    }
}