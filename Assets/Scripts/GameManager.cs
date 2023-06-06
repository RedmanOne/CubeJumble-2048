using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //initialize singleton
    public static GameManager Instance { get; set; }
    private void Awake()
    {
        Application.targetFrameRate = 60;
        //check if there is another instance
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    [Header("References")]
    public CubeSpawner cubeSpawner;
    public CubesManager cubesManager;

    [Header("Gameplay settings")]
    public int maxCubesAmount = 25;
    public float dragSensitivitySetting = 7f;
    public float launchForce = 15f;
    
    //Game values
    public int score { get; set; }
    public int highscore { get; set; }
    public int cubesAmountLeft { get; set; }
    public bool roundIsOver { get; set; }

    private void Start()
    {
        highscore = PlayerPrefs.GetInt("HighScore");
        RoundStart();
    }

    private void Update()
    {
        if(cubesAmountLeft > 0)
        {
            cubesAmountLeft = maxCubesAmount - CubesManager.Instance.activeCubes.Count;
        }
        else
        {
            if (cubesAmountLeft < -100)
                return;

            cubesAmountLeft = -1000; //to ensure that RoundOver() called once
            Invoke("RoundOver", 2f);
            return;
        }
    }

    //public void ClearSavedHighScore()
    //{
    //    highscore = 0;
    //    PlayerPrefs.SetInt("HighScore", highscore);
    //}

    public void ScoreUpdate(int points)
    {
        score += points;
        if (highscore < score)
            highscore = score;
    }

    public void RoundStart()
    {
        score = 0;
        cubesAmountLeft = maxCubesAmount;
        roundIsOver = false;
        cubesManager.ClearTheScene();
        cubeSpawner.SpawnNewCube();
    }

    private void RoundOver()
    {
        roundIsOver = true;
        cubesManager.ClearTheScene();
        PlayerPrefs.SetInt("HighScore", highscore);
    }
}
