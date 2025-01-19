using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : SingletonBase<ScoreManager>
{
    [SerializeField] 
    private TextMeshProUGUI scoreText;

    private int _score;

    protected override void Awake()
    {
        base.Awake();
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        // Do not init in Start, it gets called OnSceneLoaded, otherwise would be called twice!
        // InitScore();
    }

    private void InitScore()
    {
        _score = PlayerPrefs.GetInt("Score", 0);
        UpdateScoreText();
    }

    public void AddScore(int score)
    {
        _score += score;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (!TryFindScoreTextIfNull()) {
            return;
        }
        if (scoreText != null) {
            scoreText.text = "Score: " + _score;
            Debug.Log($"Score updated: {_score}");
        } else {
            Debug.LogError("ScoreText object not found in the scene.");
        }
    }

    // TODO: Find better approach to find Score text in scene.
    private bool TryFindScoreTextIfNull()
    {
        if (scoreText != null) {
            return true;
        }

        // Attempt to find the ScoreText object by name
        GameObject scoreObject = GameObject.Find("Score");
        if (scoreObject != null) {
            scoreText = scoreObject.GetComponent<TextMeshProUGUI>();
            return true;
        } else {
            Debug.LogWarning("ScoreText object not found. Ensure there is a UI element named 'Score' in the scene.");
        }

        return false;
    }

    public int GetScore()
    {
        return _score;
    }

    public void ResetScore()
    {
        _score = 0;
        PlayerPrefs.SetInt("Score", _score);
        PlayerPrefs.Save();

    }
    
    public void SaveScore()
    {
        PlayerPrefs.SetInt("Score", _score);
        PlayerPrefs.Save();
    }


    /* *******************************
     * EVENT CALLBACKS
     * *******************************/


    // Callback function called when a new scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene loaded: {scene.name}, Mode: {mode}");

        InitScore();
    }

}