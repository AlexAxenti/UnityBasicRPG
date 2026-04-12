using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private TMP_Text enemyNameText;
    [SerializeField] private Image healthFillImage;

    private void Start()
    {
        if (enemyHealth != null && enemyNameText != null)
        {
            enemyNameText.text = enemyHealth.EnemyName;
        }
    }

    private void Update()
    {
        if (enemyHealth == null || healthFillImage == null) return;

        float healthPercent = (float)enemyHealth.CurrentHealth / enemyHealth.MaxHealth;
        healthFillImage.fillAmount = healthPercent;
    }
}