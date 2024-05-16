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
    public float m_MouseDirection;
    public PlayerInput m_PlayerInput;


    // Start is called before the first frame update
    void Start()
    {
        m_Camera = GetComponent<Camera>();
        m_PlayerInput = GetComponentInParent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        //if player isnt moving with a controller, move the camera with the mouse, else do it with the controller
        if (m_PlayerInput.actions["Look"].ReadValue<Vector2>().x != 0)
        {
            m_MouseDirection = Input.GetAxis("Mouse X");
            m_Camera.transform.RotateAround(m_ShipTransform.position, Vector3.up, m_MouseDirection * m_Sensitivity * Time.deltaTime);
        }

    }
}
