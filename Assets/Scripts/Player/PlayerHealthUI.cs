using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image healthFillImage;

    private void Update()
    {
        if (playerHealth == null || healthFillImage == null) return;

        float healthPercent = (float)playerHealth.CurrentHealth / playerHealth.MaxHealth;
        healthFillImage.fillAmount = healthPercent;
    }
}