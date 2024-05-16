using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Purchasing.Extension;

public class PirateController : MonoBehaviour
{
    public int m_MaxLife;
    public int m_Damage;

    [Header("PURSUIT VARIABLES")]
    public bool m_PlayerInView;

    [Header("NAVIGATION VARIABLES")]
    public Vector3 m_ReturnPoint;
    public float m_TimeBetweenSetDestination;
    private float m_RerouteTimer;

    [Header("COMPONENTS")]
    public SphereCollider m_VisionSphere;
    public HealthController m_HealthController;
    public NavMeshAgent m_NavMeshAgent;
    public Transform m_PlayerTransform;
    
    public enum PirateStates
    {
        NONE = -1,
        CHECKING_FOR_PLAYER,
        PURSUING,
        RETURN_TO_POINT
    }

    public PirateStates m_CurrentState = PirateStates.CHECKING_FOR_PLAYER;

    // Start is called before the first frame update
    void Start()
    {
        m_HealthController = GetComponent<HealthController>();
        m_VisionSphere = GetComponent<SphereCollider>();
        m_NavMeshAgent = GetComponent<NavMeshAgent>();

        //We find the player transforms by searching it with the tag
        m_PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        m_MaxLife = m_HealthController.m_HealthPoints;

        //The return point for the pirate will be wherever it spawns
        m_ReturnPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_HealthController.m_IsDead) m_CurrentState = PirateStates.RETURN_TO_POINT;

        switch (m_CurrentState)
        {
            case PirateStates.CHECKING_FOR_PLAYER:
                CheckingForPlayer();
                break;
            case PirateStates.PURSUING:
                Pursuit();
                break;
            case PirateStates.RETURN_TO_POINT:
                ReturnToPoint();
                break;
            default:
                break;
        }
    }
    public void CheckingForPlayer()
    {
        m_NavMeshAgent.isStopped = true;

        if (m_PlayerInView) m_CurrentState = PirateStates.PURSUING;
    }

    public void Pursuit()
    {
        m_NavMeshAgent.isStopped = false;

        if(Time.time > m_RerouteTimer) 
        {
            m_NavMeshAgent.SetDestination(m_PlayerTransform.position);
            m_RerouteTimer = Time.time + m_TimeBetweenSetDestination;
        }
    }


    public void ReturnToPoint()
    {
        m_NavMeshAgent.isStopped = false;
        m_NavMeshAgent.SetDestination(m_ReturnPoint);

        if(m_NavMeshAgent.remainingDistance < 0.1f)
        {
            m_HealthController.m_HealthPoints = m_MaxLife;
            m_CurrentState = PirateStates.CHECKING_FOR_PLAYER;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            m_PlayerInView = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            m_PlayerInView = false;
            m_CurrentState = PirateStates.RETURN_TO_POINT;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            //When the player is hit, we deal damage to it and the pirate flees to the return point
            collision.gameObject.GetComponent<HealthController>().DealDamage(m_Damage);
            m_CurrentState = PirateStates.RETURN_TO_POINT;
        }
    }
}
