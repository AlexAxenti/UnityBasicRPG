using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private float inspectRadius = 2f;
    [SerializeField] private float maxInspectDistance = 2.5f;
    [SerializeField] private float forwardDotThreshold = 0.4f;
    [SerializeField] private LayerMask inspectableLayers;

    private Transform detectionOrigin;
    private InteractionPromptUI promptUI;

    private Inspectable currentInspectable;

    public Inspectable CurrentInspectable => currentInspectable;

    private void Awake()
    {
        promptUI = FindAnyObjectByType<InteractionPromptUI>(FindObjectsInactive.Include);
        detectionOrigin = transform;

        if (detectionOrigin == null)
            Debug.LogWarning($"Detection origin not assigned on {gameObject.name}, using player position");

        if (promptUI == null)
            Debug.LogWarning($"InteractionPromptUI not assigned on {gameObject.name}");
    }

    private void Update()
    {
        FindBestInspectable();
        UpdatePrompt();
        HandleInteractionInput();
    }

    private void FindBestInspectable()
    {
        currentInspectable = null;

        Vector3 origin = detectionOrigin != null ? detectionOrigin.position : transform.position;
        Collider[] hits = Physics.OverlapSphere(origin, inspectRadius, inspectableLayers);

        float bestScore = float.MinValue;

        foreach (Collider hit in hits)
        {
            Inspectable inspectable = hit.GetComponentInParent<Inspectable>();
            if (inspectable == null) continue;

            Vector3 toTarget = inspectable.transform.position - origin;
            float distance = toTarget.magnitude;
            if (distance > maxInspectDistance) continue;

            Vector3 directionToTarget = toTarget.normalized;
            float dot = Vector3.Dot(transform.forward, directionToTarget);
            if (dot < forwardDotThreshold) continue;

            float score = dot * 10f - distance;

            if (score > bestScore)
            {
                bestScore = score;
                currentInspectable = inspectable;
            }
        }
    }

    private void UpdatePrompt()
    {
        if (promptUI == null) return;

        if (currentInspectable == null)
        {
            promptUI.Hide();
            return;
        }

        promptUI.Show(currentInspectable.GetDisplayText());
    }

    private void HandleInteractionInput()
    {
        if (currentInspectable == null) return;
        if (!currentInspectable.CanInteract()) return;

        if (Keyboard.current == null) return;

        Key interactionKey = currentInspectable.InteractionKey;
        if (Keyboard.current[interactionKey].wasPressedThisFrame)
        {
            currentInspectable.Interact(this);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Transform originTransform = detectionOrigin != null ? detectionOrigin : transform;
        Vector3 origin = originTransform.position;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(origin, inspectRadius);

        Vector3 forwardPoint = origin + transform.forward * maxInspectDistance;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(origin, forwardPoint);
    }
}