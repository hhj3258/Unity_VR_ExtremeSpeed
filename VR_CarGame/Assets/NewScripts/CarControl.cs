using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 상속관계
// MonoBehaviour -> CarSettings -> CarSetValues -> CarControl
public class CarControl : CarSetValues
{

    private nWheelComponent[] wheels;   //바퀴 갯수만큼 배열 index 설정할 것
    
    private class nWheelComponent
    {
        public Transform wheel;
        public WheelCollider collider;
        public Vector3 startPos;
        public float rotation = 0f;
        public float rotation2 = 0f;
        public float maxSteer;
        public bool drive;
        public float posY = 0f;
    }

    
    // 휠 콜라이더 동적 생성 메소드
    // 매개변수의 값들은 CarSetting 스크립트에서 참조할 것
    private nWheelComponent SetWheelComponent(Transform wheel, float maxSteer, bool drive, float posY)
    {
        nWheelComponent result = new nWheelComponent();
        GameObject wheelCol = new GameObject(wheel.name + "WheelColider");  //휠 콜라이더를 붙여줄 오브젝트 동적 생성

        wheelCol.transform.parent = transform;  //휠 콜라이더를 현재 스크립트가 달려있는 오브젝트의 자식으로 생성
        wheelCol.transform.position = wheel.position;   //매개변수로 들어온 wheel의 포지션 할당
        wheelCol.transform.eulerAngles = transform.eulerAngles;   //휠 콜라이더 회전값은 차량 rotation을 할당
        posY = wheelCol.transform.localPosition.y;      

        wheelCol.AddComponent(typeof(WheelCollider));   //wheelCol 오브젝트에 휠 콜라이더 추가

        //nWheelComponent 클래스 변수 값 세팅
        result.wheel = wheel;
        result.collider = wheelCol.GetComponent<WheelCollider>();
        result.drive = drive;
        result.posY = posY;
        result.maxSteer = maxSteer;
        result.startPos = wheelCol.transform.localPosition;

        return result;
    }

    private void Awake()
    {
        if (carSetting.automaticGear) 
            neutralGear = false;

        myRigidbody = transform.GetComponent<Rigidbody>();

        //바퀴 4개 동적 생성
        wheels = new nWheelComponent[4];

        // SetWheelComponent(바퀴 오브젝트(Transform), 좌우 회전각, 구동바퀴 여부, 바퀴 오브젝트의 Y포지션 값) 
        wheels[0] = SetWheelComponent(carWheels.wheels.frontRight, carSetting.maxSteerAngle, 
            carWheels.wheels.frontWheelDrive, carWheels.wheels.frontRight.position.y);
        wheels[1] = SetWheelComponent(carWheels.wheels.frontLeft, carSetting.maxSteerAngle,
            carWheels.wheels.frontWheelDrive, carWheels.wheels.frontLeft.position.y);

        //뒷바퀴는 좌우 회전 X
        wheels[2] = SetWheelComponent(carWheels.wheels.backRight, 0f,
            carWheels.wheels.backWheelDrive, carWheels.wheels.backRight.position.y);
        wheels[3] = SetWheelComponent(carWheels.wheels.backLeft, 0f,
            carWheels.wheels.backWheelDrive, carWheels.wheels.backLeft.position.y);

        //핸들 모델이 있으면 해당 오일러앵글 값을 steerCurAngle에 저장
        if (carSetting.handle)
            steerCurAngle = carSetting.handle.localEulerAngles;


        foreach(nWheelComponent w in wheels)
        {
            WheelCollider col = w.collider;
            col.suspensionDistance = carWheels.setting.distance;
            
            //차량 스프링, 댐퍼
            JointSpring js = col.suspensionSpring;
            js.spring = carSetting.springs;
            js.damper = carSetting.dampers;
            col.suspensionSpring = js;

            col.radius = carWheels.setting.radius;
            col.mass = carWheels.setting.weight;


            WheelFrictionCurve wfc;     //휠 마찰력 그래프 설정

            wfc = col.forwardFriction;
            wfc.asymptoteValue = 5000f;
            wfc.extremumSlip = 2f;
            wfc.asymptoteSlip = 20f;
            wfc.stiffness = carSetting.stiffness;
            col.forwardFriction = wfc;

            wfc = col.sidewaysFriction;
            wfc.asymptoteValue = 7500f;
            wfc.asymptoteSlip = 2f;
            wfc.stiffness = carSetting.stiffness;
            col.sidewaysFriction = wfc;

        }
    }

    //기어 상승
    public void ShiftUp()
    {
        float now = Time.timeSinceLevelLoad;    //현재 씬 시작 후 경과 시간

        if (now < shiftDelay) return;

        //현재 기어단이 6보다 작을 때만
        if (currentGear < carSetting.gears.Length - 1)
        {
            carSounds.switchGear.GetComponent<AudioSource>().Play();

            currentGear++;

            shiftDelay = now + 1f;
            shiftTime = 1.5f;
        }
    }

    public void ShiftDown()
    {
        float now = Time.timeSinceLevelLoad;    //현재 씬 시작 후 경과 시간

        if (now < shiftDelay) return;

        if(currentGear > 0 || neutralGear)
        {
            carSounds.switchGear.GetComponent<AudioSource>().Play();

            currentGear--;

            shiftDelay = now + 0.1f;
            shiftTime = 2f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.transform.root.GetComponent<CarControl>())
        //{

        //}
    }

    private void FixedUpdate()
    {
        speed = myRigidbody.velocity.magnitude * 2.7f;

        if(speed < lastSpeed -10&& slip < 10)
        {
            slip = lastSpeed / 15f;
        }

        lastSpeed = speed;

        //if (slip2 != 0f)
        //{
        //    slip2 = Mathf.MoveTowards(slip2, 0f, 0f);
        //}

        myRigidbody.centerOfMass = carSetting.centerMass;


        //조작키 할당
        if(carWheels.wheels.frontWheelDrive || carWheels.wheels.backWheelDrive)
        {
            //steer = Mathf.MoveTowards(steer, Input.GetAxis("Horizontal"), 0.2f);
            steer = Input.GetAxis("Horizontal");
            accel = Input.GetAxis("Vertical");
            brake = Input.GetButton("Jump");
        }

        //전륜구동, 후륜구동, 사륜구동이 아니면 액셀=0
        if (!carWheels.wheels.frontWheelDrive && !carWheels.wheels.backWheelDrive)
            accel = 0.0f;

        if (carSetting.handle)
            //핸들의 z축만 A,D 인풋에 따라 회전시켜줌
            carSetting.handle.localEulerAngles = 
                new Vector3(steerCurAngle.x, steerCurAngle.y, steerCurAngle.z + (steer * -120.0f));


        if(currentGear == 1 && accel < 0f)
        {
            //기어가 1단이고 액셀이 0보다 작고 스피드가 5보다 작으면 기어다운
            if (speed < 5f) ShiftDown();
        }
        else if (currentGear == 0 && accel > 0f)
        {
            if (speed < 5f) ShiftUp();
        }
        else if (motorRPM > carSetting.shiftUpRPM && accel > 0f && speed > 10f && !brake)
        {
            ShiftUp();
        }
        else if (motorRPM < carSetting.shiftDownRPM)
        {
            ShiftDown();
        }


    }
}
