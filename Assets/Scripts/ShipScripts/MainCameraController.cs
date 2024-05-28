using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class MainCameraController : MonoBehaviour
{
    public Camera m_Camera;
    public float m_Sensitivity;

    public Transform m_ShipTransform;
    public float m_CameraDirection;
    public PlayerInput m_PlayerInput;

    public CameraShake m_CameraShake;

    // Start is called before the first frame update
    void Start()
    {
        m_Camera = GetComponent<Camera>();
        m_PlayerInput = GetComponentInParent<PlayerInput>();
        m_CameraShake = GetComponentInParent<CameraShake>();
    }

    // Update is called once per frame
    void Update()
    {
        //Update the sensitivity selected by the player
        m_Sensitivity = SingletonOptions.m_Instance.m_SensitivityValue;

        //if player isnt moving with a controller, move the camera with the mouse, else do it with the controller
        //also, if the camera is shaking we dont allow it to move, since it causes problems
        if (m_PlayerInput.actions["Look"].ReadValue<Vector2>().x != 0 && !m_CameraShake.m_IsShaking)
        {
            
            m_CameraDirection = m_PlayerInput.actions["Look"].ReadValue<Vector2>().x;

            m_CameraDirection = Input.GetAxis("Mouse X");

            //TODO ADD CONTROLLER SUPPORT
            //We clamp the value between -1 / 1 to have consistency between platforms
            //m_CameraDirection = Mathf.Clamp(m_CameraDirection, -1, 1);

            //Debug.Log(m_CameraDirection.ToString());

            m_Camera.transform.RotateAround(m_ShipTransform.position, Vector3.up, 500 * m_CameraDirection * m_Sensitivity * Time.deltaTime);
        }
        //m_Sensitivity = Singleton.player_Sensitivity;
    }
}
