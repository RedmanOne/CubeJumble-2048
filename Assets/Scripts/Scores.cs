using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIManager))]
public class Scores : MonoBehaviour
{
    private UIManager uiManager;
    private int score;
    private int highscore;
    private int objectsLeft;

    private void Awake()
    {
        uiManager = GetComponent<UIManager>();

        EventBus.onObjectMerge += ScoreUpdate;
        EventBus.onStartNewGame += NewGame;
        EventBus.onObjectsCountChanged += ObjectsAmountUpdate;
        EventBus.onEndGame += FinalScoreUpdate;
    }

    private void NewGame()
    {
        score = 0;
        highscore = PlayerPrefs.GetInt("HighScore");
        ScoreUpdate(0);
    }

    private void ScoreUpdate(int points)
    {
        score += points;
        if (highscore < score)
        {
            highscore = score;
            PlayerPrefs.SetInt("HighScore", highscore);
        }
        uiManager.ScoreUpdate(score, highscore);
    }

    private void FinalScoreUpdate()
    {
        ScoreUpdate(0);
        uiManager.FinalScore(score, highscore);
    }

    private void ObjectsAmountUpdate(int currAmount)
    {
        objectsLeft = GameSettings.Instance.MaxObjectsAmount() - currAmount;
        uiManager.ObjectsAmountUpdate(objectsLeft);
    }

    public void ClearHighScore()
    {
        PlayerPrefs.SetInt("HighScore", 0);
    }
}        

