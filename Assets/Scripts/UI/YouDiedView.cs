using UnityEngine;
using UnityEngine.UI;

// TODO: is also used in You Won scene, do something about it!
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
        restartButton.onClick.AddListener(GameManager.Instance.RestartCurrentLevel);
        mainMenuButton.onClick.AddListener(GameManager.Instance.LoadMainMenu);
    }
}
