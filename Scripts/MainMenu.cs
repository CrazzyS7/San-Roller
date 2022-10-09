using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI mHiScoreText;

    private readonly int mHiScore = 100;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        return;
    }

    public void QuitGame()
    {
        Application.Quit();
        return;
    }

    public void GetHiScore()
    {
        int score = (PlayerPrefs.HasKey("highScore")) ? PlayerPrefs.GetInt("highScore") : mHiScore;
        mHiScoreText.text = score.ToString();
        return;
    }
}
