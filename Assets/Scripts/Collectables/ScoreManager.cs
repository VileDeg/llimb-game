using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI scoreText;

    private int _score;

    public static ScoreManager Instance { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
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
        scoreText.text = "Score: " + _score;
        Debug.Log($"Score updated: {_score}");
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
}