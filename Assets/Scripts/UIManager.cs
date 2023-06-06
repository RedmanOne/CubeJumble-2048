using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("In-game UI references")]
    public Image cubesAmountFillImage;
    public Text cubesAmountText;
    public Text scoreText, highScoreText;

    [Header("Menu UI references")]
    public GameObject menuPanel;
    public Text menuScoreText, menuHighScoreText;

    private void Start()
    {
        InvokeRepeating("InterfaceUpdate",0,0.5f);
    }

    private void InterfaceUpdate()
    {
        if (GameManager.Instance.roundIsOver)
        {
            if(!menuPanel.activeSelf)
                menuPanel.SetActive(true);
            menuHighScoreText.text = $"Highest Score: {GameManager.Instance.highscore}";
            menuScoreText.text = $"Current Score: {GameManager.Instance.score}";

            return;
        }

        ScoreUIUpdate();
        CubesAmountUIUpdate();
    }

    private void ScoreUIUpdate()
    {
        highScoreText.text = $"Top: {GameManager.Instance.highscore}";
        scoreText.text = $"Score: {GameManager.Instance.score}";
    }

    private void CubesAmountUIUpdate()
    {
        cubesAmountFillImage.fillAmount = (float)GameManager.Instance.cubesAmountLeft / 25;
        cubesAmountText.text = $"{GameManager.Instance.cubesAmountLeft}";
        if (GameManager.Instance.cubesAmountLeft < 0)
        {
            cubesAmountText.text = "-";
        }
    }
}
