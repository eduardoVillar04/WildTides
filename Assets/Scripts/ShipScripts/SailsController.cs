using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SailsController : MonoBehaviour
{
    [Header("COMPONENTS")]
    public List<Cloth> m_SailsList = null;
    
    [Header("CONTROL VARS")]
    public float m_CurrentExternalAccelerationMagnitude = 0.0f;
    public float m_MaxExternalAccelerationMagnitude = 50.0f;
    public float m_CurrentShipSpeed = 0.0f;
    //The velocity magnitude value the ship has to have so teh sails reach max external acceleration
    public float m_MaxShipSpeedMagnitude = 10.0f;

    //The component that will be used to reference speed if the ship is from the player
    private Rigidbody m_ShipRigidBody = null;

    //The component that will be used to reference speed if the ship is from an enemy pirate
    private PirateController m_PirateController = null;

    [Range(0f, 1f)]
    public float m_RandomFactor = 0.5f;

    private void Start()
    {
        m_ShipRigidBody = GetComponent<Rigidbody>();

        //If its a pirate, also get the pirate controller to calculate velocity
        if (GetComponent<PirateController>() != null)
        {
            m_PirateController = GetComponent<PirateController>();
        }
    }


    // Update is called once per frame
    void Update()
    {
        //If the ship is a pirate the velocity is found in the navmesh component, in the player ship its in the rigidbody
        if (m_PirateController != null)
        {
            m_CurrentShipSpeed = m_PirateController.m_NavMeshAgent.velocity.magnitude;
        }
        else
        {
            m_CurrentShipSpeed = m_ShipRigidBody.velocity.magnitude;
        }


        //Calculate the relation between the current and max speed, if close to 1 the sails will be at max acceleration
        m_CurrentExternalAccelerationMagnitude = Mathf.Clamp01(m_CurrentShipSpeed / m_MaxShipSpeedMagnitude) * m_MaxExternalAccelerationMagnitude;

        foreach (Cloth cloth in m_SailsList)
        {
            //Since the externalAcceleration is applied in world space, transform the velociy forward vector to world space
            if (m_ShipRigidBody.velocity.normalized != Vector3.zero)
            {
                cloth.externalAcceleration = transform.TransformVector(m_ShipRigidBody.velocity.normalized) * m_CurrentExternalAccelerationMagnitude;
            }
            //If the rb velocity vector is 0 and its a pirate ship
            //(which can be the case for the pirate since it uses the navmesh component to move), use the vector that faces the player from the pirate
            else if (m_PirateController != null)
            {
                cloth.externalAcceleration = (m_PirateController.m_PlayerTransform.position - m_ShipRigidBody.transform.position).normalized * m_CurrentExternalAccelerationMagnitude;
            }

            //Apply a random acceleration based on the current externalAcceleration and a random factor
            cloth.randomAcceleration = cloth.externalAcceleration * m_RandomFactor;
        }

        
    }

}
