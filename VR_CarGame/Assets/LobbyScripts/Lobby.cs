using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour
{
    public Image cursorGaugeImage;
    public GameObject mainCam;
    private float gaugeTimer = 0.0f;
    private float gazeTime = 2.0f;

    public GameObject[] cars;

    float ySpeed = 0;
    public static int cnt;
    public static string carName;


    private void Start()
    {
        //Instantiate(cars[0],transform.position,Quaternion.identity);
        cnt = 0;
    }
    // Update is called once per frame
    void Update()
    {
        ySpeed += 15f * Time.deltaTime;
        cars[cnt].transform.localRotation = Quaternion.Euler(new Vector3(0f, ySpeed, 0f));

        RaycastHit hit;
        Vector3 forward = mainCam.transform.TransformDirection(Vector3.forward);
        cursorGaugeImage.fillAmount = gaugeTimer;

        int layerMask = 1 << LayerMask.NameToLayer("UI");
        //Debug.Log("layerMask: "+layerMask);   //32
        //Debug.Log(LayerMask.NameToLayer("UI")); //5
        if (Physics.Raycast(this.transform.position, forward, out hit, 100f, layerMask))
        {
            //Debug.Log(hit.transform.name);
            gaugeTimer += 1.0f / gazeTime * Time.deltaTime;
            var hitName = hit.transform.name;
            if (gaugeTimer >= 1f)
            {
                if (hitName.Equals("btnRight"))
                {
                    //지금 보이는 차량을 off
                    cars[cnt].SetActive(false);
                    cnt++;
                    //cnt가 cars 최대 길이와 같다면 0으로 초기화
                    if (cnt == cars.Length) cnt = 0;
                    //0번째 차량을 active
                    ySpeed = 0f;
                    cars[cnt].transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                    cars[cnt].SetActive(true);
                    carName = cars[cnt].name;
                }

                if (hitName.Equals("btnLeft"))
                {
                    cars[cnt].SetActive(false);
                    cnt--;
                    if (cnt < 0) cnt = cars.Length - 1;
                    ySpeed = 0f;
                    cars[cnt].transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                    cars[cnt].SetActive(true);
                    carName = cars[cnt].name;
                }

                if (hitName.Equals("btnPlay"))
                {

                    SceneManager.LoadScene("SampleScene");
                }
            }

            if (gaugeTimer >= 1.0f)
                gaugeTimer = 0.0f;
        }
        else
            gaugeTimer = 0f;

    }
}
