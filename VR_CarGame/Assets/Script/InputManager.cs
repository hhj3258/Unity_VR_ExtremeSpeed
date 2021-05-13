using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class InputManager : MonoBehaviour
{
    public bool throttle;
    public float steer;
    public bool is_L;
    public bool brake;

    // Update is called once per frame
    void Update()
    {
        throttle = Input.GetMouseButton(0);   // W, S
        //Debug.Log(Input.GetAxis("Vertical"));
        steer = Input.GetAxis("Horizontal");    //A, D

        is_L = Input.GetKeyDown(KeyCode.L);

        brake = Input.GetKey(KeyCode.Space);

        //Debug.Log(Input.GetAxis("XboxReset"));
        if (Input.GetKeyDown(KeyCode.R) )
        {
            SceneManager.LoadScene(0);
        }

        
    }
}
