using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Rendering;

public class PirateController : Enemy
{
    public int m_Damage;

    [Header("PATROL")]
    public Transform m_Patrol;

    [Header("PURSUIT VARIABLES")]
    public float m_PursuitSpeed;
    public float m_StoppingDistance;
    public Transform[] m_AllTargetPoints;
    public int m_CurrentTargetPointIndex = 0;
    public Transform m_CurrentTargetPoint;
    public bool m_IsPatrolingEnemy;

    [Header("SHOOTING VARIABLES")]
    public float m_SpeedWhileShooting;
    public float m_ShootingDistance;
    public Transform m_BulletSpawnPoint;
    public Transform m_CannonTransform;
    public float m_CannonRotationSpeed;
    public GameObject m_BulletPrefab;
    public float m_BulletSpeed;
    public float m_TimeBetweenShots;
    public float m_ShotTimer;

    [Header("NAVIGATION VARIABLES")]
    public Vector3 m_ReturnPoint;
    public float m_TimeBetweenSetDestination;
    private float m_RerouteTimer;

    [Header("COMPONENTS")]
    public SphereCollider m_VisionSphere;
    public NavMeshAgent m_NavMeshAgent;
    public Transform m_PlayerTransform;

    [Header("Audio")]
    public AudioClip m_ShootSound;
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
    public override void Start()
    {
        base.Start();

        //ready to fire
        m_ShotTimer = m_TimeBetweenShots;

        m_VisionSphere = GetComponent<SphereCollider>();
        m_NavMeshAgent = GetComponent<NavMeshAgent>();

        //We find the player transforms by searching it with the tag
        m_PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;

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
            m_IsPatrolingEnemy = true;
            m_CurrentState = PirateStates.GO_TO_NEXT_POINT;
            m_CurrentTargetPointIndex = -1;
        }
        catch (System.Exception)
        {
            m_IsPatrolingEnemy = false;
            m_ReturnPoint = transform.position;
            m_CurrentState = PirateStates.CHECKING_FOR_PLAYER;
        }

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

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
        if(!m_IsPatrolingEnemy) m_NavMeshAgent.isStopped = true;
        
        if (m_IsPatrolingEnemy && m_NavMeshAgent.remainingDistance < 0.1f)
        {
            m_CurrentState = PirateStates.GO_TO_NEXT_POINT;
        }

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
        m_NavMeshAgent.autoBraking = true;
        m_NavMeshAgent.isStopped = false;

        //[anterior]
        //The route is not calculated every frame to boost performance
        if (Time.time > m_RerouteTimer)
        {
            m_NavMeshAgent.SetDestination(m_PlayerTransform.position);
            m_RerouteTimer = Time.time + m_TimeBetweenSetDestination;
            m_NavMeshAgent.stoppingDistance = m_StoppingDistance;
        }

        if (m_NavMeshAgent.remainingDistance < m_ShootingDistance)
        {
            m_NavMeshAgent.speed = m_SpeedWhileShooting;

            //We make the cannon rotate towards the player
            CannonLooksAtPlayer();

            ////We add time to the timer until timebetweenshots is reached
            m_ShotTimer += Time.deltaTime;

            if(m_ShotTimer > m_TimeBetweenShots)
            {
                m_ShotTimer = 0;
                Shoot();
            }
        }
        else
        {
            m_ShotTimer = 0;
            m_NavMeshAgent.speed = m_PursuitSpeed;
        }
    }

    public void CannonLooksAtPlayer()
    {    
        //Look at player
        Vector3 lookPos = m_PlayerTransform.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        m_CannonTransform.rotation = Quaternion.Slerp(m_BulletSpawnPoint.rotation, rotation, Time.deltaTime * m_CannonRotationSpeed);
    }

    public void Shoot()
    {
        GameObject cannonBullet = Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position, Quaternion.identity);
        Rigidbody cbRB = cannonBullet.GetComponent<Rigidbody>();
        Vector3 direction = m_PlayerTransform.position - m_BulletSpawnPoint.position;
        cbRB.AddForce(direction * m_BulletSpeed, ForceMode.VelocityChange);
        //Audio
        SoundEffectsManager.instance.PlaySoundFXClip(m_ShootSound, transform, 0.6f);
    }


    public void ReturnToPoint()
    {
        m_NavMeshAgent.autoBraking = true;
        m_NavMeshAgent.isStopped = false;
        
        if(!m_IsPatrolingEnemy)
        {
            m_NavMeshAgent.SetDestination(m_ReturnPoint);
        }
        else
        {
            m_NavMeshAgent.SetDestination(m_CurrentTargetPoint.position);
        }

        if(m_NavMeshAgent.remainingDistance < 1f)
        {
            m_CurrentState = PirateStates.CHECKING_FOR_PLAYER;
        }
    }


    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if(other.gameObject.CompareTag("Player"))
        {
            m_CurrentState = PirateStates.PURSUING;
        }

    }

    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }

}
