using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField] private CountersUI countersUI;
    [SerializeField] private MenusUI menusUI;

    public void ScoreChanged(int score, int highscore)
    {
        countersUI.UpdateScoresUI(score, highscore);
    }

    public void GameEnded(int score, int highscore)
    {
        countersUI.UpdateMenuScoreUI(score, highscore);
        menusUI.OpenMainMenu();
    }

    public void ObjectsAmountChanged(int amountLeft, int amountMax)
    {
        countersUI.UpdateObjectsAmountUI(amountLeft, amountMax);
    }

    public void CloseMainMenu()
    {
        menusUI.CloseMainMenu();
    }

}
