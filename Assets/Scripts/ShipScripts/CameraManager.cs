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
        //We change the camera if the action button is pressed and if the game is not paused
        if (m_PlayerInput.actions["ChangeCamera"].IsPressed() && Time.timeScale != 0)
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
