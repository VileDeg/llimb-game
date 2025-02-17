using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : AView
{
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button quitGameButton;
    private void Start()
    {
        Initialize();
    }
    public override void Initialize()
    {
        // Set up button listeners
        startGameButton.onClick.AddListener(GameManager.Instance.StartGame);
        quitGameButton.onClick.AddListener(GameManager.Instance.QuitGame);
    }
}
