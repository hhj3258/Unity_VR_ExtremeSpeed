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


        //foreach (nWheelComponent w in wheels)
        //{

        //    WheelCollider col = w.collider;
        //    col.suspensionDistance = carWheels.setting.distance;
        //    JointSpring js = col.suspensionSpring;

        //    js.spring = carSetting.springs;
        //    js.damper = carSetting.dampers;
        //    col.suspensionSpring = js;


        //    col.radius = carWheels.setting.radius;

        //    col.mass = carWheels.setting.weight;


        //    WheelFrictionCurve fc = col.forwardFriction;

        //    fc.asymptoteValue = 5000.0f;
        //    fc.extremumSlip = 2.0f;
        //    fc.asymptoteSlip = 20.0f;
        //    fc.stiffness = carSetting.stiffness;
        //    col.forwardFriction = fc;
        //    fc = col.sidewaysFriction;
        //    fc.asymptoteValue = 7500.0f;
        //    fc.asymptoteSlip = 2.0f;
        //    fc.stiffness = carSetting.stiffness;
        //    col.sidewaysFriction = fc;

        //}
    }
}
