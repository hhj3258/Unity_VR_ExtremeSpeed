using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSetValues : CarSettings
{
    
    protected float steer = 0f;     //A,D
    protected float accel = 0f;     //W,S

    protected bool brake;   //SPACE

    protected bool shifmotor;

    protected float curTorque = 100f;
    protected float powerShift = 100;
    protected bool shift;

    protected float torque = 100f;

    protected float speed = 0f;

    protected float lastSpeed = -10f;

    protected bool shifting = false;

    protected float[] efficiencyTable 
        = { 0.6f, 0.65f, 0.7f, 0.75f, 0.8f, 0.85f, 
            0.9f, 1.0f, 1.0f, 0.95f, 0.80f, 0.70f, 
            0.60f, 0.5f, 0.45f, 0.40f, 0.36f, 0.33f, 
            0.30f, 0.20f, 0.10f, 0.05f };

    protected float efficiencyTableStep = 250f;

    protected float pitch;
    protected float pitchDelay;

    protected float shiftTime = 0f;

    protected float shiftDelay = 0f;

    protected int currentGear = 0;
    protected bool neutralGear = true;
    protected float motorRPM = 0f;
    protected bool backward=false;

    ///////////////////////////////////////////////////////////////////

    protected float wantedRPM = 0f;
    protected float wRotate;
    protected float slip, slip2 = 0f;

    protected GameObject[] Particle = new GameObject[4];

    protected Vector3 steerCurAngle;  //현재 핸들 각도

    protected Rigidbody myRigidbody;
}
