using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private CharacterStats characterStats;
    private CharacterEquipment characterEquipment;
    private CharacterCombat combat;

    private Transform targetTransform;

    private void Awake()
    {
        characterStats = GetComponent<CharacterStats>();
        characterEquipment = GetComponent<CharacterEquipment>();
        combat = GetComponent<CharacterCombat>();

        if (characterStats == null)
            Debug.LogError($"CharacterStats missing on {gameObject.name}");

        if (combat == null)
            Debug.LogError($"CharacterCombat missing on {gameObject.name}");
    }

    private void Update()
    {
        if (targetTransform == null)
            return;

        float distance = Vector3.Distance(transform.position, targetTransform.position);

        if (distance > characterEquipment.AttackRange)
        {
            MoveTowardTarget();
        }
        else
        {
            combat.TryAttack();
        }
    }

    private void MoveTowardTarget()
    {
        Vector3 direction = (targetTransform.position - transform.position).normalized;
        direction.y = 0f;

        transform.position += direction * characterStats.MovementSpeed * Time.deltaTime;

        if (direction != Vector3.zero)
        {
            transform.forward = direction;
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