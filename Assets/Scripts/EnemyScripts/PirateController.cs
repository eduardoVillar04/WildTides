using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Rendering;

public class PirateController : MonoBehaviour
{
    public int m_MaxLife;
    public int m_Damage;

    [Header("PATROL")]
    public Transform m_Patrol;

    [Header("PURSUIT VARIABLES")]
    public bool m_PlayerInView;
    public Transform[] m_AllTargetPoints;
    public int m_CurrentTargetPointIndex = 0;
    public Transform m_CurrentTargetPoint;
    public bool m_IsPatrolingPirate;

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
        GO_TO_NEXT_POINT,
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

        //If the patrol transform is null, an exception will be thrown
        try
        {
            //We put all the patrol points into a list
            m_AllTargetPoints = new Transform[m_Patrol.childCount];
            for (int i = 0; i < m_AllTargetPoints.Length; i++)
            {
                m_AllTargetPoints[i] = m_Patrol.GetChild(i);
            }
            m_CurrentTargetPoint = m_AllTargetPoints[m_CurrentTargetPointIndex];
            m_IsPatrolingPirate = true;
            m_CurrentState = PirateStates.GO_TO_NEXT_POINT;
            m_CurrentTargetPointIndex = -1;
        }
        catch (System.Exception)
        {
            m_IsPatrolingPirate = false;
            m_ReturnPoint = transform.position;
            m_CurrentState = PirateStates.CHECKING_FOR_PLAYER;
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (m_HealthController.m_IsDead) m_CurrentState = PirateStates.RETURN_TO_POINT;

        switch (m_CurrentState)
        {
            case PirateStates.GO_TO_NEXT_POINT:
                GoToNextPoint();
                break;
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
        //We activate autobraking so the patroling pirates dont go past the patrol point
        m_NavMeshAgent.autoBraking = true;
        if(!m_IsPatrolingPirate) m_NavMeshAgent.isStopped = true;
        
        if (m_IsPatrolingPirate && m_NavMeshAgent.remainingDistance < 0.1f)
        {
            m_CurrentState = PirateStates.GO_TO_NEXT_POINT;
        }

        if (m_PlayerInView) m_CurrentState = PirateStates.PURSUING;

    }

    public void GoToNextPoint()
    {
        //We add the aproppiate amount to the index, and we send the pirate to the next patrol point
        m_CurrentTargetPointIndex++;
        if (m_CurrentTargetPointIndex >= m_AllTargetPoints.Length)
        {
            m_CurrentTargetPointIndex = 0;
        }
        m_CurrentTargetPoint = m_AllTargetPoints[m_CurrentTargetPointIndex];

        m_NavMeshAgent.SetDestination(m_CurrentTargetPoint.position);
        m_NavMeshAgent.isStopped = false;

        m_CurrentState = PirateStates.CHECKING_FOR_PLAYER;
    }

    public void Pursuit()
    {
        //We deactivate autobraking so the pirates dont try to stop when reaching the player
        m_NavMeshAgent.autoBraking = false;
        m_NavMeshAgent.isStopped = false;

        //The route is not calculated every frame to boost performance
        if(Time.time > m_RerouteTimer) 
        {
            m_NavMeshAgent.SetDestination(m_PlayerTransform.position);
            m_RerouteTimer = Time.time + m_TimeBetweenSetDestination;
        }
    }


    public void ReturnToPoint()
    {
        m_NavMeshAgent.autoBraking = true;
        m_NavMeshAgent.isStopped = false;
        
        if(!m_IsPatrolingPirate)
        {
            m_NavMeshAgent.SetDestination(m_ReturnPoint);
        }
        else
        {
            m_NavMeshAgent.SetDestination(m_CurrentTargetPoint.position);
        }

        if(m_NavMeshAgent.remainingDistance < 1f)
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

    public void EnableNavMeshAgent()
    {
        m_NavMeshAgent.enabled = true;
    }
}
