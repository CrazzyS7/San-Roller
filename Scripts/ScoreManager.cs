//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI mFinalScoreText;
    public TextMeshProUGUI mHiScoreText;
    public TextMeshProUGUI mScoreText;

    private static ScoreManager mSingleton;
    private readonly int mScorePoints = 3;
    private int mHiScore = 0;
    public static ScoreManager ScoreManagerSingleton => mSingleton;

    public int Score
    { get; set; }

    private void Start()
    {
        if(PlayerPrefs.HasKey("highScore"))
        {
            mHiScore = PlayerPrefs.GetInt("highScore");
            mHiScoreText.text = "HI-SCORE: " + Score;
        }
        LoadScore();
        return;
    }

    private void Awake()
    {
        if(mSingleton == null)
        {
            mSingleton = this;
        }
        else if (mSingleton != this)
        {
            Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }
        return;
    }

    public void UpdateScores()
    {
        Score += mScorePoints;
        UpdateScoreText();
        return;
    }

    public void TimeScore(int _score)
    {
        Score += _score;
        UpdateScoreText();
        return;
    }

    private void UpdateScoreText()
    {
        if (Score > mHiScore)
        {
            mHiScore = Score;
        }
        mScoreText.text = "SCORE: " + Score;
        mHiScoreText.text = "HI-SCORE: " + mHiScore;
        mFinalScoreText.text = "YOUR SCORE: " + Score;
        return;
    }

    public void LoadScore()
    {
        if(PlayerPrefs.HasKey("playerScore"))
        {
            Score = PlayerPrefs.GetInt("playerScore");
            Debug.Log("Player score loaded");
        }
        else
        {
            Score = 0;
            Debug.Log("Player score reset");
        }
        return;
    }

    public void SaveScore()
    {
        PlayerPrefs.SetInt("highScore", mHiScore);
        PlayerPrefs.SetInt("playerScore", Score);
        return;
    }

    public void ResetScore()
    {
        Score = 0;
        return;
    }
}
