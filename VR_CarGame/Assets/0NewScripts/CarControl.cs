using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 상속관계
// MonoBehaviour -> CarSettings -> CarSetValues -> CarControl
public class CarControl : CarSetValues
{

    [SerializeField] private InputManager im;
    private nWheelComponent[] wheels;   //바퀴 갯수만큼 배열 index 설정할 것
    private GameObject mainCam;

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
        mainCam = GameObject.FindWithTag("MainCamera");

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

    //기어 다운
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


    
    private void Update()
    {
        carSounds.idleEngine.transform.position = mainCam.transform.position;
        carSounds.lowEngine.transform.position = mainCam.transform.position;
        carSounds.highEngine.transform.position = mainCam.transform.position;
        carSounds.switchGear.transform.position = mainCam.transform.position;
    }

    [Obsolete]
    private void FixedUpdate()
    {
        //Debug.Log("speed:"+speed);
        speed = myRigidbody.velocity.magnitude * 2.7f;
        
        if(speed < lastSpeed -10&& slip < 10)
        {
            slip = lastSpeed / 15f;
        }

        lastSpeed = speed;

        myRigidbody.centerOfMass = carSetting.centerMass;


        //조작키 할당
        if(carWheels.wheels.frontWheelDrive || carWheels.wheels.backWheelDrive)
        {
            steer = im.steer;
            accel = im.accel;
            //Debug.Log(accel);
            brake = im.brake;
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


        if (speed < 1f) backward = true;

        if (currentGear == 0 && backward)
        {
            if (speed < carSetting.gears[0] * -10)
                accel = -accel;
        }
        else
            backward = false;


        wantedRPM = (5500f * accel) * 0.1f + wantedRPM * 0.9f;

        float rpm = 0f;
        int motorizedWheels = 0;
        bool floorContact = false;
        int currentWheel = 0;

        foreach(nWheelComponent w in wheels)
        {
            WheelHit hit;
            WheelCollider col = w.collider;

            if (w.drive)
            {
                if(!neutralGear && brake && currentGear < 2)
                {
                    rpm += accel * carSetting.idleRPM;
                }
                else
                {
                    if (!neutralGear)
                        rpm += col.rpm;
                    else
                        rpm += carSetting.idleRPM * accel;

                }

                motorizedWheels++;
            }


            if (brake || accel < 0f)
            {
                if( accel<0f || ( brake && (w == wheels[2] || w == wheels[3])))
                {
                    if (brake && (accel > 0f))
                        slip = Mathf.Lerp(slip, 5f, accel * 0.01f);
                    else if (speed > 1f)
                        slip = Mathf.Lerp(slip, 1f, 0.002f);
                    else
                        slip = Mathf.Lerp(slip, 1f, 0.02f);
                }

                wantedRPM = 0f;
                col.brakeTorque = carSetting.brakePower;
                w.rotation = wRotate;
            }
            else
            {
                if(accel==0 || neutralGear == true)
                    col.brakeTorque = 1000;
                else
                    col.brakeTorque = 0;


                if(speed > 0f)
                {
                    if (speed > 100)
                        slip = Mathf.Lerp(slip, 1f + Mathf.Abs(steer), 0.02f);
                    else
                        slip = Mathf.Lerp(slip, 1.5f, 0.02f);
                }
                else
                {
                    slip = Mathf.Lerp(slip, 0.01f, 0.02f);
                }

                wRotate = w.rotation;
            }



            WheelFrictionCurve wfc;

            wfc = col.forwardFriction;
            wfc.asymptoteValue = 5000.0f;
            wfc.extremumSlip = 2.0f;
            wfc.asymptoteSlip = 20.0f;
            wfc.stiffness = carSetting.stiffness / (slip + slip2);
            col.forwardFriction = wfc;

            wfc = col.sidewaysFriction;
            wfc.stiffness = carSetting.stiffness / (slip + slip2);
            wfc.extremumSlip = 0.2f + Mathf.Abs(steer);
            col.sidewaysFriction = wfc;

            if (shift && (currentGear > 1 && speed > 50.0f) && shifmotor && Mathf.Abs(steer) < 0.2f)
            {

                if (powerShift == 0) { shifmotor = false; }

                powerShift = Mathf.MoveTowards(powerShift, 0.0f, Time.fixedDeltaTime * 10.0f);



                curTorque = powerShift > 0 ? carSetting.shiftPower : carSetting.power;

            }
            else
            {

                if (powerShift > 20)
                {
                    shifmotor = true;
                }


                powerShift = Mathf.MoveTowards(powerShift, 100.0f, Time.fixedDeltaTime * 5.0f);
                curTorque = carSetting.power;

            }

            //바퀴 회전
            w.rotation = Mathf.Repeat(w.rotation + col.rpm * 360f / 60f * Time.fixedDeltaTime, 360f);
            w.rotation2 = Mathf.Lerp(w.rotation2, col.steerAngle, 0.1f);
            //x=앞뒤,y=좌우
            w.wheel.localRotation = Quaternion.Euler(w.rotation, w.rotation2, 0f);


            Vector3 wlp = w.wheel.localPosition;    //wheel local position

            //바닥 태그에 따른 브레이크 사운드와 파티클 설정
            if(col.GetGroundHit(out hit))
            {
                if (carParticles.brakeParticlePrefab)
                {
                    if (Particle[currentWheel] == null)
                    {
                        Particle[currentWheel] = Instantiate(carParticles.brakeParticlePrefab, w.wheel.position, Quaternion.identity) as GameObject;
                        Particle[currentWheel].name = "WheelParticle";
                        Particle[currentWheel].transform.parent = transform;

                        Particle[currentWheel].AddComponent<AudioSource>();
                        Particle[currentWheel].GetComponent<AudioSource>().maxDistance = 50;
                        Particle[currentWheel].GetComponent<AudioSource>().spatialBlend = 1;
                        Particle[currentWheel].GetComponent<AudioSource>().dopplerLevel = 5;
                        Particle[currentWheel].GetComponent<AudioSource>().rolloffMode = AudioRolloffMode.Custom;
                    }

                    var pc = Particle[currentWheel].GetComponent<ParticleSystem>();
                    bool wGrounded = false;

                    for(int i = 0; i < carSetting.hitGround.Length; i++)
                    {
                        if (hit.collider.CompareTag(carSetting.hitGround[i].tag))
                        {
                            wGrounded = carSetting.hitGround[i].grounded;

                            if((brake||Mathf.Abs(hit.sidewaysSlip) > 0.5f) && speed > 1)
                            {
                                Particle[currentWheel].GetComponent<AudioSource>().clip = carSetting.hitGround[i].brakeSound;
                            }
                            else if(Particle[currentWheel].GetComponent<AudioSource>().clip != 
                                carSetting.hitGround[i].groundSound && !Particle[currentWheel].GetComponent<AudioSource>().isPlaying)
                            {
                                Particle[currentWheel].GetComponent<AudioSource>().clip = carSetting.hitGround[i].groundSound;
                            }

                            Particle[currentWheel].GetComponent<ParticleSystem>().startColor = carSetting.hitGround[i].brakeColor;
                        }
                    }


                    if(wGrounded && speed > 5 && !brake)
                    {
                        pc.enableEmission = true;

                        Particle[currentWheel].GetComponent<AudioSource>().volume = 0.5f;

                        if (!Particle[currentWheel].GetComponent<AudioSource>().isPlaying)
                            Particle[currentWheel].GetComponent<AudioSource>().Play();

                    }
                    else if ((brake || Mathf.Abs(hit.sidewaysSlip) > 0.6f) && speed > 1)
                    {
                        if ((accel < 0f) || ((brake || Mathf.Abs(hit.sidewaysSlip) > 0.6f) && (w == wheels[2] || w == wheels[3])))
                        {
                            Particle[currentWheel].GetComponent<AudioSource>().volume = 10f;

                            if (!Particle[currentWheel].GetComponent<AudioSource>().isPlaying)
                                Particle[currentWheel].GetComponent<AudioSource>().Play();

                            pc.enableEmission = true;
                            Particle[currentWheel].GetComponent<AudioSource>().volume = 10;
                        }
                    }
                    else
                    {
                        pc.enableEmission = false;

                        Particle[currentWheel].GetComponent<AudioSource>().volume = 
                            Mathf.Lerp(Particle[currentWheel].GetComponent<AudioSource>().volume, 0, Time.fixedDeltaTime * 10.0f);
                    }
                }

                //Vector3.Dot(A, B) = Vector3.Magnitude(A) * Vector3.Magnitude(B) * Mathf.Cos(θ);

                wlp.y -= Vector3.Dot(w.wheel.position - hit.point, transform.TransformDirection(0, 1, 0) / transform.lossyScale.x) - (col.radius);
                wlp.y = Mathf.Clamp(wlp.y, -10f, w.posY);
                floorContact = floorContact || w.drive;
            }   // end if(col.GetGroundHit(out hit))
            else
            {
                if (Particle[currentWheel] != null)
                {
                    var pc = Particle[currentWheel].GetComponent<ParticleSystem>();
                    pc.enableEmission = false;
                }

                wlp.y = w.startPos.y - carWheels.setting.distance;

                myRigidbody.AddForce(Vector3.down * 5000);

            }

            currentWheel++;
            w.wheel.localPosition = wlp;    
        } // end foreach


        //구동 휠 갯수만큼 rpm을 나눠서 적용시켜줌
        if(motorizedWheels > 1)
        {
            rpm = rpm / motorizedWheels;
        }

        motorRPM = 0.95f * motorRPM + 0.05f * Mathf.Abs(rpm * carSetting.gears[currentGear]);
        if (motorRPM > 5500f) motorRPM = 5200f;

        int index = (int)(motorRPM / efficiencyTableStep);
        if (index >= efficiencyTable.Length)
            index = efficiencyTable.Length - 1;
        if (index < 0) index = 0;


        float newTorque = curTorque * carSetting.gears[currentGear] * efficiencyTable[index];

        foreach(nWheelComponent w in wheels)
        {
            WheelCollider col = w.collider;

            if (w.drive)
            {
                if (Mathf.Abs(col.rpm) > Mathf.Abs(wantedRPM))
                {
                    col.motorTorque = 0;
                }
                else
                {
                    float curTorqueCol = col.motorTorque;

                    if (!brake && accel != 0 && neutralGear == false)
                    {
                        if ((speed < carSetting.limitForwardSpeed && currentGear > 0) || (speed < carSetting.limitBackwardSpeed && currentGear == 0))
                        {
                            col.motorTorque = curTorqueCol * 0.9f + newTorque * 1f;
                        }
                        else
                        {
                            col.motorTorque = 0;
                            col.brakeTorque = 2000f;
                        }
                    }
                    else
                    {
                        col.motorTorque = 0;
                    }
                }
            }

            if (brake || slip2 > 2.0f)
            {
                col.steerAngle = Mathf.Lerp(col.steerAngle, steer * w.maxSteer, 0.02f);
            }
            else
            {
                
                float SteerAngle = Mathf.Clamp(speed / carSetting.maxSteerAngle, 1f, carSetting.maxSteerAngle);
                col.steerAngle = steer * (w.maxSteer / SteerAngle);
            }

        }



        pitch = Mathf.Clamp(1.2f + ( (motorRPM - carSetting.idleRPM) / (carSetting.shiftUpRPM - carSetting.idleRPM) ), 1.0f, 10.0f);
        //Debug.Log(1.2f + ((motorRPM - carSetting.idleRPM) / (carSetting.shiftUpRPM - carSetting.idleRPM)));
        shiftTime = Mathf.MoveTowards(shiftTime, 0.0f, 0.1f);

        if (pitch == 1)
        {
            carSounds.idleEngine.volume = Mathf.Lerp(carSounds.idleEngine.volume, 1.0f, 0.1f);
            carSounds.lowEngine.volume = Mathf.Lerp(carSounds.lowEngine.volume, 0.5f, 0.1f);
            carSounds.highEngine.volume = Mathf.Lerp(carSounds.highEngine.volume, 0.0f, 0.1f);

        }
        else
        {

            carSounds.idleEngine.volume = Mathf.Lerp(carSounds.idleEngine.volume, 1.8f - pitch, 0.1f);


            if ((pitch > pitchDelay || accel > 0) && shiftTime == 0.0f)
            {
                carSounds.lowEngine.volume = Mathf.Lerp(carSounds.lowEngine.volume, 0.0f, 0.2f);
                carSounds.highEngine.volume = Mathf.Lerp(carSounds.highEngine.volume, 1.0f, 0.1f);
            }
            else
            {
                carSounds.lowEngine.volume = Mathf.Lerp(carSounds.lowEngine.volume, 0.5f, 0.1f);
                carSounds.highEngine.volume = Mathf.Lerp(carSounds.highEngine.volume, 0.0f, 0.2f);
            }

            carSounds.highEngine.pitch = pitch;
            carSounds.lowEngine.pitch = pitch;

            pitchDelay = pitch;
            //Debug.Log("pitchDelay" + pitchDelay);
        }

    }
}
