using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ItemContextMenuUI : MonoBehaviour
{
    [SerializeField] private RectTransform rootTransform;
    [SerializeField] private Button primaryActionButton;
    [SerializeField] private TMP_Text primaryActionText;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Vector2 cursorOffset = new(12f, -12f);

    private System.Action onPrimaryAction;

    private Canvas parentCanvas;

    private void Awake()
    {
        parentCanvas = GetComponentInParent<Canvas>();
        Hide();

        primaryActionButton.onClick.AddListener(HandlePrimaryActionClicked);
        cancelButton.onClick.AddListener(Hide);
    }

    public void Show(Vector2 screenPosition, string primaryLabel, System.Action primaryAction)
    {
        onPrimaryAction = primaryAction;
        primaryActionText.text = primaryLabel;
        gameObject.SetActive(true);

        RectTransform canvasRect = parentCanvas.transform as RectTransform;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPosition,
            parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : parentCanvas.worldCamera,
            out Vector2 localPoint))
        {
            rootTransform.localPosition = localPoint + cursorOffset;
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        onPrimaryAction = null;
    }

    private void HandlePrimaryActionClicked()
    {
        onPrimaryAction?.Invoke();
        Hide();
    }
}