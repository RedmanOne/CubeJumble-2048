using UnityEngine;
using UnityEngine.UI;

public class MenusUI : MonoBehaviour
{
    [Header("Menus references")]
    [SerializeField] private Image mainMenuPanel;

    public void OpenMainMenu()
    {
        if (!mainMenuPanel.gameObject.activeSelf)
            mainMenuPanel.gameObject.SetActive(true);
    }

    public void CloseMainMenu()
    {
        if (mainMenuPanel.gameObject.activeSelf)
            mainMenuPanel.gameObject.SetActive(false);
    }

}
