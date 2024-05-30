 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;

public class ShipMovement : MonoBehaviour
{
    public PlayerInput m_PlayerInput;

    public Vector2 m_MoveInput = Vector2.zero;

    [Header("VELOCITY VARIABLES")]
    public float m_Acceleration;
    public float m_MaxSpeed;

    [Header("ROTATION VARIABLES")]
    public float m_RotationSpeed;
    public float m_MaxRotationSpeed;
    public float m_RotationDamping;

    [Header("MOBILE INPUTS")]
    public VariableJoystick m_LeftStick;

    private Rigidbody m_Rigidbody;


    // Start is called before the first frame update
    void Start()
    {
        m_PlayerInput = GetComponent<PlayerInput>();
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        Inputs();
        Movement(dt);
        Rotation(dt);
    }

    private void Inputs()
    {
        //TODO QUITAR
        //if (m_LeftStick.enabled && m_LeftStick != null)
        //{
        //    m_MoveInput.x = m_LeftStick.Horizontal;
        //    m_MoveInput.y = m_LeftStick.Vertical;
        //}
        //else
        //{
        //    m_MoveInput = m_PlayerInput.actions["Move"].ReadValue<Vector2>();
        //}

#if UNITY_ANDROID
            m_MoveInput.x = m_LeftStick.Horizontal;
            m_MoveInput.y = m_LeftStick.Vertical;
#else
        m_MoveInput = m_PlayerInput.actions["Move"].ReadValue<Vector2>();
#endif
    }

    private void Movement(float dt)
    {
        float forwardMovement = m_MoveInput.y;

        //NOT NEEDED, FRICTION DOES THE JOB
        //Make the velocity of the ship not go above maximum
        //m_ShipRigidbody.velocity = Vector3.ClampMagnitude(m_ShipRigidbody.velocity, m_MaxSpeed);
        
        Vector3 movement = transform.forward * forwardMovement * m_Acceleration * dt;
        m_Rigidbody.AddForce(movement);

    }

    private void Rotation(float dt)
    {
        float horizontalMovement = m_MoveInput.x;

        //Make the angular velocity of the ship not go above maximum
        m_Rigidbody.angularVelocity = Vector3.ClampMagnitude(m_Rigidbody.angularVelocity, m_MaxRotationSpeed);

        float rotationY = m_RotationSpeed * horizontalMovement * dt;
        m_Rigidbody.AddTorque(0, rotationY, 0);


        //If the player is not trying to rotate, we damp the rotation
        if(horizontalMovement == 0)
        {
            m_Rigidbody.angularVelocity = Vector3.Slerp(m_Rigidbody.angularVelocity, Vector3.zero, m_RotationDamping * dt);
        }

    }

}
