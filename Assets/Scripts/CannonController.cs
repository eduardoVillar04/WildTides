using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CannonController : MonoBehaviour
{
    [Header("ART")]
    public GameObject m_GustOfWind;

    [Header("INPUT VARIABLES")]
    public bool m_ChangeCameraIsPressed;
    public bool m_ShootIsPressed;

    [Header("CONTROL VARIABLES")]
    public float m_ShootCooldown;
    private float m_ShootColdowntimer;


    private PlayerInput m_PlayerInput;

    // Start is called before the first frame update
    void Start()
    {
        m_PlayerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Inputs()
    {
        m_ChangeCameraIsPressed = m_PlayerInput.actions["ChangeCamera"].IsPressed();

    }
}
