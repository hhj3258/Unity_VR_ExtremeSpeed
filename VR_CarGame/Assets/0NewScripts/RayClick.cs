using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class RayClick : MonoBehaviour
{
    public Image cursorGaugeImage;
    public GameObject mainCam;
    private float gaugeTimer = 0.0f;
    private float gazeTime = 0.5f;
    public DashBoard dashBoard;

    RaycastHit hit;

    public Canvas menus;

    void Update()
    {

        
        Vector3 forward = mainCam.transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(this.transform.position, forward, Color.green);
        cursorGaugeImage.fillAmount = gaugeTimer;


        int layerMask = 1 << LayerMask.NameToLayer("UI");
        //if (Physics.Raycast(this.transform.position, forward, out hit, 1f, layerMask))
        //{
        //    Debug.Log(hit.transform.name);
        //    if (hit.transform.name.Equals("Button"))
        //    {
        //        gaugeTimer += 1.0f / gazeTime * Time.deltaTime;
        //    }

        //    if (gaugeTimer >= 1.0f)
        //    {
        //        dashBoard.SongChanger();
        //        gaugeTimer = 0.0f;

        //    }
        //}


        if(Physics.Raycast(this.transform.position, forward, out hit, 100f))
        {
            //Debug.Log(hit.transform.name);
            if (hit.transform.name.Equals("btnHome"))
            {
                gaugeTimer += 1.0f / gazeTime * Time.deltaTime;
            }

            if (hit.transform.name.Equals("btnCamera"))
            {
                gaugeTimer += 1.0f / gazeTime * Time.deltaTime;
                
            }

            if (gaugeTimer >= 1f)
            {
                if (hit.transform.name.Equals("btnHome"))
                    SceneManager.LoadScene(0);

                if (hit.transform.name.Equals("btnCamera"))
                {
                    CameraManager.camMode = false;
                    menus.gameObject.SetActive(false);
                }
                    

                gaugeTimer = 0f;
                
            }
                
        }
    }
}
