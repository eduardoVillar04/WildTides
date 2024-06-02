using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CreditScript : MonoBehaviour
{
    public GameObject m_Credits;

    public PlayerInput m_PlayerInput;

    // Start is called before the first frame update
    void Start()
    {
        m_Credits.gameObject.SetActive(true);
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_PlayerInput.actions["Pause"].WasPressedThisFrame())
        {
            Debug.Log("Quit");
            Application.Quit();
        }

    }
}
