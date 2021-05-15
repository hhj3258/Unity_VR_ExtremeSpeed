using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CarSetting : MonoBehaviour
{
    // 휠 세팅 //////////////////////////////////////////////////////////////////
    
    [SerializeField]
    private nCarWheels carWheels;
    
    [Serializable]
    protected class nCarWheels
    {
        public nConnectWheel wheels;
        public nWheelSetting setting;
    }
    
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
    private nCarSounds carSounds;
    
    [Serializable]
    protected class nCarSounds
    {
        public AudioSource idleEngine, lowEngine, highEngine;

        public AudioSource switchGear;
    }
    
    // 엔진 세팅 //////////////////////////////////////////////////////////////

    protected class nCarSetting
    {
        public Transform handle;    // 핸들 
    }
}
