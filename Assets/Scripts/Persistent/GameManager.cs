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
    [SerializeField]
    private SceneAsset mainMenuSceneAsset;

    [SerializeField]
    private SceneAsset youDiedSceneAsset;

    [SerializeField]
    private SceneAsset[] levelSceneAssets;

    [Header("Level Settings")]
    [SerializeField]
    private Vector2 _levelDimensions;

    private string[] _listOfLevelNames;
    private int _currentLevelIndex = 0;

    private GameState _readonly_gameState;

    // Public properties for accessing scenes and state
    public string MainMenuSceneName => mainMenuSceneAsset != null ? mainMenuSceneAsset.name : string.Empty;
    public string YouDiedSceneName => youDiedSceneAsset != null ? youDiedSceneAsset.name : string.Empty;
    public string[] ListOfLevelNames => _listOfLevelNames;
    public int CurrentLevelIndex => _currentLevelIndex;

    public GameState CurrentGameState
    {
        get => _readonly_gameState;
        private set
        {
            _readonly_gameState = value;
        }
    }

    private bool _isLevelTimerActive = false;
    private float _readonly_timeSpentInLevel = 0f;
    protected override void Awake()
    {
        base.Awake();

        // Initialize level names from SceneAssets
        if (levelSceneAssets != null && levelSceneAssets.Length > 0)
        {
            _listOfLevelNames = new string[levelSceneAssets.Length];
            for (int i = 0; i < levelSceneAssets.Length; i++)
            {
                _listOfLevelNames[i] = levelSceneAssets[i].name;
            }
        }
        else
        {
            _listOfLevelNames = Array.Empty<string>();
        }
    }

    private void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        switch (CurrentGameState)
        {
            case GameState.Playing:
                if (Input.GetKeyUp(KeyCode.R)) // Restart level
                {
                    RestartCurrentLevel();
                }
                else if (Input.GetKeyUp(KeyCode.N)) // Next level
                {
                    LoadLevel_Cheat(LoadLevelMode.Next);
                }
                else if (Input.GetKeyUp(KeyCode.B)) // Previous level
                {
                    LoadLevel_Cheat(LoadLevelMode.Previous);
                }
                else if (Input.GetKeyUp(KeyCode.Escape)) // Main menu
                {
                    GoToMainMenu();
                }
                break;

            case GameState.GameOver:
                break;
        }
    }


    // === Scene Loading Methods ===

    public void RestartCurrentLevel()
    {
        // Get the current level name and reload it
        string currentLevel = ListOfLevelNames[CurrentLevelIndex];
        LoadScene(currentLevel);
    }

    public void GoToMainMenu()
    {
        // Load the main menu scene
        LoadScene(MainMenuSceneName);
    }

    public void LoadYouDiedScene()
    {
        if (!string.IsNullOrEmpty(YouDiedSceneName))
        {
            LoadScene(YouDiedSceneName);
        }
    }


    private void LoadLevel_Cheat(LoadLevelMode mode)
    {
        switch (mode)
        {
            case LoadLevelMode.Next:
                if (_currentLevelIndex < _listOfLevelNames.Length - 1)
                {
                    LoadLevel(_currentLevelIndex + 1);
                }
                break;

            case LoadLevelMode.Current:
                LoadLevel(_currentLevelIndex);
                break;

            case LoadLevelMode.Previous:
                if (_currentLevelIndex > 0)
                {
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
        if (index >= 0 && index < _listOfLevelNames.Length)
        {
            _currentLevelIndex = index;
            LoadScene(_listOfLevelNames[index]);
        }
    }
    public bool EscapedLevel(Vector2 pos, Vector2 size)
    {
        Vector2 ps = pos + size;
        Vector2 half = _levelDimensions * 0.5f;

        return ps.x < -half.x || ps.x > half.x ||
               ps.y < -half.y || ps.y > half.y;
    }
    public void LoadScene(string sceneName)
    {
        StopLevelTimer();
        SceneManager.LoadScene(sceneName);
    }

    public void GameLost()
    {
        Debug.Log("You lost! Loading 'You Died' scene.");
        LoadYouDiedScene();
    }

    // === Timer Methods ===

    private void UpdateLevelTimer()
    {
        if (_isLevelTimerActive)
        {
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

    // === Debug Gizmos ===

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(_levelDimensions.x, _levelDimensions.y, 0));
    }
}
