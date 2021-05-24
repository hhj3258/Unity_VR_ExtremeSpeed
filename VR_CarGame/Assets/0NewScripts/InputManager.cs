using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class InputManager : MonoBehaviour
{
    public float accel;
    public float steer;
    public bool brake;

    public Camera cam;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }

        InputCardBoard();

        /*
        steer = Input.GetAxis("Horizontal");

        //accel = Input.GetAxis("Fire2");
        //accel = Input.GetAxis("RTrigger");
        accel = Input.GetAxis("Vertical");
        //Debug.Log(accel);
        brake = Input.GetButton("Jump");    //Space
        */
    }

    void InputCardBoard()
    {
        accel = Input.GetAxis("Fire1");
        steer = Mathf.Clamp(cam.transform.localRotation.z * -1.5f, -1f, 1f);
        //Debug.Log(cam.transform.localRotation.z);

    }

    void InputXbox()
    {

    }
}
