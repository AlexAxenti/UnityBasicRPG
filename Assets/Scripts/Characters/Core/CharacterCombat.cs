using UnityEngine;

public class CharacterCombat : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 0.5f;

    private float lastAttackTime;
    private bool attackWindowOpen;
    private bool hasHitThisSwing;

    private CharacterInfo characterInfo; 
    private CharacterStats characterStats;
    private CharacterEquipment equipment;
    private Animator animator;

    private void Awake()
    {
        characterStats = GetComponent<CharacterStats>();
        equipment = GetComponent<CharacterEquipment>();
        characterInfo = GetComponent<CharacterInfo>();
        animator = GetComponent<Animator>();

        if (characterStats == null)
            Debug.LogError($"CharacterStats missing on {gameObject.name}");

        if (animator == null)
            Debug.LogError($"Animator missing on {gameObject.name}");

        if (characterInfo == null)
            Debug.LogError($"CharacterInfo missing on {gameObject.name}");
    }

    public bool TryAttack()
    {
        if (Time.time < lastAttackTime + attackCooldown)
            return false;

        lastAttackTime = Time.time;
        animator.SetTrigger("Attack");
        return true;
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

    private void Update()
    {
        if (attackWindowOpen && !hasHitThisSwing)
        {
            CheckForHit();
        }
    }

    private void CheckForHit()
    {
        Vector3 origin = transform.position + Vector3.up;

        Vector3 direction = transform.forward;
        float range = GetAttackRange();

        if (Physics.Raycast(
            origin,
            direction,
            out RaycastHit hit,
            range,
            Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Ignore))
        {
            CharacterInfo hitInfo = hit.collider.GetComponentInParent<CharacterInfo>();
            if (hitInfo == null || !FactionRules.CanAttack(characterInfo.FactionType, hitInfo.FactionType))
                return;

            CharacterHealth targetHealth = hit.collider.GetComponentInParent<CharacterHealth>();
            if (targetHealth == null)
                return;

            float totalDamage = GetDamage();
            targetHealth.TakeDamage(totalDamage);
            hasHitThisSwing = true;
        }
    }

    //TODO should these 2 be here or should it be centralized in character stats or elsewhere
    public float GetDamage()
    {
        float baseDamage = characterStats != null ? characterStats.Damage : 0f;
        float weaponBonus = equipment != null ? equipment.DamageBonus : 0f;
        return baseDamage + weaponBonus;
    }

    public float GetAttackRange()
    {
        if (equipment != null && equipment.EquippedWeapon != null)
            return equipment.AttackRange;

        return characterStats != null ? characterStats.AttackRange : 1f;
    }
}