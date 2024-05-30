using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;
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

    private float m_CameraDirectionX;

    void Start()
    {
        m_Camera = GetComponent<Camera>();
        m_PlayerInput = GetComponentInParent<PlayerInput>();
        m_CameraShake = GetComponentInParent<CameraShake>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_CameraShake.m_IsShaking)
        {
            //Check which controller is being used
            m_CurrentControlScheme = m_PlayerInput.currentControlScheme;

            //Update the sensitivity selected by the player
            m_Sensitivity = SingletonOptions.m_Instance.m_SensitivityValue;

            //Mobile Inputs: If right stick is enabled the camera direction is updated with it
            if (m_RightStick.enabled)
            {
                m_CameraDirectionX = m_RightStick.Horizontal;
            }
            else
            {
                m_CameraDirectionX = m_PlayerInput.actions["Look"].ReadValue<Vector2>().x;
            }
                        

            //new input system values for mouse are too large compared to controller, multiplying by 0.1f gives us the old systems values
            //we only apply this if mouse is being used
            if (m_CurrentControlScheme == "Keyboard&Mouse")
            {
                m_CameraDirectionX *= 0.1f;
            }


            m_Camera.transform.RotateAround(m_ShipTransform.position, Vector3.up, 500 * m_CameraDirectionX * m_Sensitivity * Time.deltaTime);
        }
    }
}
