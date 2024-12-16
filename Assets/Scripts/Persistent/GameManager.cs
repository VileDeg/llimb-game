using System;
using UnityEngine;
using UnityEngine.SceneManagement;
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
#if UNITY_EDITOR
    [SerializeField]
    private SceneAsset mainMenuSceneAsset;

    [SerializeField]
    private SceneAsset youDiedSceneAsset;

    [SerializeField]
    private SceneAsset[] levelSceneAssets;
#endif

    [SerializeField]
    private string mainMenuSceneName;

    [SerializeField]
    private string youDiedSceneName;

    [SerializeField]
    private string[] levelSceneNames;

    [Header("Level Settings")]
    [SerializeField]
    private Vector2 _levelDimensions;

    private string[] _listOfLevelNames;
    private int _currentLevelIndex = 0;

    private GameState _readonly_gameState;

    public string MainMenuSceneName => mainMenuSceneName;
    public string YouDiedSceneName => youDiedSceneName;
    public string[] ListOfLevelNames => _listOfLevelNames;
    public int CurrentLevelIndex => _currentLevelIndex;

    public GameState CurrentGameState
    {
        get => _readonly_gameState;
        private set => _readonly_gameState = value;
    }

    private bool _isLevelTimerActive = false;
    private float _readonly_timeSpentInLevel = 0f;

    protected override void Awake()
    {
        base.Awake();

        _listOfLevelNames = levelSceneNames ?? Array.Empty<string>();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        // Update scene names based on SceneAssets for editor usability
        mainMenuSceneName = mainMenuSceneAsset != null ? mainMenuSceneAsset.name : string.Empty;
        youDiedSceneName = youDiedSceneAsset != null ? youDiedSceneAsset.name : string.Empty;

        if (levelSceneAssets != null) {
            levelSceneNames = new string[levelSceneAssets.Length];
            for (int i = 0; i < levelSceneAssets.Length; i++) {
                levelSceneNames[i] = levelSceneAssets[i] != null ? levelSceneAssets[i].name : string.Empty;
            }
        }
    }
#endif

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

    public void RestartCurrentLevel()
    {
        string currentLevel = ListOfLevelNames[CurrentLevelIndex];
        Debug.Log($"GameManager: Restarting current level: {currentLevel}");
        LoadScene(currentLevel);
    }

    public void GoToMainMenu()
    {
        Debug.Log("GameManager: Returning to main menu");
        LoadScene(MainMenuSceneName);
    }

    public void LoadYouDiedScene()
    {
        if (!string.IsNullOrEmpty(YouDiedSceneName)) {
            LoadScene(YouDiedSceneName);
        }
    }
    public void GameLost()
    {
        Debug.Log("GameManager: Player lost the game.");

        // Transition to a "Game Over" state or scene
        CurrentGameState = GameState.GameOver;
        if (!string.IsNullOrEmpty(YouDiedSceneName)) {
            LoadScene(YouDiedSceneName);
        } else {
            Debug.LogWarning("GameManager: No 'You Died' scene is set.");
        }
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
            LoadScene(_listOfLevelNames[index]);
        }
    }

    public void LoadScene(string sceneName)
    {
        StopLevelTimer();
        SceneManager.LoadScene(sceneName);
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
}
