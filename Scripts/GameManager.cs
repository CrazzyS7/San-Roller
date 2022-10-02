using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;
using System.Runtime.CompilerServices;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI mGameOverText;
    public TextMeshProUGUI mCompleteText;
    public TextMeshProUGUI mTimerText;
    public GameObject mCreditScreen;
    public GameObject mTitleScreen;
    public GameObject mScoreBoard;
    public Button mRestartButton;
    public Button mExitButton;

    private static GameManager mSingleton;
    private GroundPiece[] mGroundPieces;
    private readonly int mScore = 0;
    private int mDifficulty = -1;
    private int mTimer = 0;

    // Properties
    public bool IsGameOver
    { get; set; }

    public void StartGame(int _difficulty)
    {
        mTitleScreen.SetActive(false);
        mDifficulty = _difficulty;

        if (_difficulty < 0 )
        {
            ScoreManager.ScoreManagerSingleton.UpdateScore = mScore;
            mTimerText.text = "TIME: UNLIMITED";
            mScoreBoard.SetActive(true);
        }
        else if(_difficulty > 0)
        {
            mTimer = 10;
            mScoreBoard.SetActive(true);
            ScoreManager.ScoreManagerSingleton.UpdateScore = mScore;
            StartCoroutine(TimerCounter());
        }
        else
        {
            mTimer = 15;
            mScoreBoard.SetActive(true);
            ScoreManager.ScoreManagerSingleton.UpdateScore = mScore;
            StartCoroutine(TimerCounter());
        }
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
        mCreditScreen.SetActive(true);
        mScoreBoard.SetActive(false);
        IsGameOver = true;
        return;
    }

    public void GameComplete()
    {
        mCompleteText.gameObject.SetActive(true);
        mCreditScreen.SetActive(true);
        mScoreBoard.SetActive(false);
        IsGameOver = true;
        return;
    }

    public static GameManager GameManagerSingleton => mSingleton;
    

    private void Start()
    {
        IsGameOver = true;
        SetupNewLevel();
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
        for(int i = 0; i < mGroundPieces.Length; i++)
        {
            if(!mGroundPieces[i].IsColored)
            {
                ScoreManager.ScoreManagerSingleton.UpdateScores();
                isFinished = false;
                break;
            }
        }

        if(isFinished)
        {
            if(mDifficulty < 0)
            {
                NextLevel();
            }
            else if(mDifficulty > 0)
            {
                int score = mTimer * 10;
                ScoreManager.ScoreManagerSingleton.TimeScore(score);
                NextLevel();
            }
            else
            {
                int score = mTimer * 3;
                ScoreManager.ScoreManagerSingleton.TimeScore(score);
                NextLevel();
            }
        }
        return;
    }

    private void NextLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex >= 1)
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
