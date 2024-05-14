using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrelController : MonoBehaviour
{
    public GameObject m_Explosion;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            //If an enemy or the player collides with the barrel, it creates and explosion and dies
            Instantiate(m_Explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
