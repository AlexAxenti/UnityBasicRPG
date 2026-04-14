using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 0.5f;
    
    private float lastAttackTime;
    private bool attackWindowOpen;
    private bool hasHitThisSwing;
    private CharacterStats characterStats;

    private Animator animator;

    private void Awake()
    {
        characterStats = GetComponent<CharacterStats>();
        animator = GetComponent<Animator>();

        if (characterStats == null)
        {
            Debug.LogError("CharacterStats component not found on " + gameObject.name);
            
        }
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
            characterStats.AttackRange, 
            Physics.DefaultRaycastLayers, 
            QueryTriggerInteraction.Ignore))
        {
            CharacterInfo hitInfo = hit.collider.GetComponentInParent<CharacterInfo>();
            if (hitInfo == null || hitInfo.FactionType != FactionType.Enemy)
                return;

            CharacterHealth enemy = hit.collider.GetComponentInParent<CharacterHealth>();

            if (enemy != null)
            {
                enemy.TakeDamage(characterStats.Damage);
                hasHitThisSwing = true;
            }
        }
    }
}
