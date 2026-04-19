using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Transform cameraTransform;
    private CameraFollow cameraFollow;

    private CharacterMotor motor;
    private CharacterCombat combat;

    private void Awake()
    {
        motor = GetComponent<CharacterMotor>();
        combat = GetComponent<CharacterCombat>();

        if (motor == null)
            Debug.LogError($"CharacterMotor missing on {gameObject.name}");

        if (combat == null)
            Debug.LogError($"CharacterCombat missing on {gameObject.name}");

        if (cameraTransform == null)
        {
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                cameraTransform = mainCam.transform;
                cameraFollow = mainCam.GetComponent<CameraFollow>();
            }
        }
    }

    private void Update()
    {
        Vector2 moveInput = ReadMoveInput();
        Vector3 moveDirection = GetCameraRelativeMoveDirection(moveInput);

        RotatePlayerToCameraYaw();

        motor.SetMoveDirection(moveDirection);
        motor.Move();

        HandleCombatInput();
        HandleDashInput(moveDirection);
    }

    private Vector2 ReadMoveInput()
    {
        Vector2 input = Vector2.zero;

        if (Keyboard.current == null)
            return input;

        if (Keyboard.current.aKey.isPressed) input.x -= 1f;
        if (Keyboard.current.dKey.isPressed) input.x += 1f;
        if (Keyboard.current.sKey.isPressed) input.y -= 1f;
        if (Keyboard.current.wKey.isPressed) input.y += 1f;

        if (input.sqrMagnitude > 1f)
            input.Normalize();

        return input;
    }

    private Vector3 GetCameraRelativeMoveDirection(Vector2 input)
    {
        if (cameraTransform == null)
            return new Vector3(input.x, 0f, input.y);

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 move = cameraForward * input.y + cameraRight * input.x;

        if (move.sqrMagnitude > 1f)
            move.Normalize();

        return move;
    }

    private void RotatePlayerToCameraYaw()
    {
        if (motor == null)
            return;

        if (cameraFollow != null)
        {
            Vector3 forward = Quaternion.Euler(0f, cameraFollow.Yaw, 0f) * Vector3.forward;
            motor.FaceDirection(forward);
        }
        else if (cameraTransform != null)
        {
            Vector3 forward = cameraTransform.forward;
            forward.y = 0f;
            motor.FaceDirection(forward);
        }
    }

    private void HandleCombatInput()
    {
        Mouse mouse = Mouse.current;

        if (mouse != null && mouse.leftButton.wasPressedThisFrame)
        {
            combat.TryAttack();
        }
    }

    private void HandleDashInput(Vector3 moveDirection)
    {
        Keyboard keyboard = Keyboard.current;

        if (keyboard != null && keyboard.spaceKey.wasPressedThisFrame)
        {
            Vector3 dashDirection = moveDirection.sqrMagnitude > 0.001f ? moveDirection : transform.forward;
            motor.TryDash(dashDirection);
        }
    }
}