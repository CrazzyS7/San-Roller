using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPiece : MonoBehaviour
{
    private bool mIsColored = false;

    public bool IsColored
    {
        get { return mIsColored; }
        set { mIsColored = value; }
    }

    public void ChangeColor(Color _color)
    {
        GetComponent<MeshRenderer>().material.color = _color;
        mIsColored = true;
        GameManager.GameManagerSingleton.CheckComplete();
        return;
    }
}
