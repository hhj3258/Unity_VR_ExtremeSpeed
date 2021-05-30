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

    // Update is called once per frame
    void Update()
    {
        transform.position = head.transform.position + head.transform.TransformDirection(new Vector3(pausePosition.horizon, pausePosition.height, pausePosition.distance));
        transform.rotation = head.transform.rotation;
    }
}
