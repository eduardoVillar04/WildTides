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

    [SerializeField] private bool m_AlreadyDied = false;

    public virtual void Start()
    {
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
            
            Sink();

            if (!m_AlreadyDied)
            {
                Invoke("ReturnToPool", 2f);
                m_AlreadyDied = true;
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
            m_HealthController.DealDamage(1);
        }

        //Knockback player
        if(collision.gameObject.CompareTag("Player"))
        {
            Vector3 direction = collision.transform.position - transform.position;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(direction.normalized * m_KnockbackForce, ForceMode.Impulse);
        }
    }

    public void ReturnToPool()
    {
        EntitiesPoolManager.instance.ReturnEntityToPool(this.gameObject);
    }
}
