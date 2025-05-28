using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CannonBulletController : MonoBehaviour
{
    public int m_Damage;
    public GameObject m_Explosion = null;
    [SerializeField] private AudioClip m_ExplosionSound;

    private void Start()
    {
        Invoke("DestroyThisObject", 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //If a player or enemy is hit, the cannon ball deals damage, creates and explosion and is destroyed

        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            //Damage
            collision.gameObject.GetComponent<HealthController>().DealDamage(m_Damage);

            //Explosion
            Instantiate(m_Explosion, transform.position, Quaternion.identity);
            //Disable the sphereCollider so that it doesnt damage the player or enemies
            m_Explosion.GetComponent<SphereCollider>().enabled = false;
            SoundEffectsManager.instance.PlaySoundFXClip(m_ExplosionSound, transform, 1f);
            
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Terrain")) Destroy(this.gameObject);

    }

    public void DestroyThisObject()
    {
        Destroy(gameObject);
    }
}
