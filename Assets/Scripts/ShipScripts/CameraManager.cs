using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    [Header("CAMERAS")]
    public Camera m_MainCamera;
    public Camera m_CannonCamera;

    public PlayerInput m_PlayerInput;

    // Start is called before the first frame update
    void Start()
    {
        m_PlayerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_PlayerInput.actions["ChangeCamera"].IsPressed())
        {
            m_MainCamera.gameObject.SetActive(false);
            m_CannonCamera.gameObject.SetActive(true);
        }
        else
        {
            m_MainCamera.gameObject.SetActive(true);
            m_CannonCamera.gameObject.SetActive(false);
        }
    }
}
