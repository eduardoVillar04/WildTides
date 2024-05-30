using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MobileDockCounter : MonoBehaviour
{
    public int m_DocksVisited;
    public int m_DocksNeededToEnd;
    public TextMeshProUGUI m_WinText;

    private void Start()
    {
        m_WinText.enabled = true;
        m_DocksVisited = 0;
    }

    private void Update()
    {
        if(m_DocksVisited == m_DocksNeededToEnd)
        {
            Time.timeScale = 0.0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<DockController>())
        {
            m_DocksVisited++;
        }
    }

}
