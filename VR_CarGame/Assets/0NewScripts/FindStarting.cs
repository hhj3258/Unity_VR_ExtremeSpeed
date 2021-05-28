using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindStarting : MonoBehaviour
{
    private beginGame beginGame;

    public beginGame BeginGame
    {
        get { return beginGame; }
        set { beginGame = value; }
    }

    
    void Start()
    {
        beginGame = FindObjectOfType<beginGame>();
    }

}
