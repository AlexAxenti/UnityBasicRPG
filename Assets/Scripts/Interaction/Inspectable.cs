using UnityEngine;
using UnityEngine.InputSystem;

public class Inspectable : MonoBehaviour
{
    [Header("Display")]
    [SerializeField] private string label = "Inspect";

    [Header("Interaction")]
    [SerializeField] private bool hasInteraction = true;
    [SerializeField] private Key interactionKey = Key.F;

    public string Label => label;
    public Key InteractionKey => interactionKey;

    public virtual string GetDisplayText()
    {
        return CanInteract() ? $"[{interactionKey}] {label}" : label;
    }

    public virtual bool CanInteract() => hasInteraction;

    public virtual void SetLabel(string newLabel)
    {
        label = newLabel;
    }

    public virtual void Interact(PlayerInteractor interactor)
    {
        // Default interaction behavior can be defined here.
    }
}