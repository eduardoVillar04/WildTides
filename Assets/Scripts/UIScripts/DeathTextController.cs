using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTextController : MonoBehaviour
{
    public GameObject m_ScoreMenu;
    private bool m_EndAnimation = false;

    private float m_Timer;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_EndAnimation)
        {
            if (Time.time > m_Timer)
            {
                m_ScoreMenu.SetActive(true);
                gameObject.SetActive(false);
            }
        }
        
    }

    void EndDeathText()
    {
        //Time.timeScale = 0f;
        m_Timer = Time.time + 3f;
        m_EndAnimation = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
