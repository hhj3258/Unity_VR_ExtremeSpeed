using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(LightingManager))]
public class CarController : MonoBehaviour
{
    // 조작키 매니저, 차량 라이트 매니저, 디스플레이 UI 매니저
    public InputManager im;
    public LightingManager lm;
    public UIManager uim;
    
    public List<WheelCollider> throttle_Wheels; // 모든 휠
    public List<GameObject> steering_Wheels;    // 전륜 휠
    public List<GameObject> meshes;             // 휠 메쉬
    public float strength_Coefficient;          //차량의 출력(힘)
    public float maxTurn;                 //휠의 좌우 각
    public Transform CM;                        //Center Mass: 차량 무게 중심
    public Rigidbody rb;
    public float brakeStrength;                 //브레이크 힘
    public List<GameObject> tailLights;

    [SerializeField] private GameObject maincam;
    
    
    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    // Start is called before the first frame update
    void Start()
    {
        im = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody>();
        //uim = GetComponent<UIManager>();
        if (CM)
        {
            rb.centerOfMass = CM.localPosition;
        }
    }

    void Update()
    {
        if (im.is_L)
        {
            lm.ToggleHeadLights();
        }

        foreach (GameObject tl in tailLights)
        {
            tl.GetComponent<Renderer>().material.
                SetColor("_EmissionColor", im.brake ? new Color(0.6f, 0.1f, 0.1f) : Color.black);
            //Debug.Log("chk");
        }

        uim.changeText(transform.InverseTransformVector(rb.velocity).z);

        
    }

    // 유니티 엔진에서의 물리력에 접하는 기능을 구현할 시에는
    // FixedUpdate() 상에서 구현해야 정확한 결과 값을 얻을 수 있음
    void FixedUpdate()
    {
        // 브레이크 및 구동 로직
        foreach (WheelCollider wheel in throttle_Wheels)
        {
            //Debug.Log(wheel.motorTorque);
            
            if (im.brake)    
            {
                wheel.motorTorque = 0f;
                wheel.brakeTorque = brakeStrength * Time.deltaTime;
            }
            else
            {
                
                wheel.motorTorque = strength_Coefficient * Time.deltaTime * Convert.ToInt32(im.throttle);
                wheel.brakeTorque = 0f;
            }
        }


        // 전륜 휠을 통해 좌우 버튼을 누를 시 각도 조절
        foreach (GameObject wheel in steering_Wheels)
        {
            Debug.Log("maincam.transform.rotation.z: "+maincam.transform.rotation.z);
            Debug.Log("im.steer: "+im.steer);
            //wheel.GetComponent<WheelCollider>().steerAngle = maxTurn * im.steer;  //각도 * 인풋bool
            wheel.GetComponent<WheelCollider>().steerAngle = maxTurn * (maincam.transform.localRotation.z * -1f);  //각도 * 인풋bool
            
            wheel.transform.localEulerAngles = new Vector3(0f, maxTurn * maincam.transform.localRotation.z * -1f, 0f);
        }
        
        //차량 속도에 따른 바퀴의 회전
        foreach (GameObject mesh in meshes)
        {
            mesh.transform.Rotate(rb.velocity.magnitude * 
                                  (transform.InverseTransformDirection(rb.velocity).z >= 0 ? 1 : -1)
                                  / (2 * Mathf.PI * 0.33f), 0f, 0f );
            // Debug.Log(rb.velocity.magnitude * 
            //           (transform.InverseTransformDirection(rb.velocity).z));
        }
    }
}
