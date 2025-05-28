using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrelController : MonoBehaviour
{
    public GameObject m_Explosion;
    public float m_ExplosionForce;
    [SerializeField] private AudioClip m_ExplosionSound;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            //If an enemy or the player collides with the barrel, it creates and explosion and dies
            Instantiate(m_Explosion, transform.position, Quaternion.identity);

            Vector3 direction = (collision.transform.position - transform.position).normalized;
            Vector3 XZplaneDirection = new Vector3(direction.x, 0, direction.z); //Dont take into account the force applied vertically
            collision.gameObject.GetComponent<Rigidbody>().AddForce(XZplaneDirection * m_ExplosionForce, ForceMode.Impulse);
            collision.gameObject.GetComponent<HealthController>().DealDamage(1);

            SoundEffectsManager.instance.PlaySoundFXClip(m_ExplosionSound, transform, 1f);
            EntitiesPoolManager.instance.ReturnEntityToPool(this.gameObject);
        }
    }
}
