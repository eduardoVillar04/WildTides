using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHealthController : MonoBehaviour
{
    public HealthController m_HealthController;
    public int m_Health;

    public GameObject m_Heart1;
    public GameObject m_Heart2;
    public GameObject m_Heart3;

    public GameObject m_DeathMenu;


    // Update is called once per frame
    void Update()
    {
        m_Health = m_HealthController.m_HealthPoints;
        if (Input.GetKeyUp(KeyCode.P)) { m_Health = 0; }
        switch (m_Health)
        {
            case 0:
                m_Heart1.SetActive(false);
                m_Heart2.SetActive(false);
                m_Heart3.SetActive(false);
                PlayerIsDead();
                break;
            case 1:
                m_Heart1.SetActive(true);
                m_Heart2.SetActive(false);
                m_Heart3.SetActive(false);
                break;
            case 2:
                m_Heart1.SetActive(true);
                m_Heart2.SetActive(true);
                m_Heart3.SetActive(false);
                break;
            case 3:
                m_Heart1.SetActive(true);
                m_Heart2.SetActive(true);
                m_Heart3.SetActive(true);
                break;
        }
    }

    private void PlayerIsDead()
    {
        m_DeathMenu.SetActive(true);
    }
}
