﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Scores))]
public class UIManager : MonoBehaviour
{
    [Header("In-game UI references")]
    [SerializeField] private Image cubesAmountFillImage;
    [SerializeField] private Text cubesAmountText;
    [SerializeField] private Text scoreText, highScoreText;

    [Header("Menu references")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private Text menuScoreText, menuHighScoreText;


    private void Awake()
    {
        EventBus.onEndGame += OpenMenu;
    }

    public void ScoreUpdate(int score, int highscore)
    {
        highScoreText.text = $"Top: { highscore }";
        scoreText.text = $"Score: { score }";
    }

    public void FinalScore(int score, int highscore)
    {
        menuHighScoreText.text = $"Highest Score: { highscore }";
        menuScoreText.text = $"Current Score: { score }";
        OpenMenu();
    }

    public void ObjectsAmountUpdate(int objectsLeft)
    {
        if (cubesAmountFillImage == null)
            return;
        cubesAmountFillImage.fillAmount = (float)objectsLeft / GameSettings.Instance.MaxObjectsAmount();
        cubesAmountText.text = $"{objectsLeft}";
        if (objectsLeft < 1)
        {
            cubesAmountText.text = "-";
        }
    }

    private void OpenMenu()
    {
        if (!menuPanel.activeSelf)
            menuPanel.SetActive(true);
    }
}
