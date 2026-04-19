using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMotor : MonoBehaviour
{
    [Header("Gravity")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float groundedStickForce = -2f;

    [Header("Directional Movement Multipliers")]
    [SerializeField] private float forwardMultiplier = 1f;
    [SerializeField] private float strafeMultiplier = 0.85f;
    [SerializeField] private float backwardMultiplier = 0.7f;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 720f;

    [Header("Dash")]
    [SerializeField] private float dashDistance = 3f;
    [SerializeField] private float dashDuration = 0.12f;
    [SerializeField] private float dashCooldown = 1.25f;

    private CharacterController controller;
    private CharacterStats characterStats;

    private float verticalVelocity;
    private Vector3 currentMoveDirection;

    private bool isDashing;
    private float dashTimer;
    private float dashCooldownTimer;
    private Vector3 dashVelocity;

    public bool IsDashing => isDashing;
    public bool CanDash => !isDashing && dashCooldownTimer <= 0f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        characterStats = GetComponent<CharacterStats>();

        if (characterStats == null)
            Debug.LogError($"CharacterStats missing on {gameObject.name}");
    }

    private void Update()
    {
        UpdateDash();
        UpdateGravity();
    }

    public void SetMoveDirection(Vector3 worldDirection)
    {
        worldDirection.y = 0f;

        if (worldDirection.sqrMagnitude > 1f)
            worldDirection.Normalize();

        currentMoveDirection = worldDirection;
    }

    public void Move()
    {
        if (isDashing)
        {
            controller.Move(dashVelocity * Time.deltaTime);
            return;
        }

        float speedMultiplier = GetDirectionalSpeedMultiplier(currentMoveDirection);
        Vector3 horizontalVelocity = currentMoveDirection * characterStats.MovementSpeed * speedMultiplier;

        Vector3 finalVelocity = horizontalVelocity;
        finalVelocity.y = verticalVelocity;

        controller.Move(finalVelocity * Time.deltaTime);
    }

    public void FaceDirection(Vector3 direction)
    {
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    public void FaceTarget(Vector3 worldPosition)
    {
        Vector3 direction = worldPosition - transform.position;
        direction.y = 0f;
        FaceDirection(direction);
    }

    public bool TryDash(Vector3 direction)
    {
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            direction = transform.forward;

        direction.Normalize();

        if (!CanDash)
            return false;

        isDashing = true;
        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;

        float dashSpeed = dashDistance / dashDuration;
        dashVelocity = direction * dashSpeed;

        return true;
    }

    private void UpdateDash()
    {
        if (dashCooldownTimer > 0f)
            dashCooldownTimer -= Time.deltaTime;

        if (!isDashing)
            return;

        dashTimer -= Time.deltaTime;

        if (dashTimer <= 0f)
        {
            isDashing = false;
            dashVelocity = Vector3.zero;
        }
    }

    private void UpdateGravity()
    {
        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = groundedStickForce;
        }

        if (!isDashing)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
    }

    private float GetDirectionalSpeedMultiplier(Vector3 moveDirection)
    {
        if (moveDirection.sqrMagnitude < 0.001f)
            return 0f;

        Vector3 local = transform.InverseTransformDirection(moveDirection.normalized);

        if (local.z > 0.5f)
            return forwardMultiplier;

        if (local.z < -0.5f)
            return backwardMultiplier;

        return strafeMultiplier;
    }
}