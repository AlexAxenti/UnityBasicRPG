using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackCooldown = 1f;

    private Transform playerTransform;
    private PlayerHealth playerHealth;
    private float lastAttackTime;

    private void Update()
    {
        if (playerTransform == null) return;

        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance > attackRange)
        {
            MoveTowardPlayer();
        }
        else
        {
            TryAttack();
        }
    }

    private void MoveTowardPlayer()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        direction.y = 0f;

        transform.position += direction * moveSpeed * Time.deltaTime;

        if (direction != Vector3.zero)
        {
            transform.forward = direction;
        }
    }

    private void TryAttack()
    {
        if (Time.time < lastAttackTime + attackCooldown) return;

        lastAttackTime = Time.time;

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
            Debug.Log("Enemy attacked player");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth foundHealth = other.GetComponentInParent<PlayerHealth>();
        if (foundHealth != null)
        {
            playerHealth = foundHealth;
            playerTransform = foundHealth.transform.root;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerHealth foundHealth = other.GetComponentInParent<PlayerHealth>();
        if (foundHealth != null && playerTransform == foundHealth.transform.root)
        {
            playerTransform = null;
            playerHealth = null;
        }
    }
}