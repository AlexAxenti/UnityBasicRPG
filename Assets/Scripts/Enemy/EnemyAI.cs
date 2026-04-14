using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 1f;
    private CharacterStats characterStats;

    private Transform targetTransform;
    private CharacterHealth targetHealth;
    private float lastAttackTime;

    private void Awake()
    {
        characterStats = GetComponent<CharacterStats>();
        if (characterStats == null)
        {
            Debug.LogError("CharacterStats component not found on " + gameObject.name);
        }
    }

    private void Update()
    {
        if (targetTransform == null) return;

        float distance = Vector3.Distance(transform.position, targetTransform.position);

        if (distance > characterStats.AttackRange)
        {
            MoveTowardTarget();
        }
        else
        {
            TryAttack();
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

    private void TryAttack()
    {
        if (Time.time < lastAttackTime + attackCooldown) return;

        lastAttackTime = Time.time;

        if (targetHealth != null)
        {
            targetHealth.TakeDamage(characterStats.Damage);
            Debug.Log("Enemy attacked target");
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
            targetHealth = foundHealth;
            targetTransform = foundHealth.transform.root;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CharacterHealth foundHealth = other.GetComponentInParent<CharacterHealth>();
        if (foundHealth != null && targetTransform == foundHealth.transform.root)
        {
            targetTransform = null;
            targetHealth = null;
        }
    }
}