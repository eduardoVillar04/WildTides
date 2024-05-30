using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if(m_DocksVisited >= m_DocksNeededToEnd)
        {
            m_WinText.gameObject.SetActive(true);
            Invoke("ChangeToCreditScene", 5f);
        }
    }

    public void ChangeToCreditScene()
    {
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings-1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<DockController>())
        {
            m_DocksVisited++;
        }
    }

}
