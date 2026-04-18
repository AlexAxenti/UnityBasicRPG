using UnityEngine;
using UnityEngine.InputSystem;

//TODO switch to playerController? once movement is moved to CharacterMotor
public class PlayerCombatInput : MonoBehaviour
{
    private CharacterCombat combat;

    private void Awake()
    {
        combat = GetComponent<CharacterCombat>();

        if (combat == null)
            Debug.LogError($"CharacterCombat missing on {gameObject.name}");
    }

    private void Update()
    {
        Mouse mouse = Mouse.current;

        if (mouse != null && mouse.leftButton.wasPressedThisFrame)
        {
            combat.TryAttack();
        }
    }
}