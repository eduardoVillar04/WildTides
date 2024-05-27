using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GustOfWindController : MonoBehaviour
{
    public float m_GustForce;

    private void Start()
    {
        Invoke("DestroyThisObject", 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Checks if other object is enemy to in order to move it, might have to be changed if we continue development
        //also, the collider must not be a trigger (for example pirate vision sphere)
        if(other.gameObject.CompareTag("Enemy") && !other.isTrigger)
        {
            Rigidbody m_EnemyRB = other.GetComponent<Rigidbody>();
            
            //If the target is a pirate, we make it pursue the player no matter the distance
            if (other.gameObject.GetComponent<PirateController>())
            {
                other.gameObject.GetComponent<PirateController>().m_CurrentState = PirateController.PirateStates.PURSUING;
            }
            
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
