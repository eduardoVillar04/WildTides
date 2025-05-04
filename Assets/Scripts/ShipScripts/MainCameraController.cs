using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

public class MainCameraController : MonoBehaviour
{
    public float m_Sensitivity;

    [Header("COMPONENTS")]
    public Camera m_Camera;
    public Transform m_ShipTransform;
    public PlayerInput m_PlayerInput;
    public string m_CurrentControlScheme;

    [Header("CAMERA SHAKE")]
    public CameraShake m_CameraShake;

    [Header("MOBILE INPUTS")]
    public VariableJoystick m_RightStick;

    //Rotating vars
    private float m_CameraDirectionX;
    private float m_InitialShipYPos;

    //Mantain pos relative to ship vars
    private Vector3 m_LastShipPos = Vector3.zero;

    void Start()
    {
        m_Camera = GetComponent<Camera>();
        m_CameraShake = GetComponent<CameraShake>();
        m_LastShipPos = m_ShipTransform.position;
        m_InitialShipYPos = m_ShipTransform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        MantainRelativePositionToShip();

        if (!m_CameraShake.m_IsShaking)
        {
            //Check which controller is being used
            m_CurrentControlScheme = m_PlayerInput.currentControlScheme;

            //Update the sensitivity selected by the player
            m_Sensitivity = SingletonOptions.m_Instance.m_SensitivityValue;

            //TODO QUITAR
            //Mobile Inputs: If right stick is enabled the camera direction is updated with it
            //if (m_RightStick.enabled)
            //{
            //    m_CameraDirectionX = m_RightStick.Horizontal;
            //}
            //else
            //{
            //    m_CameraDirectionX = m_PlayerInput.actions["Look"].ReadValue<Vector2>().x;
            //}

#if UNITY_ANDROID
            m_CameraDirectionX = m_RightStick.Horizontal;
            
#else
            m_CameraDirectionX = m_PlayerInput.actions["Look"].ReadValue<Vector2>().x;
            
#endif

            //new input system values for mouse are too large compared to controller, multiplying by 0.1f gives us the old systems values
            //we only apply this if mouse is being used
            if (m_CurrentControlScheme == "Keyboard&Mouse" || m_CurrentControlScheme == null)
            {
                m_CameraDirectionX *= 0.1f;
            }

            RotateCamera();

        }
    }

    private void RotateCamera()
    {
        //Rotate the camera using the initial yPos of the ship, to not cause wobbling
        Vector3 rotationPos = new Vector3(m_ShipTransform.position.x, m_InitialShipYPos, m_ShipTransform.position.z);
        m_Camera.transform.RotateAround(rotationPos, Vector3.up, 500 * m_CameraDirectionX * m_Sensitivity * Time.deltaTime);
    }

    private void MantainRelativePositionToShip()
    {
        Vector3 shipMovement = m_ShipTransform.position - m_LastShipPos;
        transform.position += shipMovement;
        m_LastShipPos = m_ShipTransform.position;
    }

    //Save the position of the ship when the camera is disabled as the last position, so that it moves to the correst position when enabled
    private void OnDisable()
    {
        m_LastShipPos = m_ShipTransform.position;
    }
}
