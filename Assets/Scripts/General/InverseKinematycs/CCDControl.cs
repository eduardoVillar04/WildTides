//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CCDControl : MonoBehaviour
//{

//	// Use this for initialization
//	void Start()
//  {
//    parts = GetComponentsInChildren<Transform>();
//	}

//	// Update is called once per frame
//	void Update()
//  {
//		if ( isSearchEnabled ) {
//      foreach ( Transform part in parts ) {
//        Vector3 currentDirection =
//          isPureCCD ?
//          ( part.childCount == 0 ? part.forward : ( parts[parts.Length-1].position - part.position ) ) :
//          part.forward;
//        Vector3 goalDirection = target.transform.position - part.position;
//        if ( part.GetComponent<RotationAxis>() != null )
//        {
//          Vector3 axisInWorldCoordinates = part.rotation * part.GetComponent<RotationAxis>().axis;
//          goalDirection = Vector3.ProjectOnPlane( goalDirection, axisInWorldCoordinates );
//        }


//        Quaternion goalOrientation = Quaternion.FromToRotation(currentDirection, goalDirection) * part.rotation;



//        Quaternion newOrientation = Quaternion.Slerp(part.rotation, goalOrientation, 1.0f * Time.deltaTime);
//        if ( part.parent == null ||
//             transform.GetComponent<AngleLimit>() == null ||
//             Vector3.Angle( part.parent.transform.forward, newOrientation * Vector3.forward ) < part.GetComponent<AngleLimit>().value ) { 
//          part.rotation = newOrientation;
//        }
//      }
//    }
//	}

//  // ATTRIBUTES
//  public  bool        isPureCCD       = true;  // Pure CCD switch
//  public  bool        isSearchEnabled = false; // Switch controlling active search
//  public  GameObject  target          = null;  // Target
//  public  Transform[] parts;                   // Object collection
//}

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class CCDControl : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        parts = GetComponentsInChildren<Transform>();

        // Default orientations, child last positions and rotation speeds
        defaultOrientations = new Dictionary<Transform, Quaternion>();
        childlastPositions = new Dictionary<Transform, Vector3>();
        rotationSpeeds = new Dictionary<Transform, Vector3>();
        rotationDefaulX = new Dictionary<Transform, Vector3>();
        foreach (Transform part in parts)
        {
            defaultOrientations.Add(part, part.rotation);
            childlastPositions.Add(part, part.childCount > 0 ? part.GetChild(0).position : Vector3.zero);
            rotationSpeeds.Add(part, Vector3.zero);
            Vector3 rawAngles = part.localEulerAngles;
            Vector3 correctedAngles = NormalizeAngles(rawAngles);
            rotationDefaulX.Add(part, correctedAngles);
        }
        foreach (KeyValuePair<Transform, Vector3> d in rotationDefaulX)
        {
            Debug.Log($"Object: {d.Key} - Local Rotation (Euler): {d.Value}");
        }
    }

    Vector3 NormalizeAngles(Vector3 angles)
    {
        return new Vector3(NormalizeAngle(angles.x), NormalizeAngle(angles.y), NormalizeAngle(angles.z));
    }

    float NormalizeAngle(float angle)
    {
        angle = angle % 360;
        if (angle > 180) angle -= 360;
        return angle;
    }

    // Update is called once per frame
    void Update()
    {   
        UpdateInput();

        foreach (Transform part in parts)
        {
            // Reaction to movement
            if (isReactionToMovementEnabled && part.childCount > 0)
            {
                rotationSpeeds[part] += 20.0f * Vector3.Cross(part.forward, childlastPositions[part] - part.position);
                part.Rotate(Time.deltaTime * rotationSpeeds[part], Space.World);
                rotationSpeeds[part] -= 0.3f * Time.deltaTime * rotationSpeeds[part];
            }
            // Current direction
            Vector3 currentDirection =
              isPureCCD ?
              (part.childCount == 0 ? part.forward : (parts[parts.Length - 1].position - part.position)) :
              part.forward;
            Vector3 goalDirection = target.transform.position - part.position;
            // Goal orientation
            Quaternion goalOrientation =
              isSearchEnabled ?
              Quaternion.FromToRotation(currentDirection, goalDirection) * part.rotation :
              defaultOrientations[part];
            // New orientation
            Quaternion newOrientation = Quaternion.Slerp(part.rotation, goalOrientation, (isSearchEnabled ? searchSpeed : idleSpeed) * Time.deltaTime);
            // Update
            if (part.parent == null ||
                  part.GetComponent<AngleLimit>() == null ||
                  Mathf.Abs(Vector3.Angle(part.parent.transform.forward, newOrientation * Vector3.forward)) > (rotationDefaulX[part].x + part.GetComponent<AngleLimit>().value))
            {
                Debug.Log(part.GetComponent<AngleLimit>().value + ":::" + (defaultOrientations[part].x + part.GetComponent<AngleLimit>().value));
                if (part.GetComponent<AngleLimit>().value == 10) { Debug.Log(part.GetComponent<AngleLimit>().value + " ___Angle:" + Vector3.Angle(part.parent.transform.forward, newOrientation * Vector3.forward)); }
                part.rotation = newOrientation;
            }
            // Child last position storage
            childlastPositions[part] = part.childCount > 0 ? part.GetChild(0).position : Vector3.zero;
        }
    }



    private void UpdateInput()
    {

        //if ( Input.GetKeyDown( KeyCode.Space ) ) {
        //  SoundManager.instance.PlayRandomSoundEffect( new AudioClip[]{ audioClipA, audioClipB }, 0.2f );
        //  StartCoroutine( SearchForSeconds( 0.5f ) );
        //}

        //// TODO: The behaviour below should not be controlled here, but in a compoment present in the parent
        //if ( transform.parent != null ) {
        //  transform.parent.position += Time.deltaTime * 10.0f * Input.GetAxis( "Vertical" ) * transform.parent.forward;
        //  transform.parent.position += Time.deltaTime * 10.0f * Input.GetAxis( "Horizontal" ) * transform.parent.right;
        //}
    }

    private IEnumerator SearchForSeconds(float duration)
    {
        isSearchEnabled = true;
        yield return new WaitForSeconds(duration);
        isSearchEnabled = false;
    }

    // ATTRIBUTES

    public bool isPureCCD = true;  // Pure CCD switch
    public bool isSearchEnabled = false; // Switch controlling active search
    public bool isReactionToMovementEnabled = true;  // Switch controlling reaction to movement
    public float searchSpeed = 1.0f;  // Search speed
    public float idleSpeed = 1.0f;  // Idle speed
    public GameObject target = null;  // Target
    public Transform[] parts;                                // Object collection

    public AudioClip audioClipA = null;
    public AudioClip audioClipB = null;

    private Dictionary<Transform, Quaternion> defaultOrientations; // Default orientations
    private Dictionary<Transform, Vector3> childlastPositions;  // Child last positions
    private Dictionary<Transform, Vector3> rotationSpeeds;      // RotationSpeeds

    private Dictionary<Transform, Vector3> rotationDefaulX;      // RotationSpeeds

}
