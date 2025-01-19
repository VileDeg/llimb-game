using UnityEngine;
using UnityEngine.UI;

public class YouDiedView : AView
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        if (ScoreManager.Instance)
        {
            ScoreManager.Instance.ResetScore();
        }
    }

    public override void Initialize()
    {
        restartButton.onClick.AddListener(RestartLevel);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    private void RestartLevel()
    {
        string currentLevel = GameManager.Instance.ListOfLevelNames[GameManager.Instance.CurrentLevelIndex];
        GameManager.Instance.LoadScene(currentLevel);
    }

    private void GoToMainMenu()
    {
        GameManager.Instance.LoadScene(GameManager.Instance.MainMenuSceneName);
    }
}
