using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class RayClick : MonoBehaviour
{
    public Image cursorGaugeImage;
    public GameObject mainCam;
    private float gaugeTimer = 0.0f;
    private float gazeTime = 1.0f;
    public DashBoard dashBoard;

    void Update()
    {

        RaycastHit hit;
        Vector3 forward = mainCam.transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(this.transform.position, forward, Color.green);
        cursorGaugeImage.fillAmount = gaugeTimer;



        if (Physics.Raycast(this.transform.position, forward, out hit, 5))
        {
            if (hit.transform.name.Equals("Button"))
            {
                gaugeTimer += 1.0f / gazeTime * Time.deltaTime;
                //Debug.Log("chk");
            }
            //Debug.Log(hit.transform.name);
            if (gaugeTimer >= 1.0f)
            {
                //hit_now_obj = hit.transform.gameObject;
                //Debug.Log("Hit!");

                dashBoard.SongChanger();
                gaugeTimer = 0.0f;

            }
        }
        else
            gaugeTimer = 0.0f;
    }
}
