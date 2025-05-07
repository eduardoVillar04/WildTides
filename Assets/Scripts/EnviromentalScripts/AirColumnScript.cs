using System.Collections.Generic;
using UnityEngine;

public class AirColumnScript : MonoBehaviour
{
    public float m_ForceMagnitude = 1.0f;
    
    public List<Rigidbody> m_RbList = new List<Rigidbody>();

    //Apply a force towards the forward direction of the column
    void FixedUpdate()
    {
        foreach (Rigidbody rb in m_RbList)
        {
            rb.AddForce(transform.forward * m_ForceMagnitude, ForceMode.Force);
        }
    }

    //Add rb of objects that enter the column
    private void OnTriggerEnter(Collider other)
    {
        m_RbList.Add(other.GetComponentInParent<Rigidbody>());
    }

    //Remove rb of objects that exit the column
    private void OnTriggerExit(Collider other)
    {
        m_RbList.Remove(other.GetComponentInParent<Rigidbody>());
    }
}
