using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyButton : MonoBehaviour
{
    private GameManager mGameManager;
    private Button mStartButton;

    public int mDifficulty;

    // Start is called before the first frame update
    void Start()
    {
        mGameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        mStartButton = GetComponent<Button>();
        mStartButton.onClick.AddListener(SetDifficulty);
        return;
    }

    /* When a button is clicked, call the StartGame() method
     * and pass it the difficulty value (-1, 0, 1) from the button 
    */
    public void SetDifficulty()
    {
        mGameManager.IsGameOver = false;
        mGameManager.StartGame(mDifficulty);
        return;
    }
}
