using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class PirateController : Enemy
{
    public int m_Damage;

    [Header("PURSUIT VARIABLES")]
    public float m_PursuitSpeed;
    public float m_StoppingDistance;

    [Header("SHOOTING VARIABLES")]
    public float m_SpeedWhileShooting;
    public float m_ShootingDistance;
    public Transform m_BulletSpawnPoint;
    public Transform m_CannonTransform;
    public float m_CannonRotationSpeed;
    public GameObject m_BulletPrefab;
    public float m_TimeBetweenShots;
    public float m_ShotTimer;
    public float m_ShootingAngle = 45.0f;
    public float m_SecsBulletIsIntangibleAfterShooting = 0.3f;

    [Header("NAVIGATION VARIABLES")]
    public float m_TimeBetweenRerouting;
    private float m_RerouteTimer;

    [Header("COMPONENTS")]
    public SphereCollider m_VisionSphere;
    public NavMeshAgent m_NavMeshAgent;
    public Transform m_PlayerTransform;
    public GameObject m_TerrainCollider;
    public GameObject m_Floaters;

    [Header("Audio")]
    public AudioClip m_ShootSound;

    public enum PirateStates
    {
        NONE = -1,
        CHECKING_FOR_PLAYER,
        PURSUING,
    }

    public PirateStates m_CurrentState = PirateStates.NONE;
    private PirateStates m_LastState = PirateStates.NONE;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        m_CurrentState = PirateStates.CHECKING_FOR_PLAYER;
        m_LastState = m_CurrentState;

        //almost ready to fire
        m_ShotTimer = m_TimeBetweenShots - 0.5f;

        m_VisionSphere = GetComponent<SphereCollider>();
        m_NavMeshAgent = GetComponent<NavMeshAgent>();

        //Navmeshagent starts disabled so that the pirate floats initially
        m_NavMeshAgent.enabled = false;

        //We find the player transforms by searching it with the tag
        m_PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        OnStateChanged();

        switch (m_CurrentState)
        {
            case PirateStates.CHECKING_FOR_PLAYER:
                CheckingForPlayer();
                break;
            case PirateStates.PURSUING:
                Pursuit();
                break;
            default:
                break;
        }

        m_LastState = m_CurrentState;
    }

    private void OnStateChanged()
    {
        if(m_LastState != m_CurrentState)
        {
            //Switch if navMeshAgent is enabled if pirate entered or left checking for players
            //When in checking for players the pirate has the navmeshagent component disabled so that floaters work, when it leaves the navmesh is enabled again
            if(m_LastState == PirateStates.CHECKING_FOR_PLAYER || m_CurrentState == PirateStates.CHECKING_FOR_PLAYER)
            {
                m_Floaters.SetActive(!m_Floaters.activeSelf);
                m_NavMeshAgent.enabled = !m_NavMeshAgent.enabled;
            }
        }
    }

    public void CheckingForPlayer()
    {
        //We activate autobraking so the patroling pirates dont go past the patrol point
        m_NavMeshAgent.autoBraking = true;
        m_NavMeshAgent.isStopped = true;
    }

    public void Pursuit()
    {
        //We deactivate autobraking so the pirates dont try to stop when reaching the player
        m_NavMeshAgent.autoBraking = true;
        m_NavMeshAgent.isStopped = false;

        //The route is not calculated every frame to boost performance
        if (Time.time > m_RerouteTimer)
        {
            m_NavMeshAgent.SetDestination(m_PlayerTransform.position);
            m_RerouteTimer = Time.time + m_TimeBetweenRerouting;
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
        //NEW SHOOTING BEHAVIOUR

        //Might have to not take into account the Y component of the vector to get better results, these formulas are for objects in the same altitude

        Vector3 targetPos = new Vector3(m_PlayerTransform.position.x, m_BulletSpawnPoint.position.y, m_PlayerTransform.position.z);
        Vector3 directionVector = targetPos - m_BulletSpawnPoint.position;
        float distanceToTarget = directionVector.magnitude;

        //Instantiate the bullet so its forward vector is towards the target
        GameObject newBullet = Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position, Quaternion.LookRotation(directionVector));
        Rigidbody bulletRb = newBullet.GetComponent<Rigidbody>();
        Transform bulletTransform = newBullet.GetComponent<Transform>();

        Vector3 initialLinearVelocity = CalculateInitialLinearVelocity(distanceToTarget);
        bulletRb.AddForce(initialLinearVelocity.z * bulletTransform.forward, ForceMode.VelocityChange);
        bulletRb.AddForce(initialLinearVelocity.y * bulletTransform.up, ForceMode.VelocityChange);

        //We change the collider state from trigger to not trigger after spawning the bullet so that it doesnt destroy the enemy that shoots it
        StartCoroutine(ChangeColliderState(newBullet.GetComponent<Collider>(), m_SecsBulletIsIntangibleAfterShooting));

        //Audio
        SoundEffectsManager.instance.PlaySoundFXClip(m_ShootSound, transform, 0.6f);
    }

    private Vector3 CalculateInitialLinearVelocity(float distanceToTarget)
    {
        //Formulas in page 121 of physics for game developers
        float velocityMagnitude = Mathf.Sqrt((distanceToTarget * Physics.gravity.magnitude) / (2 * Mathf.Sin(m_ShootingAngle) * Mathf.Cos(m_ShootingAngle)));
        Vector3 initialVelocity = new Vector3(0, velocityMagnitude * Mathf.Cos(m_ShootingAngle), velocityMagnitude * Mathf.Sin(m_ShootingAngle));
        return initialVelocity;
    }   

    private IEnumerator ChangeColliderState(Collider objCollider, float intangibleTime)
    {
        yield return new WaitForSeconds(intangibleTime);
        objCollider.isTrigger = !objCollider.isTrigger;
    }

    public void OnTriggerEnter(Collider other)
    {
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
