
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndingUI : MonoBehaviour
{
    public TextMeshProUGUI endingTitleText;
    public TextMeshProUGUI endingDescriptionText;
    public TextMeshProUGUI finalStatsText;
    public Button restartButton;
    public Button quitButton;

    public void Initialize()
    {
        gameObject.SetActive(false);
        restartButton.onClick.AddListener(OnRestartGame);
        quitButton.onClick.AddListener(OnQuitGame);
    }

    public void ShowEnding(string title, string description, string finalStats)
    {
        endingTitleText.text = title;
        endingDescriptionText.text = description;
        finalStatsText.text = finalStats;
        gameObject.SetActive(true);
    }

    public void HideEnding()
    {
        gameObject.SetActive(false);
    }

    void OnRestartGame()
    {
        Debug.Log("Restarting game...");
        GameManager.Instance.StartNewGame();
        HideEnding();
    }

    void OnQuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
