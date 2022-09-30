using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyButtonX : MonoBehaviour
{
    private GameManager mGameManagerX;
    private Button mStartButton;

    public int mDifficulty;

    // Start is called before the first frame update
    void Start()
    {
        mGameManagerX = GameObject.Find("GameManager").GetComponent<GameManager>();
        mStartButton = GetComponent<Button>();
        mStartButton.onClick.AddListener(SetDifficulty);
        return;
    }

    /* When a button is clicked, call the StartGame() method
     * and pass it the difficulty value (1, 2, 3) from the button 
    */
    public void SetDifficulty()
    {
        mGameManagerX.StartGame(mDifficulty);
        return;
    }
}
