using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackCooldown = 0.5f;
    
    private float lastAttackTime;
    private bool attackWindowOpen;
    private bool hasHitThisSwing;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Mouse mouse = Mouse.current;

        if (mouse != null && mouse.leftButton.wasPressedThisFrame)
        {
            TryAttack();
        }

        if (attackWindowOpen && !hasHitThisSwing)
        {
            CheckForHit();
        }
    }

    public void StartAttackWindow()
    {
        attackWindowOpen = true;
        hasHitThisSwing = false;
    }

    public void EndAttackWindow()
    {
        attackWindowOpen = false;
    }

    private void TryAttack()
    {
        if (Time.time < lastAttackTime + attackCooldown) 
            return;

        lastAttackTime = Time.time;
        animator.SetTrigger("Attack");
    }

    private void CheckForHit()
    {
        Ray ray = new Ray(transform.position + Vector3.up, transform.forward);

        if (Physics.Raycast(
            ray, 
            out RaycastHit hit, 
            attackRange, 
            Physics.DefaultRaycastLayers, 
            QueryTriggerInteraction.Ignore))
        {
            EnemyHealth enemy = hit.collider.GetComponentInParent<EnemyHealth>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                hasHitThisSwing = true;
            }
        }
    }
}
