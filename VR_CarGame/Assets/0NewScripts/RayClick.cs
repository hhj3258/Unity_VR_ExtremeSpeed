using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class RayClick : MonoBehaviour
{
    [SerializeField] private Image cursorGaugeImage;
    [SerializeField] private GameObject mainCam;
    [SerializeField] private DashBoard dashBoard;
    [SerializeField] private Canvas[] pauseMenu;

    [SerializeField] private GameObject pausePanel;

    public static float gaugeTimer = 0.0f;
    private float gazeTime = 1f;

    public static bool isReset = false;

    RaycastHit hit;

    void Update()
    {


        Vector3 forward = mainCam.transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(this.transform.position, forward, Color.green);
        cursorGaugeImage.fillAmount = gaugeTimer;


        int layerMask = 1 << LayerMask.NameToLayer("UI");
        if (Physics.Raycast(this.transform.position, forward, out hit, 10f, layerMask))
        {
            Debug.Log(hit.collider.transform.name);
            if (hit.collider.transform.name.Equals("btnMusic"))
            {
                gaugeTimer += 1.0f / gazeTime * Time.unscaledDeltaTime;

                if (gaugeTimer >= 1.0f)
                {
                    dashBoard.SongChanger();
                    gaugeTimer = 0.0f;
                }
            }

            if (hit.collider.transform.name.Equals("btnHome") ||
                hit.collider.transform.name.Equals("btnCamera") ||
                hit.collider.transform.name.Equals("btnPause") ||
                hit.collider.transform.name.Equals("btnRESUME") ||
                hit.collider.transform.name.Equals("btnLOBBY")
                )
            {
                gaugeTimer += 1.0f / gazeTime * Time.unscaledDeltaTime;
            }

            if (hit.collider.transform.name.Equals("btnReset"))
            {
                gaugeTimer += 1.0f / gazeTime * Time.unscaledDeltaTime;
            }


            if (gaugeTimer >= 1f)
            {

                if (hit.collider.transform.name.Equals("btnCamera"))
                {
                    CameraManager.camMode = CameraManager.camMode ? false : true;
                }

                if (hit.collider.transform.name.Equals("btnPause"))
                {
                    Time.timeScale = 0;
                    pauseMenu[0].gameObject.SetActive(true);
                    pauseMenu[1].gameObject.SetActive(true);
                    pausePanel.gameObject.SetActive(true);
                }

                if (hit.collider.transform.name.Equals("btnRESUME"))
                {
                    Time.timeScale = 1;
                    pauseMenu[0].gameObject.SetActive(false);
                    pauseMenu[1].gameObject.SetActive(false);
                    pausePanel.gameObject.SetActive(false);
                }

                if (hit.collider.transform.name.Equals("btnLOBBY"))
                {
                    SceneManager.LoadScene(0);
                }

                if (hit.collider.transform.name.Equals("btnReset"))
                {
                    isReset = true;
                }


                gaugeTimer = 0f;
                
            }
        }
        else
        {
            isReset = false;
            gaugeTimer = 0f;
        }
            
    }//void Update()

}
