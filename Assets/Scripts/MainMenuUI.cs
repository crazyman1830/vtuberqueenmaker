using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Button newGameButton;
    public Button loadGameButton;
    public Button settingsButton;
    public Button quitButton;

    public AudioClip buttonClickSFX;

    void Start()
    {
        newGameButton.onClick.AddListener(OnNewGame);
        loadGameButton.onClick.AddListener(OnLoadGame);
        settingsButton.onClick.AddListener(OnSettings);
        quitButton.onClick.AddListener(OnQuit);
    }

    public void OnNewGame()
    {
        Debug.Log("Starting a new game...");
        if (buttonClickSFX != null)
        {
            GameManager.Instance.SoundManager.PlaySFX(buttonClickSFX);
        }
        GameManager.Instance.StartNewGame();
    }

    public void OnLoadGame()
    {
        Debug.Log("Loading a saved game...");
        if (GameManager.Instance.SaveManager.LoadGame())
        {
            GameManager.Instance.SceneLoader.LoadScene("Game");
        }
    }

    public void OnSettings()
    {
        Debug.Log("Opening settings...");
    }

    public void OnQuit()
    {
        Debug.Log("Quitting the game...");
        Application.Quit();
    }
}