using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    Transform LeftDoor = null;
    Transform RightDoor = null;

    HingeJoint LeftDoorJoint = null;
    HingeJoint RightDoorJoint = null;

    //JointMotor LeftDoorMotor = ;

    //JointMotor LeftDoorMotor = null;

    // Start is called before the first frame update
    void Start()
    {
        LeftDoor = transform.GetChild(0);
        LeftDoorJoint = LeftDoor.GetComponent<HingeJoint>();
        //LeftDoorMotor = LeftDoorJoint.motor;

        RightDoor = transform.GetChild(1);
        RightDoorJoint = RightDoor.GetComponent<HingeJoint>();

        LeftDoorJoint.useMotor = true;
        LeftDoorJoint.useLimits = true;

        RightDoorJoint.useMotor = true;
        RightDoorJoint.useLimits = true;
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            RotateInwards();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            RotateOutwards();
        }
    }

    public void RotateInwards()
    {
        //LeftDoorJoint.motor.targetVelocity = -20;
        //LeftDoorJoint.
    }

    public void RotateOutwards()
    {

    }
}
