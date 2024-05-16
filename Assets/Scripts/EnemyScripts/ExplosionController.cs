using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    public int m_DamageDealt;
    public float m_BlastForce;

    private void OnTriggerEnter(Collider other)
    {
        //We deal damage
        other.gameObject.GetComponent<HealthController>().DealDamage(m_DamageDealt);

        //And send back whatever entered the explosion
        Vector3 direction = other.transform.position - transform.position;
        other.GetComponent<Rigidbody>().AddForce(direction.normalized * m_BlastForce, ForceMode.Impulse);

        //Explosion sound effect
        

        //After 0.2s, we destroy the explosion
        Invoke("DestroyThisObject", 0.2f);
    }

    private void DestroyThisObject()
    {
        Destroy(gameObject);
    }

    
}
