using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameManager;


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



    [SerializeField]
    private string[] _listOfLevelNames;

    [SerializeField]
    private string _mainMenuSceneName;

    [SerializeField]
    private string _youDiedSceneName;

    private int _currentLevelIndex = 0;

    // === GAME STATE

    private GameState _readonly_gameState;

    public GameState _GameState
    {
        get { return _readonly_gameState; }
        private set {
            _readonly_gameState = value;
            //OnGameStateChanged?.Invoke(_readonly_gameState);
        }
    }

    // === LEVEL TIMER

    /* seconds */
    private float _readonly_timeSpentInLevel = 0f;
    private float _TimeSpentInLevel
    {
        get { return _readonly_timeSpentInLevel; }
        set {
            _readonly_timeSpentInLevel = value;
            //OnTimeSpentInLevelChanged?.Invoke(_readonly_timeSpentInLevel);
        }
    }
    private bool _isLevelTimerActive = false;

    [HideInInspector]
    public bool IsGamePaused = false;

    //public static event Action<float> OnTimeSpentInLevelChanged;
    public static event Action<float> OnTimeSpentInLevelOneSecondPassed;

    protected override void Awake()
    {
        base.Awake();

        // Subscribe to events
        //OnTimeSpentInLevelOneSecondPassed += OnTimeSpentInLevelOneSecondPassedHandler;
    }


    private void OnDestroy()
    {
        // Unsubscribe from events
        //OnTimeSpentInLevelOneSecondPassed -= OnTimeSpentInLevelOneSecondPassedHandler;
    }




    // Update is called once per frame
    void Update()
    {
#if FALSE
        ProcessInput();


        // Update the timer if the level is active
        if (_isLevelTimerActive) {
            UpdateLevelTimer();
        }
#endif
    }

    // Process cheats and stuff
    private void ProcessInput()
    {
        
        switch (_GameState) {
            case GameState.Playing:
                
                if (Input.GetKeyUp(KeyCode.N)) {
                    LoadLevel_Cheat(LoadLevelMode.Next);
                } else if (Input.GetKeyUp(KeyCode.B)) {
                    LoadLevel_Cheat(LoadLevelMode.Previous);
                } else if (Input.GetKeyUp(KeyCode.I)) {
                    //OnPlayerImmortalityEnabled?.Invoke();
                //} else if (Input.GetKey(KeyCode.RightShift) && Input.GetKeyUp(KeyCode.T)) { // Skip time
                //    _TimeSpentInLevel += 30; // Skip 30 secs
                //} else if (Input.GetKey(KeyCode.RightShift) && Input.GetKeyUp(KeyCode.Alpha1)) {
                //    AddPlayerUpdarade(PlayerUpgradeKind.DualGun);
                //} else if (Input.GetKey(KeyCode.RightShift) && Input.GetKeyUp(KeyCode.Alpha2)) {
                //    AddPlayerUpdarade(PlayerUpgradeKind.FireRate);
                //} else if (Input.GetKey(KeyCode.RightShift) && Input.GetKeyUp(KeyCode.Alpha3)) {
                //    AddPlayerUpdarade(PlayerUpgradeKind.Durability);
                } else if (Input.GetKeyUp(KeyCode.Escape)) { // Return to main menu
                    LoadScene(_mainMenuSceneName);
                }
                break;
            case GameState.GameOver:
                break;
        }
    }


    private void LoadLevel_Cheat(LoadLevelMode mode)
    {
        //CleanupOnLevelEnd();
        LoadLevel_ByMode(mode);
    }

    private void LoadLevel_ByMode(LoadLevelMode mode)
    {
        switch (mode) {
            case LoadLevelMode.Next:
                if (_currentLevelIndex < _listOfLevelNames.Length - 1) {
                    LoadLevel(_listOfLevelNames[_currentLevelIndex + 1]);
                } else {
                    LogUtil.Info("GameManager: No level to load: hit end of the list");
                }
                break;
            case LoadLevelMode.Current:
                if (_currentLevelIndex < _listOfLevelNames.Length && _currentLevelIndex > -1) {
                    LoadLevel(_listOfLevelNames[_currentLevelIndex]);
                } else {
                    LogUtil.Error("Internal error: invalid level index.");
                }
                break;
            case LoadLevelMode.Previous:
                if (_currentLevelIndex > 0) {
                    LoadLevel(_listOfLevelNames[_currentLevelIndex - 1]);
                } else {
                    LogUtil.Info("GameManager: No level to load: hit start of the list");
                }
                break;
            default:
                Debug.Assert(false);
                break;

        }
    }


    public void LoadLevel(string levelName)
    {
        int sceneIndex = GetLevelIndex(levelName);

        _currentLevelIndex = sceneIndex;
        LoadScene(levelName);
    }

    private int GetLevelIndex(string leevelName)
    {
        if (string.IsNullOrEmpty(leevelName)) {
            throw new Exception("GameManager: Scene name null or empty");
        }

        int levelIndex = Array.IndexOf(_listOfLevelNames, leevelName);
        if (levelIndex < 0) {
            return -1; // is not level
        }

        return levelIndex;
    }


    public void LoadScene(string sceneName)
    {
        //ResetPlayerScore();
        StopLevelTimer();

        SceneManager.LoadScene(sceneName);
    }



    public void GameLost()
    {
        LogUtil.Info("You lost! :(");
        LoadScene(_youDiedSceneName);
    }



    // <<< TIMER 

    private void UpdateLevelTimer()
    {
        int wholeSecondsBefore = (int)_TimeSpentInLevel;
        _TimeSpentInLevel += Time.deltaTime;
        if ((int)_TimeSpentInLevel > wholeSecondsBefore) {
            OnTimeSpentInLevelOneSecondPassed?.Invoke(_TimeSpentInLevel);
        }
    }

    public void ResetAndStartLevelTimer()
    {
        _TimeSpentInLevel = 0f; // Reset the timer
        _isLevelTimerActive = true;  // Start tracking time
    }

    public void StopLevelTimer()
    {
        _isLevelTimerActive = false; // Stop tracking time
    }

    // >>> TIMER


    private void PauseGame()
    {
        IsGamePaused = true;
    }

    private void UnpauseGame()
    {
        IsGamePaused = false;
    }


    /*  ========================================== *
     *  EVENT HANDLERS                             *
     *  ========================================== */


    private void OnTimeSpentInLevelOneSecondPassedHandler(float timeSpentInLevel)
    {
        /*
        _Credits += _creditsPerSecond;

        if (timeSpentInLevel > _levelStaticData[_currentLevelIndex].SecondsToFinish) {
            // Finish level
            if (_currentLevelIndex != _listOfLevelNames.Length - 1) {
                // If not last level
                OnLevelWon?.Invoke();
            } else {
                // Destroy all enemies and meteorites
                // Summon boss
                OnSummonBoss?.Invoke();
            }
            // Load next level in OnLevelEndAnimationCompletedHandler
        }
        */
    }
}


