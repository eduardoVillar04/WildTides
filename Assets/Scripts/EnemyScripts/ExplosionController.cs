using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExplosionController : MonoBehaviour
{
    public int m_DamageDealt;
    public float m_BlastForce;
    public ParticleSystem m_ParticleSystem;

    private void Start()
    {
        m_ParticleSystem = GetComponent<ParticleSystem>();
        //After 0.2s, we destroy the explosion
        StartCoroutine(DestroyThisObject(m_ParticleSystem.main.duration));
    }

    private void OnTriggerEnter(Collider other)
    {
        //if its an enemy, it dies
        if (other.gameObject.GetComponent<Enemy>()) other.gameObject.GetComponent<HealthController>().m_IsDead = true;

        //We deal damage
        other.gameObject.GetComponent<HealthController>().DealDamage(m_DamageDealt);

        //And send back whatever entered the explosion
        Vector3 direction = other.transform.position - transform.position;
        other.GetComponent<Rigidbody>().AddForce(direction.normalized * m_BlastForce, ForceMode.Impulse);
    }

    public IEnumerator DestroyThisObject(float timeBeforeDestruction)
    {
        yield return new WaitForSeconds(timeBeforeDestruction);
        Destroy(gameObject);
    }


}
