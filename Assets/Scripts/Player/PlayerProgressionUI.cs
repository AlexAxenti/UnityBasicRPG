using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProgressionUI : MonoBehaviour
{
    [SerializeField] private PlayerProgression playerProgression;
    [SerializeField] private Image experienceFillImage;
    [SerializeField] private TMP_Text playerLevelText;

    private void Update()
    {
        if (playerProgression == null || experienceFillImage == null || playerLevelText == null) return;

        float experiencePercent = (float)playerProgression.CurrentExperience / playerProgression.ExperienceToNextLevel;
        experienceFillImage.fillAmount = experiencePercent;

        playerLevelText.text = $"Level: {playerProgression.Level}";
    }
}