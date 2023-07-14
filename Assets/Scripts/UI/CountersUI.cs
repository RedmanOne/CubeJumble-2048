using UnityEngine;
using UnityEngine.UI;

public class CountersUI : MonoBehaviour
{
    [Header("Game counters references")]
    [SerializeField] private Image cubesAmountFillImage;
    [SerializeField] private Text cubesAmountText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text highScoreText;

    [Header("Menu counters references")]
    [SerializeField] private Text menuScoreText;
    [SerializeField] private Text menuHighScoreText;

    public void UpdateScoresUI(int score, int highscore)
    {
        highScoreText.text = $"Top: { highscore }";
        scoreText.text = $"Score: { score }";
    }

    public void UpdateObjectsAmountUI(int amountLeft, int amountMax)
    {
        if (cubesAmountFillImage != null)
            cubesAmountFillImage.fillAmount = (float)amountLeft / amountMax;

        cubesAmountText.text = $"{amountLeft}";
        if (amountLeft < 1)
        {
            cubesAmountText.text = "-";
        }
    }

    public void UpdateMenuScoreUI(int score, int highscore)
    {
        menuHighScoreText.text = $"Highest Score: { highscore }";
        menuScoreText.text = $"Current Score: { score }";
    }
}
