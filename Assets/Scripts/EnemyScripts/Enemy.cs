using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    [Header("BASE ENEMY PARAMETERS")]
    public Transform m_Transform;
    public Rigidbody m_Rigidbody;
    public HealthController m_HealthController;
    public float m_KnockbackForce;
    public float m_DeathSpeed;
    public float m_SecondsBeforeDespawn = 3f;
    private float m_DespawnTimer = 3f;

    public virtual void Start()
    {
        m_Transform = GetComponent<Transform>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_HealthController = GetComponent<HealthController>();
        m_DespawnTimer = m_SecondsBeforeDespawn;
    }

    public virtual void Update()
    {
        if(m_HealthController.m_IsDead)
        {
            //if it has a navmesh agent we disable it so the object can sink
            if(GetComponent<NavMeshAgent>()) gameObject.GetComponent<NavMeshAgent>().enabled = false;
            
            Sink();

            m_DespawnTimer -= Time.deltaTime;

            if(m_DespawnTimer < 0)
            {
                EntitiesPoolManager.instance.ReturnEntityToPool(this.gameObject);
            }

        }
    }

    public void Sink()
    {
        //We make the enemy sink
        m_Transform.position -= new Vector3(0, m_DeathSpeed, 0);
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        //Deal damage if knocked into terrain
        if(collision.gameObject.CompareTag("Terrain") || collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("[" + this.gameObject.name + "]" + " HIT BY: " + collision.gameObject.name);
            if (collision.gameObject == this.gameObject) Debug.LogError("HIT BY MYSELF"); 
            
            m_HealthController.DealDamage(1);
        }

        //Knockback player
        if(collision.gameObject.CompareTag("Player"))
        {
            Vector3 direction = collision.transform.position - transform.position;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(direction.normalized * m_KnockbackForce, ForceMode.Impulse);
        }
    }
}
