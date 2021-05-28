using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorReset : MonoBehaviour
{
    [SerializeField] private GameObject carobj;
    private bool isEnter = false;

    public bool IsEnter
    {
        get { return isEnter; }
        set { isEnter = value; }
    }

    private GameManager gManager;
    private void Start()
    {
        gManager = GameManager.Instance;
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("colCar"))
        {
            //Debug.Log(RayClick.gaugeTimer);
            if (RayClick.isReset || Input.GetKey(KeyCode.T))
            {
                //Debug.Log("chk");

                gManager.MyCar.transform.position = transform.position;
                gManager.MyCar.transform.rotation = transform.rotation;
                gManager.MyCar.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("colCar"))
        {
            isEnter = true;
            Debug.Log(transform.name);
        }
            
    }
}
