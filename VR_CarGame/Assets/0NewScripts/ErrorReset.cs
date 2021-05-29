using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorReset : MonoBehaviour
{

    private bool isEnter = false;

    private Vector3 myPos;
    private Quaternion myRot;


    [SerializeField] private FindStarting findStarting;


    public bool IsEnter
    {
        get { return isEnter; }
        set { isEnter = value; }
    }

    public Vector3 MyPos
    {
        get { return myPos; }
        set { myPos = value; }
    }

    public Quaternion MyRot
    {
        get { return myRot; }
        set { myRot = value; }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("colCar"))
        {
            if (!isEnter)
            {
                isEnter = true;

                if(!findStarting.BeginGame)
                    findStarting.BeginGame= FindObjectOfType<beginGame>();


                findStarting.BeginGame.SectionList.Add(transform.GetComponent<ErrorReset>());
                //Debug.Log("BeginGame.SectionList.Count: " + findStarting.BeginGame.SectionList.Count);

                myPos = transform.position;
                myRot = transform.rotation;
            }

            //Debug.Log(transform.name);



            //Debug.Log(myPos);
            //Debug.Log(myRot);
        }

    }
}
