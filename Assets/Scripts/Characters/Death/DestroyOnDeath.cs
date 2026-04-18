using UnityEngine;

public class DestroyOnDeath : MonoBehaviour
{
    [SerializeField] private float destroyDelay = 0f;

    private CharacterHealth characterHealth;

    private void Awake()
    {
        characterHealth = GetComponent<CharacterHealth>();
    }

    private void OnEnable()
    {
        if (characterHealth != null)
            characterHealth.OnDied += HandleDied;
    }

    private void OnDisable()
    {
        if (characterHealth != null)
            characterHealth.OnDied -= HandleDied;
    }

    private void HandleDied(CharacterHealth health)
    {
        Destroy(gameObject, destroyDelay);
    }
}