using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CannonBulletController : MonoBehaviour
{
    public int m_Damage;

    private void Start()
    {
        Invoke("DestroyThisObject", 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //If a player is hit, the cannon ball deals damage and is destroyed

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<HealthController>().DealDamage(m_Damage);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Terrain")) Destroy(this.gameObject);

    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    //If a player is hit, the cannon ball deals damage and is destroyed

    //    if(other.gameObject.CompareTag("Player") && !other.isTrigger)
    //    {
    //        other.GetComponent<HealthController>().DealDamage(m_Damage);
    //        Destroy(gameObject);
    //    }

    //    if (other.gameObject.CompareTag("Terrain")) Destroy(this.gameObject);

    //    //TODO: friendly fire?

    //}

    public void DestroyThisObject()
    {
        Destroy(gameObject);
    }
}
