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
        m_MainCamera.gameObject.SetActive(true);
        m_CannonCamera.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

#if UNITY_ANDROID
        //We dont want the camera to be constantly updating if the user is using mobile
#else
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
#endif
    }

    //This method is used in the change camera button in the mobile canvas
    public void ChangeCameraButton()
    {
        if (m_MainCamera.gameObject.activeSelf && Time.timeScale != 0)
        {
            m_MainCamera.gameObject.SetActive(false);
            m_CannonCamera.gameObject.SetActive(true);
        }
        else if(m_CannonCamera.gameObject.activeSelf && Time.timeScale != 0)
        {
            m_MainCamera.gameObject.SetActive(true);
            m_CannonCamera.gameObject.SetActive(false);
        }
    }
}
