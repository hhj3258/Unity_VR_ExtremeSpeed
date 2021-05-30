using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour
{
    private GameManager gManager;

    public Image cursorGaugeImage;
    public GameObject mainCam;
    private float gaugeTimer = 0.0f;
    private float gazeTime = 2.0f;

    public GameObject[] cars;

    float ySpeed = 0;
    public static int cnt;

    public Canvas carUI;
    public Canvas mapUI;
    public Canvas panels;
    public Canvas colorUI;

    private bool isMap=false;
    private string trackName;

    private bool isColor = false;

    public static Color selectedColor;
    public static int inputMenu;

    public Image btnXbox;
    public Image btnCardBoard;

    public static bool controller = true;

    [SerializeField] private Material[] carMaterial;

    private void Start()
    {
        gManager = GameManager.Instance;
        
        Time.timeScale = 1;
        cnt = 0;
        trackName = "Drift Track";
        selectedColor = Color.white;
        gManager.carMaterial[0].color = selectedColor;
        inputMenu = 0;

        if (!controller)
        {
            btnXbox.gameObject.SetActive(true);
            btnCardBoard.gameObject.SetActive(false);
        }
    }


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
                }

                if (hitName.Equals("btnLeft"))
                {
                    cars[cnt].SetActive(false);
                    cnt--;
                    if (cnt < 0) cnt = cars.Length - 1;
                    ySpeed = 0f;
                    cars[cnt].transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                    cars[cnt].SetActive(true);
                }

                if (hitName.Equals("btnPlay"))
                {
                    SceneManager.LoadScene(trackName);
                }

                if (hitName.Equals("btnMap"))
                {
                    carUI.gameObject.SetActive(false);
                    panels.gameObject.SetActive(true);
                    mapUI.gameObject.SetActive(true);

                    isMap = isMap ? false : true;
                }

                if (isMap && hit.transform.CompareTag("imgTrack"))
                {
                    trackName = hitName;

                    carUI.gameObject.SetActive(true);
                    panels.gameObject.SetActive(false);
                    mapUI.gameObject.SetActive(false);

                    isMap = false;
                }

                if (hitName.Equals("btnColor"))
                {
                    carUI.gameObject.SetActive(false);
                    colorUI.gameObject.SetActive(true);
                    isColor = isColor ? false : true;
                }

                if (isColor && hit.transform.CompareTag("imgColor"))
                {
                    selectedColor = hit.transform.GetComponent<Image>().material.color;

                    switch (cnt)
                    {
                        case 0:
                            carMaterial[0].color = selectedColor;
                            break;
                        case 1:
                            carMaterial[1].color = selectedColor;
                            break;
                        case 2:
                            carMaterial[2].color = selectedColor;
                            break;
                        case 3:
                            carMaterial[3].color = selectedColor;
                            break;
                    }
                }

                if(isColor&& hitName.Equals("btnOk"))
                {
                    carUI.gameObject.SetActive(true);
                    colorUI.gameObject.SetActive(false);

                    isColor = false;
                }

                if (hitName.Equals("btnCardBoard"))
                {
                    inputMenu = 1;
                    btnCardBoard.gameObject.SetActive(false);
                    btnXbox.gameObject.SetActive(true);

                    controller = false;
                }
                if (hitName.Equals("btnXbox"))
                {
                    inputMenu = 0;
                    btnCardBoard.gameObject.SetActive(true);
                    btnXbox.gameObject.SetActive(false);

                    controller = true;
                }
            }

            if (gaugeTimer >= 1.0f)
                gaugeTimer = 0.0f;
        }
        else
            gaugeTimer = 0f;

    }


}
