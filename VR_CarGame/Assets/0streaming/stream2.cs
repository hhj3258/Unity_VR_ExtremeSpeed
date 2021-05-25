using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stream2 : MonoBehaviour
{
    public GameObject pre;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("chk1");
        if (other.CompareTag("colCar"))
        {
            Debug.Log("chk2");
            var dir = Vector3.Angle(transform.forward, other.transform.position - transform.position);

            if (dir > 90f)
            {
                Debug.Log("chk3");
                Instantiate(pre);
            }
        }
    }
}
