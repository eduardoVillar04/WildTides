using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public TextMeshProUGUI m_ScoreText;
    public GameObject m_TideLevelObject;
    private TideLevelController m_TideLevelController;
    public GameObject m_FinalDeathMenu;

    public DockScore m_DockScore;
    public int m_FinalScore;

    private bool m_IsShowingScore;
    private int m_CurrentScoreNumber = 0;
    private int m_TideLevel;

    private float m_CurrentTimeNumber;
    private float m_ExtraTime = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        m_TideLevelController = m_TideLevelObject.GetComponent<TideLevelController>();
        m_DockScore = FindObjectOfType<DockScore>();
    }

    // Update is called once per frame
    void Update()
    {
        //TODO QUITAR (OLD - USING TIDE LEVEL INSTEAD OF DOCKS VISITED)
        //if (m_IsShowingScore)
        //{
        //    if (Time.time > m_CurrentTimeNumber)
        //    {
        //        m_CurrentTimeNumber = Time.time + m_ExtraTime;
        //        if (m_CurrentScoreNumber != m_TideLevel)
        //        {
        //            m_CurrentScoreNumber++;
        //            m_ScoreText.text = m_CurrentScoreNumber.ToString();
        //        }
        //        else
        //        {
        //            gameObject.GetComponent<Animator>().SetTrigger("2Animation");
        //            m_FinalDeathMenu.SetActive(true);
        //        }
        //    }
        //}

        if (m_IsShowingScore)
        {
            if (Time.time > m_CurrentTimeNumber)
            {
                m_CurrentTimeNumber = Time.time + m_ExtraTime;
                if (m_CurrentScoreNumber != m_FinalScore)
                {
                    m_CurrentScoreNumber++;
                    m_ScoreText.text = m_CurrentScoreNumber.ToString();
                }
                else
                {
                    gameObject.GetComponent<Animator>().SetTrigger("2Animation");
                    m_FinalDeathMenu.SetActive(true);
                }
            }
        }
    }

    public void ShowScore()
    {
        m_ScoreText.text = 0.ToString();
        m_TideLevel = m_TideLevelController.m_TideLevel;
        m_TideLevelObject.SetActive(false);
        m_IsShowingScore = true;

        //New
        m_FinalScore = m_DockScore.m_NumDocksVisited * 10;

        m_CurrentTimeNumber = Time.time + m_ExtraTime;
    }

}
