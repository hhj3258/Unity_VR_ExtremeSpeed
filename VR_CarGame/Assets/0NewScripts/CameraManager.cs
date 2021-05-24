using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private GameObject focus;

    [SerializeField]
    protected OutterCamera outterCamera;

    [Serializable]
    protected class OutterCamera
    {
        public float distance;
        public float height;
        public float dampening;
    }

    [SerializeField]
    protected InnerCamera innerCamera;

    [Serializable]
    protected class InnerCamera
    {
        public float h2;
        public float d2;
        public float l;
    }

    bool camMode = true;

    void Start()
    {
        focus = GameObject.FindWithTag("Car");
    }

    private void Update()
    {
        if(!focus)
            focus = GameObject.FindWithTag("Car");

        if (focus)
        {
            if (Input.GetButtonDown("Fire3") || Input.GetKeyDown(KeyCode.C))
            {
                camMode = camMode ? false : true;
            }
            if (camMode)
            {
                transform.position = Vector3.Lerp(
                    transform.position,
                    focus.transform.position + focus.transform.TransformDirection(new Vector3(0f, outterCamera.height, -outterCamera.distance)),
                    outterCamera.dampening * Time.fixedDeltaTime);
                //this.transform.position = Vector3.Lerp(transform.position, transform.position, Time.deltaTime * dampening);
                transform.LookAt(focus.transform);
                //Camera.main.fieldOfView = 60f;
            }
            
            if (!camMode)
            {
                transform.position = focus.transform.position + focus.transform.TransformDirection(new Vector3(innerCamera.l, innerCamera.h2, -innerCamera.d2));

                transform.rotation = focus.transform.rotation;
                //Camera.main.fieldOfView = 80f;
            }

            
        }

    }

}
