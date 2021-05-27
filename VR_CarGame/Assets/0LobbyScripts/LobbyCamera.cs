using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCamera : MonoBehaviour
{
    public float x;
    public float y;
    public float z;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(x, y, z);
    }

}
