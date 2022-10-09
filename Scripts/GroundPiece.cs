using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class GroundPiece : MonoBehaviour
{
    public bool IsColored
    { get; set; }

    public void ChangeColor(Color _color)
    {
        GetComponent<MeshRenderer>().material.color = _color;
        IsColored = true;
        GameManager.GameManagerSingleton.CheckComplete();
        return;
    }
}
