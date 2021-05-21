using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class InputManager : MonoBehaviour
{
    public float accel;
    public float steer;
    public bool brake;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) )
        {
            SceneManager.LoadScene(0);
        }

        steer = Input.GetAxis("Horizontal");

        //accel = Input.GetAxis("Fire2");
        //accel = Input.GetAxis("RTrigger");
        accel = Input.GetAxis("Vertical");
        //Debug.Log(accel);
        brake = Input.GetButton("Jump");    //Space
    }
}
