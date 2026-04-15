using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private CharacterHealth characterHealth;
    [SerializeField] private Image healthFillImage;

    private void Update()
    {
        if (characterHealth == null || healthFillImage == null) return;

        float healthPercent = (float)characterHealth.CurrentHealth / characterHealth.MaxHealth;
        healthFillImage.fillAmount = healthPercent;
    }
}