using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

public class GameManager : MonoBehaviour
{
    public GameObject mTitleScreen;

    private static GameManager mSingleton;
    private GroundPiece[] mGroundPieces;

    public void StartGame(int _difficulty)
    {
        mTitleScreen.SetActive(false);
        return;
    }

    public static GameManager GameManagerSingleton
    {
        get { return mSingleton; }
    }

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
}
