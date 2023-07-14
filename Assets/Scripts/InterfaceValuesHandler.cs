using UnityEngine;
using Zenject;

public class InterfaceValuesHandler : MonoBehaviour
{
    private UIManager uiManager;
    private GameSettings gameSettings;
    private SignalBus signalBus;

    private int score;
    private int highscore;
    private int objectsLeft;


    [Inject]
    public void Construct(UIManager uiManager, GameSettings gameSettings, SignalBus signalBus)
    {
        this.uiManager = uiManager;
        this.gameSettings = gameSettings;
        this.signalBus = signalBus;

        signalBus.Subscribe<StartNewGameSignal>(NewGame);
        signalBus.Subscribe<EndGameSignal>(EndGameScore);
        signalBus.Subscribe<ObjectMergeSignal>(x => ScoreUpdate(x.power));
        signalBus.Subscribe<ObjectsCountChangedSignal>(x => ObjectsAmountUpdate(x.count));
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
        uiManager.ScoreChanged(score, highscore);
    }

    private void EndGameScore()
    {
        ScoreUpdate(0);
        uiManager.GameEnded(score, highscore);
    }

    private void ObjectsAmountUpdate(int currAmount)
    {
        objectsLeft = gameSettings.MaxObjectsAmount() - currAmount;
        uiManager.ObjectsAmountChanged(objectsLeft, gameSettings.MaxObjectsAmount());
    }

    public void ClearHighScore()
    {
        PlayerPrefs.SetInt("HighScore", 0);
    }

    private void OnDestroy()
    {
        signalBus.TryUnsubscribe<StartNewGameSignal>(NewGame);
        signalBus.TryUnsubscribe<EndGameSignal>(EndGameScore);
        signalBus.TryUnsubscribe<ObjectMergeSignal>(x => ScoreUpdate(x.power));
        signalBus.TryUnsubscribe<ObjectsCountChangedSignal>(x => ObjectsAmountUpdate(x.count));
    }
}        

