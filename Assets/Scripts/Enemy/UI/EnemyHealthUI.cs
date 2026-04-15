using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    [SerializeField] private TMP_Text enemyNameText;
    [SerializeField] private Image healthFillImage;

    private CharacterHealth characterHealth;
    private CharacterInfo characterInfo;

  void Awake()
  {
    if (characterHealth == null)
    {
        characterHealth = GetComponentInParent<CharacterHealth>();
    }

    if (characterInfo == null)
    {
        characterInfo = GetComponentInParent<CharacterInfo>();
    }
  }

  private void Start()
    {
        if (characterInfo != null && enemyNameText != null)
        {
            enemyNameText.text = characterInfo.CharacterName;
        }
    }

    private void Update()
    {
        if (characterHealth == null || healthFillImage == null) return;

        float healthPercent = (float)characterHealth.CurrentHealth / characterHealth.MaxHealth;
        healthFillImage.fillAmount = healthPercent;
    }
}