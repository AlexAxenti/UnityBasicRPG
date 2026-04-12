using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackCooldown = 0.5f;
    
    private float lastAttackTime;

    void Update()
    {
        Mouse mouse = Mouse.current;

        if (mouse != null && mouse.leftButton.wasPressedThisFrame && Time.time >= lastAttackTime + attackCooldown)   
        {
            lastAttackTime = Time.time;
            Ray ray = new Ray(transform.position + Vector3.up, transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, attackRange))
            {
                EnemyHealth enemy = hit.collider.GetComponentInParent<EnemyHealth>();

                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
        }
    }
}
