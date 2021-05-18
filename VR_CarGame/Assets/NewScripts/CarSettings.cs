using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CarSettings : MonoBehaviour
{


    // 휠 세팅 //////////////////////////////////////////////////////////////////

    [SerializeField]
    protected nCarWheels carWheels;
    
    [Serializable]
    protected class nCarWheels
    {
        public nConnectWheel wheels;
        public nWheelSetting setting;
    }
    
    //인스펙터에서 차량의 휠 Transform 설정
    [Serializable]
    protected class nConnectWheel
    {
        
        public bool frontWheelDrive = true;     //전륜구동인가
        public Transform frontRight;
        public Transform frontLeft;

        public bool backWheelDrive = true;      //후륜구동인가
        public Transform backRight;
        public Transform backLeft;
    }
    
    [Serializable]
    protected class nWheelSetting
    {
        public float radius = 0.35f;    //차량 휠 반지름
        public float weight = 1000f;   //차량 휠 무게
        public float distance = 0.2f;  //차량과 휠 사이 거리
    }
    
    // 사운드 세팅 /////////////////////////////////////////////////////////////

    [SerializeField]
    protected nCarSounds carSounds;
    
    [Serializable]
    protected class nCarSounds
    {
        public AudioSource idleEngine, lowEngine, highEngine;
        public AudioSource switchGear;
    }

    // 파티클 세팅 ////////////////////////////////////////////////////////////

    [SerializeField]
    protected CarParticles carParticles;

    [Serializable]
    protected class CarParticles
    {
        public GameObject brakeParticlePrefab;
        //public ParticleSystem siftParticle1, shiftParticle2;
        private GameObject[] wheelParticle = new GameObject[4];
    }


    // 엔진 세팅 //////////////////////////////////////////////////////////////

    [SerializeField]
    protected nCarSetting carSetting;

    [Serializable]
    protected class nCarSetting
    {
        public Transform handle;    // 핸들 
        public nHitGround[] hitGround;  //여러가지 지형에 대응

        //public List<Transform> camViews;

        public float springs = 25000f;  //차량 스프링 계수
        public float dampers = 1500f;   //댐퍼계수: 스프링 신축성 조절

        public float power = 120f;      //마력
        public float shiftPower = 150f;
        public float brakePower = 150f; //브레이크 힘

        public Vector3 centerMass = new Vector3(0f, -0.05f, 0f);    //차량 무게 중심

        public float maxSteerAngle = 30f;   //휠 좌우각

        //자동 변속 변수 설정
        public float shiftDownRPM = 2000f;
        public float shiftUpRPM = 4000f;
        public float idleRPM = 500f;

        public float stiffness = 2f;    //차량 마찰력 변수

        public bool automaticGear = true;

        public float[] gears = {-10f, 9f, 6f, 4.5f, 3f, 2.5f};

        public float limitBackwardSpeed = 60f;  //최고 후진 속도
        public float limitForwardSpeed = 220f;  //최고 전진 속도

    }
    
    // 바닥 종류에 따라 값들을 설정 ////////////////////////////////////////////////
    [Serializable]
    protected class nHitGround
    {
        public string tag = "street";
        public bool grounded = false;
        public AudioClip brakeSound;
        public AudioClip groundSound;
        public Color brakeColor;
    }
    
    
}
