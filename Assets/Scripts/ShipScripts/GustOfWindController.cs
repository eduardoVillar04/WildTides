using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GustOfWindController : MonoBehaviour
{
    public float m_GustForce;

    private void Start()
    {
        Invoke("DestroyThisObject", 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Checks if other obkect is enemy to move it, might have to be changed if we continue development
        if(other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody m_EnemyRB = other.GetComponent<Rigidbody>();
            
            //We use impulse to add more or less force depending on mass of the object
            m_EnemyRB.AddForce(transform.forward * m_GustForce, ForceMode.Impulse);
            Destroy(gameObject);
        }
    }

    public void DestroyThisObject()
    {
        Destroy(gameObject);
    }
}
