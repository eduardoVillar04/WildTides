using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCDControlNew : MonoBehaviour
{
    // ATTRIBUTES
    public bool isPureCCD = true;  // Pure CCD switch
    public bool isSearchEnabled = false; // Switch controlling active search
    public GameObject target = null;  // Target
    public Transform[] parts;                   // Object collection    

    // Use this for initialization
    void Start()
    {
        parts = GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space)) { isSearchEnabled = !isSearchEnabled; }
        if (isSearchEnabled)
        {
            foreach (Transform part in parts)
            {
                Vector3 currentDirection =
                  isPureCCD ?
                  (part.childCount == 0 ? part.forward : (parts[parts.Length - 1].position - part.position)) :
                  part.forward;
                Vector3 goalDirection = target.transform.position - part.position;
                if (part.GetComponent<RotationAxis>() != null)
                {
                    Vector3 axisInWorldCoordinates = part.rotation * part.GetComponent<RotationAxis>().axis;
                    goalDirection = Vector3.ProjectOnPlane(goalDirection, axisInWorldCoordinates);
                }


                Quaternion goalOrientation = Quaternion.FromToRotation(currentDirection, goalDirection) * part.rotation;



                Quaternion newOrientation = Quaternion.Slerp(part.rotation, goalOrientation, 1.0f * Time.deltaTime);
                if (part.parent == null ||
                     transform.GetComponent<AngleLimit>() == null ||
                     Vector3.Angle(part.parent.transform.forward, newOrientation * Vector3.forward) < part.GetComponent<AngleLimit>().value)
                {
                    part.rotation = newOrientation;
                }
            }
        }
    }

    
}
