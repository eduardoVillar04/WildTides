using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build.Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class CannonCamera : MonoBehaviour
{
    [Header("COMPONENTS")]
    public PlayerInput m_PlayerInput;
    public GameObject m_Sails;
    public GameObject m_Sight;
    public CameraShake m_CameraShake;
    public Transform m_Cannon;
    private float m_Sensitivity;
    public float m_ExtraGamepadSens;
    //Makes sure the player cant go further from specified angle
    public float m_yRotationLimit = 88f;
    public string m_CurrentControlScheme;

    //PRUEBAS
   
    private float m_CameraDirectionX;
    private float m_CameraDirectionY;
    private Vector2 rotation;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //while the cannon camera is active, the canno will follow it
        RotateCannon();

        if(!m_CameraShake.m_IsShaking)
        {
            //Check which controller is being used
            m_CurrentControlScheme = m_PlayerInput.currentControlScheme;

            //Update the sensitivity selected by the player
            m_Sensitivity = SingletonOptions.m_Instance.m_SensitivityValue;

            m_CameraDirectionX = m_PlayerInput.actions["Look"].ReadValue<Vector2>().x;
            m_CameraDirectionY = m_PlayerInput.actions["Look"].ReadValue<Vector2>().y;

            //new input system values for mouse are too large compared to controller, multiplying by 0.1f gives us the old systems values
            //we only apply this if mouse is being used
            if (m_CurrentControlScheme == "Keyboard&Mouse")
            {
                m_CameraDirectionX *= 0.1f;
                m_CameraDirectionY *= 0.1f;
            }
            else
            {
                //With gamepad cannonCamera moves slowly compared to mainCamera, so we add a little extra sensitivity;
                m_Sensitivity += m_ExtraGamepadSens;
            }

            rotation.x += m_CameraDirectionX * m_Sensitivity;
            rotation.y += m_CameraDirectionY * m_Sensitivity;
            //We limit the rotation of the y axis
            rotation.y = Mathf.Clamp(rotation.y, -m_yRotationLimit, m_yRotationLimit);

            var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
            var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);

            transform.localRotation = xQuat * yQuat; 
        }

    }

    public void RotateCannon()
    {
        m_Cannon.rotation = transform.rotation;
    }

    private void OnEnable()
    {
        //We make sure the camera has the same rotation as the cannon
        transform.rotation = m_Cannon.rotation;
        m_Sails.SetActive(false);
        m_Sight.SetActive(true);
    }

    private void OnDisable()
    {
        m_Sails.SetActive(true);
        m_Sight.SetActive(false);
    }


}
