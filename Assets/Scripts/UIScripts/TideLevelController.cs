using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TideLevelController : MonoBehaviour
{
    public int m_TideLevel;

    private float m_Timer = 30f;
    private float m_CurrentTimer;

    public TextMeshProUGUI m_TideLevelText;
    
    // Start is called before the first frame update
    void Start()
    {
        m_CurrentTimer = Time.time + m_Timer;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > m_CurrentTimer)
        {
            m_CurrentTimer = Time.time + m_Timer;
            m_TideLevel++;
        }
        m_TideLevelText.text = m_TideLevel.ToString();
    }

    public void AddTideLevel()
    {
        m_TideLevel++;
    }

}
