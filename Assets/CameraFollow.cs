using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    [Header("Distance / Height")]
    public float distance = 6f;
    public float height = 2f;

    [Header("Rotation")]
    public float mouseSensitivity = 0.15f;
    public float minPitch = -20f;
    public float maxPitch = 60f;

    private float yaw = 0f;
    private float pitch = 20f;

    void Start()
    {
        if (target != null)
        {
            yaw = target.eulerAngles.y;
        }
    }

    void LateUpdate()
    {
        if (target == null || Mouse.current == null) return;

        if (Mouse.current.rightButton.isPressed)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            yaw += mouseDelta.x * mouseSensitivity;
            pitch -= mouseDelta.y * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 targetPosition = target.position + Vector3.up * height;
        Vector3 cameraOffset = rotation * new Vector3(0f, 0f, -distance);

        transform.position = targetPosition + cameraOffset;
        transform.LookAt(targetPosition);
    }
}