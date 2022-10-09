using Scene = UnityEngine.SceneManagement.Scene;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI mGameOverText;
    public TextMeshProUGUI mCompleteText;
    public TextMeshProUGUI mTimerText;
    public GameObject mCreditScreen;
    public GameObject mTitleScreen;
    public GameObject mScoreBoard;

    private static GameManager mSingleton;
    private GroundPiece[] mGroundPieces;
    private readonly int mNumScenes = 3;
    private int mDifficulty = -1;
    private int mTimer = 0;

    public static GameManager GameManagerSingleton => mSingleton;

    // Properties
    public bool IsGameOver
    { get; set; }

    private void Start()
    {
        if(PlayerPrefs.HasKey("continue"))
        {
            int difficulty = PlayerPrefs.GetInt("continue");
            PlayerPrefs.DeleteKey("continue");
            StartGame(difficulty);
            IsGameOver = false;
        }
        else
        {
            IsGameOver  = true;
        }
        SetupNewLevel();
        return;
    }

    public void StartGame(int _difficulty)
    {
        mTitleScreen.SetActive(false);
        mScoreBoard.SetActive(true);
        mDifficulty = _difficulty;
        IsGameOver = false;

        if (_difficulty < 0 )
        {
            mTimerText.text = "TIME: UNLIMITED";
            
        }
        else if(_difficulty > 0)
        {
            mTimer = 10;
            mTimerText.text = "TIME: " + mTimer;
            StartCoroutine(TimerCounter());
        }
        else
        {
            mTimer = 15;
            mTimerText.text = "TIME: " + mTimer;
            StartCoroutine(TimerCounter());
        }
        ScoreManager.ScoreManagerSingleton.LoadScore();
        return;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
        return;
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(0);
        return;
    }

    private void UpdateTimer()
    {
        mTimer--;
        mTimerText.text = "TIME: " + mTimer;

        if (mTimer < 0)
        {
            GameOver();
        }
        return;
    }

    public void GameOver()
    {
        mGameOverText.gameObject.SetActive(true);
        GameCom();
        return;
    }

    public void GameComplete()
    {
        mCompleteText.gameObject.SetActive(true);
        GameCom();
        return;
    }

    private void GameCom()
    {
        PlayerPrefs.DeleteKey("playerScore");
        PlayerPrefs.DeleteKey("continue");
        mCreditScreen.SetActive(true);
        mScoreBoard.SetActive(false);
        IsGameOver = true;
        return;
    }

    private void SetupNewLevel()
    {
        mGroundPieces = FindObjectsOfType<GroundPiece>();
        return;
    }

    private void Awake()
    {
        if (mSingleton == null)
        {
            mSingleton = this;
        }
        else if(mSingleton != this)
        {
            Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }
        return;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinished;
        return;
    }

    public void CheckComplete()
    {
        bool isFinished = true;
        foreach(GroundPiece piece in mGroundPieces)
        {
            if(!piece.IsColored)
            {
                isFinished = false;
                break;
            }
        }

        if(isFinished)
        {
            if(mDifficulty > 0)
            {
                int score = mTimer * 10;
                ScoreManager.ScoreManagerSingleton.TimeScore(score);
            }
            else if (mDifficulty == 0)
            {
                int score = mTimer * 3;
                ScoreManager.ScoreManagerSingleton.TimeScore(score);
            }
            NextLevel();
        }
        return;
    }

    private void NextLevel()
    {
        PlayerPrefs.SetInt("continue", mDifficulty);
        ScoreManager.ScoreManagerSingleton.SaveScore();
        
        if (SceneManager.GetActiveScene().buildIndex >= mNumScenes)
        {
            GameComplete();
        }
        else
        {
            int level = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(level);
        }
        return;
    }

    private void OnLevelFinished(Scene _scene, LoadSceneMode _mode)
    {
        SetupNewLevel();
        return;
    }

    IEnumerator TimerCounter()
    {
        
        while (!IsGameOver)
        {
            yield return new WaitForSeconds(1);
            UpdateTimer();
        }
    }
}
