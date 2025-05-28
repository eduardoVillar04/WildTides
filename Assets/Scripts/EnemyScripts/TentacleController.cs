using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TentacleController : Enemy
{
    public int m_Damage;

    [Header("PURSUIT VARIABLES")]
    public bool m_PlayerInView;

    [Header("NAVIGATION VARIABLES")]
    public Vector3 m_ReturnPoint;
    public float m_TimeBetweenSetDestination;
    private float m_RerouteTimer;

    [Header("COMPONENTS")]
    public SphereCollider m_VisionSphere;
    public NavMeshAgent m_NavMeshAgent;
    public Transform m_PlayerTransform;

    public enum TentacleStates
    {
        NONE = -1,
        GO_TO_NEXT_POINT,
        CHECKING_FOR_PLAYER,
        PURSUING,
    }

    public TentacleStates m_CurrentState = TentacleStates.CHECKING_FOR_PLAYER;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        m_VisionSphere = GetComponent<SphereCollider>();
        m_NavMeshAgent = GetComponent<NavMeshAgent>();

        //We find the player transforms by searching it with the tag
        m_PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        switch (m_CurrentState)
        {
            case TentacleStates.CHECKING_FOR_PLAYER:
                CheckingForPlayer();
                break;
            case TentacleStates.PURSUING:
                Pursuit();
                break;
            default:
                break;
        }

    }
    public void CheckingForPlayer()
    {
        //We activate autobraking so the patroling pirates dont go past the patrol point
        m_NavMeshAgent.autoBraking = true;

        m_NavMeshAgent.isStopped = true;

        if (m_PlayerInView) m_CurrentState = TentacleStates.PURSUING;
    }


    public void Pursuit()
    {
        //We deactivate autobraking so the pirates dont try to stop when reaching the player
        m_NavMeshAgent.autoBraking = false;
        m_NavMeshAgent.isStopped = false;

        //The route is not calculated every frame to boost performance
        if (Time.time > m_RerouteTimer)
        {
            m_NavMeshAgent.SetDestination(m_PlayerTransform.position);
            m_RerouteTimer = Time.time + m_TimeBetweenSetDestination;
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            m_PlayerInView = true;
        }
    }

    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (collision.gameObject.CompareTag("Player"))
        {
            //When the player is hit, we deal damage to it and the tentacle dies
            m_HealthController.DealDamage(1);
            collision.gameObject.GetComponent<HealthController>().DealDamage(m_Damage);
        }
    }
}
