using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerChk : MonoBehaviour
{
    private bool isEnter = false;

    public bool IsEnter
    {
        get { return isEnter; }
        set { isEnter = value; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("colCar") && !isEnter)
        {
            isEnter = true;
            Debug.Log(transform.name);
        }
    }
}
