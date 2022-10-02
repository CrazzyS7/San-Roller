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
    public TextMeshProUGUI mHiScoreText;
    public TextMeshProUGUI mScoreText;
    public TextMeshProUGUI mTimerText;
    public GameObject mCreditScreen;
    public GameObject mTitleScreen;
    public GameObject mScoreBoard;
    public Button mRestartButton;
    public Button mExitButton;

    private static GameManager mSingleton;
    private GroundPiece[] mGroundPieces;
    private bool mIsGameOver = false;
    private int mTimer = 0;
    private int mScore = 0;

    public void StartGame(int _difficulty)
    {
        mTitleScreen.SetActive(false);
        mScoreBoard.SetActive(true);
        mScore = 0;

        if (_difficulty < 0 )
        {
            UpdateScore(mScore);
        }
        else if(_difficulty > 0)
        {
            mTimer = 15;
            UpdateScore(mScore);
            StartCoroutine(TimerCounter());
        }
        else
        {
            mTimer = 30;
            UpdateScore(mScore);
            StartCoroutine(TimerCounter());
        }
        return;
    }

    public void UpdateScore(int _score)
    {
        mScore += _score;
        mScoreText.text = "SCORE: " + mScore;
        return;
    }

    private void UpdateTimer()
    {
        mTimer--;
        mTimerText.text = "Timer: " + mTimer;

        if (mTimer <= 0)
        {
            GameOver();
        }
        return;
    }

    public void GameOver()
    {
        mCreditScreen.SetActive(true);
        mIsGameOver = true;
        return;
    }

    public static GameManager GameManagerSingleton => mSingleton;
    

    private void Start()
    {
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
                isFinished = false;
                break;
            }
        }

        if(isFinished)
        {
            NextLevel();
        }
        return;
    }

    private void NextLevel()
    {
        int level = (SceneManager.GetActiveScene().buildIndex >= 1) ? 0 : SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(level);
        return;
    }

    private void OnLevelFinished(Scene _scene, LoadSceneMode _mode)
    {
        SetupNewLevel();
        return;
    }

    IEnumerator TimerCounter()
    {
        while (!mIsGameOver)
        {
            yield return new WaitForSeconds(1);
            UpdateTimer();
        }
    }
}
