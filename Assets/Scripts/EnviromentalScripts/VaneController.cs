using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaneController : MonoBehaviour
{
   
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HS_ProjectileMover>() != null)
        {
            var joint = gameObject.AddComponent<HingeJoint>();
            joint.axis = Vector3.up; // Eje Y (gira en horizontal)
            joint.useSpring = true;

            JointSpring spring = new JointSpring();
            spring.spring = 0f; // No fuerza de retorno
            spring.damper = 10f; // Amortiguación para frenar rotación
            spring.targetPosition = 0f;

            joint.spring = spring;
        }
    }
}
