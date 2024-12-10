using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : AView
{
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button quitGameButton;
    private void Awake()
    {
        Initialize();
    }
    public override void Initialize()
    {
        // Set up button listeners
        startGameButton.onClick.AddListener(StartGame);
        quitGameButton.onClick.AddListener(QuitGame);
    }

    private void StartGame()
    {
        // Load the first level in the list
        string firstLevel = GameManager.Instance.ListOfLevelNames[0];
        GameManager.Instance.LoadScene(firstLevel);
    }

    private void QuitGame()
    {
        // Quit the game
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
