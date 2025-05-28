using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Transform LeftDoor = null;
    public Transform RightDoor = null;

    public HingeJoint LeftDoorJoint = null;
    public HingeJoint RightDoorJoint = null;

    
    

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

        JointLimits LeftDoorLimits = LeftDoorJoint.limits;
        LeftDoorLimits.max = 0f;
        LeftDoorLimits.min = -90f;
        LeftDoorJoint.limits = LeftDoorLimits;

        JointLimits RightDoorLimits = RightDoorJoint.limits;
        RightDoorLimits.max = 90f;
        RightDoorLimits.min = 0f;
        RightDoorJoint.limits = RightDoorLimits;

        //se ralla porque la puerta derecha es la izquierda flipeada en z, pero los límites miran para el mismo lado de antes

        
    }

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
        JointMotor LeftDoorMotor = LeftDoorJoint.motor;
        LeftDoorMotor.targetVelocity = -20;
        LeftDoorMotor.force = 20;

        LeftDoorJoint.motor = LeftDoorMotor;

        JointMotor RightDoorMotor = RightDoorJoint.motor;
        RightDoorMotor.targetVelocity = 20;
        RightDoorMotor.force = 20;

        RightDoorJoint.motor = RightDoorMotor;


        //LeftDoorJoint.motor.targetVelocity = -20;
        //LeftDoorJoint.
    }

    public void RotateOutwards()
    {
        JointMotor LeftDoorMotor = LeftDoorJoint.motor;
        LeftDoorMotor.targetVelocity = 20;
        LeftDoorMotor.force = 20;

        LeftDoorJoint.motor = LeftDoorMotor;

        JointMotor RightDoorMotor = RightDoorJoint.motor;
        RightDoorMotor.targetVelocity = -20;
        RightDoorMotor.force = 20;

        RightDoorJoint.motor = RightDoorMotor;
    }
}
