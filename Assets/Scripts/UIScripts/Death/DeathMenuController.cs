using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DeathMenuController : MonoBehaviour
{
    public GameObject firstButton;
    public GameObject DeathText;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartGame()
    {
        SingletonOptions.m_Instance.m_SensitivityValue = DeathText.GetComponent<DeathTextController>().m_Sensitivity;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoMainMenu()
    {
        SingletonOptions.m_Instance.m_SensitivityValue = DeathText.GetComponent<DeathTextController>().m_Sensitivity;
        SceneManager.LoadScene(0);
    }
}
