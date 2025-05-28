using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR;

public class BoidFishController : MonoBehaviour
{
    [Range(0.0f, 100.0f)]
    public float m_LinearVelocityMagnitude = 10.0f;
    private float m_InitialLinearVelocityMagnitude = 0.0f;

    [Range(0.0f, 100.0f)]
    public float m_SeparationFactor = 1.0f;

    [Range(0.0f, 100.0f)]
    public float m_CohesionFactor = 1.0f;

    [Range(0.0f, 100.0f)]
    public float m_AlignmentFactor = 1.0f;

    [Range(0.0f, 100.0f)]
    public float m_SeparationTerrainFactor = 1.0f;

    [Range(0.0f, 200.0f)]
    public float m_BoatAttractionFactor = 1.0f;
    private float m_InitialBoatAttractionFactor = 0.0f;

    [Range(0.0f, 100.0f)]
    public float m_QueryRadius = 100.0f;

    [Range(0.0f, 100.0f)]
    public float m_TerrainQueryRadius = 5.0f;

    [Header("COMPONENTS")]
    public SphereCollider m_SphereCollider = null;
    public Rigidbody m_Rigidbody = null;

    [SerializeField]
    private Transform m_ShipTransform = null;
    [SerializeField]
    private ShipMovement m_ShipMovement = null;
    [SerializeField]
    private Rigidbody m_ShipRb = null;

    [Header("CHASE PARAMS")]
    public float m_SpeedMultInRelationToShip = 1.0f; //How much faster the fish will be than the boat in chase
    public float m_ChaseTime = 0.0f;
    public float m_FleeingTime = 0.0f;
    [SerializeField]
    private bool m_ChaseTimerActive = false;
    [SerializeField]
    private bool m_IsFleeingFromPlayer = false;
    [SerializeField]
    private float m_Timer = 0.0f;

    [Header("BUFF PARAMS")]
    public int m_AccelerationBuff = 0;

    private void Start()
    {
        m_ShipTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        m_ShipMovement = m_ShipTransform.GetComponent<ShipMovement>();
        m_ShipRb = m_ShipTransform.GetComponent<Rigidbody>();
        m_InitialLinearVelocityMagnitude = m_LinearVelocityMagnitude;
        m_InitialBoatAttractionFactor = m_BoatAttractionFactor;
    }

    void Update()
    {
        UpdateSeparation();

        ChaseBehaviour();

        Vector3 vectorXZ = transform.forward;
        vectorXZ.y = 0;

        transform.position += Time.deltaTime * m_LinearVelocityMagnitude * vectorXZ;
    }

    void UpdateSeparation()
    {
        //Standard boid behaviour
        Vector3 positionAverage = Vector3.zero;
        Vector3 directionAlignment = Vector3.zero;
        Vector3 directionSeparation = Vector3.zero;

        //Custom behaviour
        Vector3 directionTerrainSeparation = Vector3.zero;
        Vector3 directionTowardsBoat = Vector3.zero;


        int otherBoidsCount = 0;
        int terrainCount = 0;
        

        Collider[] colliders = Physics.OverlapSphere(transform.position, m_QueryRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != gameObject && (collider.transform.position - transform.position).magnitude <= m_QueryRadius && collider.GetComponent<BoidFishController>() != null)
            {
                ++otherBoidsCount;
                positionAverage += collider.transform.position;
                directionAlignment += collider.transform.forward;
                directionSeparation += transform.position - collider.transform.position;
            }
            
            if (collider.gameObject != gameObject && (collider.transform.position - transform.position).magnitude <= m_TerrainQueryRadius && collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
            {
                //Check for terrain so the boid can flee from its direction
                ++terrainCount;
                Debug.Log(terrainCount);
                //Remove Y component from terrain position
                Vector3 colliderPos = new Vector3(collider.transform.position.x, transform.position.y, collider.transform.position.z);
                directionTerrainSeparation += transform.position - collider.transform.position;
            }
            
            //Fish is near boat, rotate towards it if its not fleeing from it
            if (collider.gameObject != gameObject && (collider.transform.position - transform.position).magnitude <= m_QueryRadius && collider.gameObject.tag == "Player")
            {
                directionTowardsBoat = collider.transform.position - transform.position;
                
                directionTowardsBoat.Normalize();

                //If its not fleeing from the player, the target orientation is towards the ship, else its in the opposite direction
                if (!m_IsFleeingFromPlayer)
                {
                    Quaternion oritentationBoat = Quaternion.LookRotation(directionTowardsBoat, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, oritentationBoat, m_BoatAttractionFactor * Time.deltaTime);
                }
                else
                {
                    Quaternion oritentationBoat = Quaternion.LookRotation(-directionTowardsBoat, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, oritentationBoat, m_BoatAttractionFactor * Time.deltaTime);
                }

            }
        }

        // Transformations for other boids
        if (otherBoidsCount > 0)
        {
            positionAverage /= otherBoidsCount;
            directionAlignment.Normalize();
            directionSeparation.Normalize();

            Vector3 directionCohesion = positionAverage - transform.position;
            directionCohesion.Normalize();

            //Get Quaternion to rotate towards the specified direction rotating the up vector
            Quaternion orientationSeparation = Quaternion.LookRotation(directionSeparation, Vector3.up);
            Quaternion orientationCohesion = Quaternion.LookRotation(directionCohesion, Vector3.up);
            Quaternion orientationAlignment = Quaternion.LookRotation(directionAlignment, Vector3.up);

            //Apply rotations
            transform.rotation = Quaternion.RotateTowards(transform.rotation, orientationSeparation, m_SeparationFactor * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, orientationCohesion, m_CohesionFactor * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, orientationAlignment, m_AlignmentFactor * Time.deltaTime);

        }

        // Transformations for terrain
        if(terrainCount > 0)
        {
            directionTerrainSeparation.Normalize();
            Quaternion orientationTerrain = Quaternion.LookRotation(directionTerrainSeparation, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, orientationTerrain, m_SeparationTerrainFactor * Time.deltaTime);
        }

    }

    private void ChaseBehaviour()
    {
        if (m_ChaseTimerActive)
        {
            m_Timer += Time.deltaTime;

            //If the chase has been going on for long enough, make the fish flee
            if (m_Timer >= m_ChaseTime)
            {
                //Reset chase
                m_ChaseTimerActive = false;
                m_IsFleeingFromPlayer = true;
                m_Timer = 0.0f;
            }
        }
        else if(m_IsFleeingFromPlayer)
        {
            m_Timer += Time.deltaTime;

            //If the fish has been fleeing for long enough, make turn off fleeing
            if (m_Timer >= m_FleeingTime)
            {
                m_IsFleeingFromPlayer = false;
                m_Timer = 0.0f;
            }
        }
    }

    //When detecting the player near, the countdown until the fish flees starts and the acceleration buff for the ship is applied
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //Make the fish has more velocity than the ship
            m_LinearVelocityMagnitude = m_InitialLinearVelocityMagnitude * m_SpeedMultInRelationToShip;
            
            m_ChaseTimerActive = true;
            ChangeShipAcceleration(m_AccelerationBuff);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            m_LinearVelocityMagnitude = m_InitialLinearVelocityMagnitude;
            ChangeShipAcceleration(-m_AccelerationBuff);
        }
    }

    private void ChangeShipAcceleration(int accelerationChange)
    {
        m_ShipMovement.m_Acceleration += accelerationChange;
        m_ShipMovement.m_RotationSpeed += accelerationChange;
    }

}
