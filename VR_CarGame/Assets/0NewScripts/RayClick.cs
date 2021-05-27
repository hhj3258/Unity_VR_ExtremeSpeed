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
    [SerializeField] private Canvas menus;
    [SerializeField] private Canvas pauseMenu;
    [SerializeField] private GameObject pausePanel;

    private float gaugeTimer = 0.0f;
    private float gazeTime = 1f;

    RaycastHit hit;

    void Update()
    {

        
        Vector3 forward = mainCam.transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(this.transform.position, forward, Color.green);
        cursorGaugeImage.fillAmount = gaugeTimer;


        int layerMask = 1 << LayerMask.NameToLayer("UI");
        if (Physics.Raycast(this.transform.position, forward, out hit, 10f, layerMask))
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.name.Equals("btnMusic"))
            {
                gaugeTimer += 1.0f / gazeTime * Time.deltaTime;
            }

            if (gaugeTimer >= 1.0f)
            {
                dashBoard.SongChanger();
                gaugeTimer = 0.0f;
            }


        }
        else if (Physics.Raycast(this.transform.position, forward, out hit, 100f))
        {
            Debug.Log(hit.transform.name);
            Debug.Log(Time.deltaTime);
            if (hit.transform.name.Equals("btnHome") || 
                hit.transform.name.Equals("btnCamera") || 
                hit.transform.name.Equals("btnPause") ||
                hit.transform.name.Equals("btnRESUME") ||
                hit.transform.name.Equals("btnLOBBY"))
            {
                gaugeTimer += 1.0f / gazeTime * Time.deltaTime;
            }


            if (gaugeTimer >= 1f)
            {
                if (hit.transform.name.Equals("btnHome"))
                    SceneManager.LoadScene(0);

                if (hit.transform.name.Equals("btnCamera"))
                {
                    CameraManager.camMode = CameraManager.camMode ? false : true;
                }

                if (hit.transform.name.Equals("btnPause"))
                {
                    pauseMenu.transform.position = new Vector3(0f, 1.8f, 1.5f);
                    Time.timeScale = 0;
                    pauseMenu.gameObject.SetActive(true);
                    pausePanel.gameObject.SetActive(true);
                }

                if (hit.transform.name.Equals("btnRESUME"))
                {
                    Time.timeScale = 1;
                    pauseMenu.gameObject.SetActive(false);
                    pausePanel.gameObject.SetActive(false);
                }

                if (hit.transform.name.Equals("btnLOBBY"))
                {
                    SceneManager.LoadScene(0);
                }

                gaugeTimer = 0f;

            }

        }
        else
            gaugeTimer = 0f;
    }
}
