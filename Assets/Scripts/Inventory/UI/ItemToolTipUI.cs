using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemTooltipUI : MonoBehaviour
{
    [SerializeField] private RectTransform rootTransform;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text statsText;

    [SerializeField] private Vector2 cursorOffset = new(16f, -16f);

    private Canvas parentCanvas;

    private void Awake()
    {
        parentCanvas = GetComponentInParent<Canvas>();
        Hide();
    }

    private void Update()
    {
        if (!gameObject.activeSelf)
            return;

        FollowMouse();
    }

    public void Show(ItemData item)
    {
        if (item == null)
        {
            Hide();
            return;
        }

        gameObject.SetActive(true);

        nameText.text = item.itemName;
        descriptionText.text = item.description;

        if (item is WeaponItemData weapon)
        {
            statsText.text =
                $"Damage: {weapon.DamageBonus}\n" +
                $"Range: {weapon.AttackRange}";
        }
        else if (item is ArmorItemData armor)
        {
            statsText.text =
                $"Armor: {armor.ArmorBonus}";
        }
        else
        {
            statsText.text = "";
        }

        FollowMouse();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void FollowMouse()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        RectTransform canvasRect = parentCanvas.transform as RectTransform;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            mousePosition,
            parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : parentCanvas.worldCamera,
            out Vector2 localPoint))
        {
            rootTransform.localPosition = localPoint + cursorOffset;
        }
    }
}