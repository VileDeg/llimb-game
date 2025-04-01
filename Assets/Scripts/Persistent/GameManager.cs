using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : SingletonBase<GameManager>
{
    public enum GameState
    {
        Invalid = -1,
        InMenu,
        Playing,
        GameOver
    }

    private enum LoadLevelMode
    {
        Invalid = -1,
        Next,
        Current,
        Previous
    }

    [Header("Scenes")]

    [SerializeField]
    private string _mainMenuSceneName;

    [SerializeField]
    private string _youDiedSceneName;

    [SerializeField]
    private string _youWonSceneName;

    [SerializeField]
    private string[] _levelSceneNames;

    [Header("Level Settings")]
    [SerializeField]
    private Vector2 _levelDimensions;

    private string[] _listOfLevelNames;
    private int _currentLevelIndex = 0;

    private GameState _readonly_gameState;

    public GameState CurrentGameState
    {
        get => _readonly_gameState;
        private set => _readonly_gameState = value;
    }

    private bool _isLevelTimerActive = false;
    private float _readonly_timeSpentInLevel = 0f;

    // TODO: compute automatically based on enemies in scene.
    // Is currently hardcoded in editor!
    [SerializeField]
    private int _numEnemiesInLevel = 0;

    private int _numEnemiesAlive = 0;

    protected override void Awake()
    {
        base.Awake();

        // TODO: why do we need this?
        _listOfLevelNames = _levelSceneNames ?? Array.Empty<string>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        if (Input.GetKeyUp(KeyCode.R)) // Restart level
        {
            RestartCurrentLevel();
        } else if (Input.GetKeyUp(KeyCode.N)) // Next level
          {
            LoadLevel_Cheat(LoadLevelMode.Next);
        } else if (Input.GetKeyUp(KeyCode.B)) // Previous level
          {
            LoadLevel_Cheat(LoadLevelMode.Previous);
        } else if (Input.GetKeyUp(KeyCode.Escape)) // Main menu
          {
            GoToMainMenu();
        }
    }
     

    public void GoToMainMenu()
    {
        Debug.Log("GameManager: Returning to main menu");
        LoadScene(_mainMenuSceneName);
    }

    public void LoadYouDiedScene()
    {
        LoadScene(_youDiedSceneName);
    }

    public void LoadYouWonScene()
    {
        LoadScene(_youWonSceneName);
    }

    public void GameLost()
    {
        Debug.Log("GameManager: Player lost the game.");

        // Transition to a "Game Over" state or scene
        CurrentGameState = GameState.GameOver;
        LoadScene(_youDiedSceneName);
    }

    private void LoadLevel_Cheat(LoadLevelMode mode)
    {
        switch (mode) {
            case LoadLevelMode.Next:
                if (_currentLevelIndex < _listOfLevelNames.Length - 1) {
                    LoadLevel(_currentLevelIndex + 1);
                }
                break;

            case LoadLevelMode.Current:
                LoadLevel(_currentLevelIndex);
                break;

            case LoadLevelMode.Previous:
                if (_currentLevelIndex > 0) {
                    LoadLevel(_currentLevelIndex - 1);
                }
                break;

            default:
                Debug.Assert(false, "Invalid LoadLevelMode");
                break;
        }
    }

    public void LoadLevel(int index)
    {
        if (index >= 0 && index < _listOfLevelNames.Length) {
            _currentLevelIndex = index;
            _numEnemiesAlive = _numEnemiesInLevel; // TODO: rework
            LogUtil.Info($"Loading level: {_listOfLevelNames[index]}");
            LoadScene(_listOfLevelNames[index]);
        }
    }

    public void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName)) {
            LogUtil.Warn("GameManager: Scene name is null or empty.");
            return;
        }
        StopLevelTimer();
        SceneManager.LoadScene(sceneName);
    }

    public void RestartCurrentLevel()
    {
        LoadLevel(_currentLevelIndex);
    }

    public void StartGame()
    {
        LoadLevel(0);
    }

    public void QuitGame()
    {
        // Quit the game
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void LoadMainMenu()
    {
        LoadScene(_mainMenuSceneName);
    }

    private void UpdateLevelTimer()
    {
        if (_isLevelTimerActive) {
            _readonly_timeSpentInLevel += Time.deltaTime;
        }
    }

    public void StartLevelTimer()
    {
        _readonly_timeSpentInLevel = 0f;
        _isLevelTimerActive = true;
    }

    public void StopLevelTimer()
    {
        _isLevelTimerActive = false;
    }

    private void FindAllEnemiesInScene()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        _numEnemiesAlive = enemies.Length;
        LogUtil.Info($"Number of enemies in the scene: {_numEnemiesAlive}");
    }


    // TODO: rework:
    public void RegisterEnemyDeath()
    {
        _numEnemiesAlive--;
        LogUtil.Info($"Enemy killed. Remaining enemies: {_numEnemiesAlive}");
        if (_numEnemiesAlive <= 0) {
            LogUtil.Info("All enemies killed. Level complete!");
            LoadYouWonScene();
        }
    }


    /* *******************************
     * EVENT CALLBACKS
     * *******************************/


    // Callback function called when a new scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // TODO:
        //LogUtil.Info($"Scene loaded: {scene.name}, Mode: {mode}");
        //FindAllEnemiesInScene();
    }
}
