using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI mFinalScoreText;
    public TextMeshProUGUI mHiScoreText;
    public TextMeshProUGUI mScoreText;

    private static ScoreManager mSingleton;
    private readonly int mScorePoints = 2;
    private int mHiScore = 0;
    private int mScore = 0;

    public static ScoreManager ScoreManagerSingleton => mSingleton;

    private void Start()
    {
        if(PlayerPrefs.HasKey("highScore"))
        {
            mHiScore = PlayerPrefs.GetInt("highScore");
            mHiScoreText.text = "HI-SCORE: " + mHiScore;
        }
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

    public int UpdateScore
    {
        get { return mScore; }
        set { mScore = value; }
    }

    public void UpdateScores()
    {
        UpdateScore += mScorePoints;
        UpdateScoreText();
        return;
    }

    public void TimeScore(int _score)
    {
        mScore += _score;
        UpdateScoreText();
        return;
    }

    private void UpdateScoreText()
    {
        if (mScore > mHiScore)
        {
            mHiScore = mScore;
            PlayerPrefs.SetInt("highScore", mHiScore);
        }
        mScoreText.text = "SCORE: " + mScore;
        mHiScoreText.text = "HI-SCORE: " + mHiScore;
        mFinalScoreText.text = "YOUR SCORE: " + mScore;
        return;
    }

    public void ResetScore()
    {
        mScore = 0;
        return;
    }
}
