using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI speedText;

    public GameObject needle;
    private GameObject mycar;
    float thisAngle = 0f;
    CarControl carScript;
    // Start is called before the first frame update
    void Start()
    {
        mycar = GameObject.FindWithTag("Car");
        carScript = mycar.GetComponent<CarControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!mycar) mycar = GameObject.FindWithTag("Car");
        Debug.Log(mycar);
        //Debug.Log(carScript.MotorRPM);
        //thisAngle = (carScript.MotorRPM / 20) - 175;
        //thisAngle = Mathf.Clamp(thisAngle, -180, 90);

        speedText.text = ((int)carScript.Speed).ToString();


        thisAngle = (carScript.MotorRPM / 25);
        //Debug.Log(thisAngle);
        thisAngle = Mathf.Clamp(thisAngle, 0, 260);
        needle.transform.localRotation = Quaternion.Euler(0, 0, -thisAngle);


    }
}
