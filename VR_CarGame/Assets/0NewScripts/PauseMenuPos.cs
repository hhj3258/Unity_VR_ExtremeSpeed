using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PauseMenuPos : MonoBehaviour
{
    [SerializeField] private GameObject head;

    [SerializeField] private PausePosition pausePosition;

    [Serializable]
    protected class PausePosition
    {
        public float horizon;
        public float height;
        public float distance;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = new Vector3(transform.position.x + pausePosition.horizon, transform.position.y+pausePosition.height, transform.position.z + pausePosition.distance);
        transform.position = head.transform.position + head.transform.TransformDirection(new Vector3(pausePosition.horizon, pausePosition.height, pausePosition.distance));
        transform.rotation = head.transform.rotation;
        //transform.position = new Vector3(pausePosition.horizon, pausePosition.height, pausePosition.distance);
    }
}
