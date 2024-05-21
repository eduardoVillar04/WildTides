using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnController : MonoBehaviour
{
    public Transform m_PlayerTransform;
    public Vector3 m_Checkpoint;
    public HealthController m_HealthController;

    private void Start()
    {
        m_PlayerTransform = GetComponent<Transform>();
        m_Checkpoint = m_PlayerTransform.position;
        m_HealthController = GetComponent<HealthController>();
    }

    private void Update()
    {
        if (m_HealthController.m_IsDead) m_PlayerTransform.position = m_Checkpoint; 
    }
}
