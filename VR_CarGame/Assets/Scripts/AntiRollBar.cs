using System.Collections;
using UnityEngine;
public class AntiRollBar : MonoBehaviour
{
    public WheelCollider WheelL;
    public WheelCollider WheelR;
    private Rigidbody carRigidBody;

    public float AntiRoll = 5000f;

    // Use this for initialization
    void Start()
    {
        carRigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void fixedUpdate()
    {
        WheelHit hit = new WheelHit();
        float travelL = 1f;
        float travelR = 1f;

        bool groundedL = WheelL.GetGroundHit(out hit);

        if (groundedL)
            travelL = (-WheelL.transform.InverseTransformPoint(hit.point).y - WheelL.radius) / WheelL.suspensionDistance;

        bool groundedR = WheelL.GetGroundHit(out hit);

        if (groundedR)
            travelR = (-WheelR.transform.InverseTransformPoint(hit.point).y - WheelR.radius) / WheelR.suspensionDistance;

        var antiRollForce = (travelL - travelR) * AntiRoll;

        if (groundedL)
            carRigidBody.AddForceAtPosition(WheelL.transform.up * -antiRollForce,
                                         WheelL.transform.position);
        if (groundedR)
            carRigidBody.AddForceAtPosition(WheelR.transform.up * antiRollForce,
                                         WheelR.transform.position);
    }
}