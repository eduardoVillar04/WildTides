using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BaseController : MonoBehaviour
{
    public GameObject m_AllCrane;
    public Transform m_ActualTarget;
    public float m_Speed = 1f;

    public void MoveBase()
    {
        // Points the base of the crane towards the target
        Vector3 dir = m_ActualTarget.position - transform.position;
        dir.y = 0; // Keep the direction to only horizontal
        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, m_Speed * Time.deltaTime);

        m_AllCrane.transform.forward = transform.forward;
    }
}
