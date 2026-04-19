using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private CharacterEquipment characterEquipment;
    private CharacterCombat combat;
    private CharacterMotor motor;

    private Transform targetTransform;

    private void Awake()
    {
        characterEquipment = GetComponent<CharacterEquipment>();
        combat = GetComponent<CharacterCombat>();
        motor = GetComponent<CharacterMotor>();

        if (characterEquipment == null)
            Debug.LogError($"CharacterEquipment missing on {gameObject.name}");

        if (combat == null)
            Debug.LogError($"CharacterCombat missing on {gameObject.name}");

        if (motor == null)
            Debug.LogError($"CharacterMotor missing on {gameObject.name}");
    }

    private void Update()
    {
        if (targetTransform == null)
        {
            motor.SetMoveDirection(Vector3.zero);
            motor.Move();
            return;
        }

        Vector3 toTarget = targetTransform.position - transform.position;
        toTarget.y = 0f;

        float distance = toTarget.magnitude;
        Vector3 direction = distance > 0.001f ? toTarget.normalized : Vector3.zero;

        bool inAttackRange = distance <= characterEquipment.AttackRange;

        if (!inAttackRange)
        {
            motor.FaceDirection(direction);
            motor.SetMoveDirection(direction);
            motor.Move();
            return;
        }

        motor.SetMoveDirection(Vector3.zero);
        motor.Move();

        if (!combat.IsAttacking)
        {
            motor.FaceDirection(direction);
            combat.TryAttack();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CharacterInfo foundInfo = other.GetComponentInParent<CharacterInfo>();
        if (foundInfo == null || foundInfo.FactionType != FactionType.Player)
            return;

        CharacterHealth foundHealth = other.GetComponentInParent<CharacterHealth>();
        if (foundHealth != null)
        {
            targetTransform = foundHealth.transform.root;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CharacterHealth foundHealth = other.GetComponentInParent<CharacterHealth>();
        if (foundHealth != null && targetTransform == foundHealth.transform.root)
        {
            targetTransform = null;
        }
    }
}