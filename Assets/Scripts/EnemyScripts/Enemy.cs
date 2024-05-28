using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    public Transform m_Transform;
    public Rigidbody m_Rigidbody;
    public HealthController m_HealthController;
    public bool m_IsDead;
    public float m_DeathSpeed;

    public virtual void Start()
    {
        //default deathspeed
        m_DeathSpeed = 0.05f;
        m_IsDead = false;
        m_Transform = GetComponent<Transform>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_HealthController = GetComponent<HealthController>();
    }

    public virtual void Update()
    {
        if(m_HealthController.m_IsDead)
        {
            //if it has a navmesh agent we disable it so the object can sink
            if(GetComponent<NavMeshAgent>()) gameObject.GetComponent<NavMeshAgent>().enabled = false;
            Die();
        }
    }

    public void Die()
    {
        //We make the enemy sink
        m_Transform.position -= new Vector3(0, m_DeathSpeed, 0);
        Invoke("InvokeDestroy",2f);
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Terrain") || collision.gameObject.CompareTag("Enemy"))
        {
            m_HealthController.DealDamage(1);
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        
    }

    public void InvokeDestroy()
    {
        Destroy(this.gameObject);
    }
}
