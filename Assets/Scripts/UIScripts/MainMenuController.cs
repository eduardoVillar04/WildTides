using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject firstButton;
    public GameObject m_HardModeOnImage;
    public GameObject m_HardModeOffImage;

    public GameObject m_LoadingBanner;

    private void Start()
    {
        if (SingletonOptions.m_Instance.m_HardModeOn)
        {
            m_HardModeOffImage.SetActive(false);
            m_HardModeOnImage.SetActive(true);
        }
        else
        {
            m_HardModeOffImage.SetActive(true);
            m_HardModeOnImage.SetActive(false);
        }
    }
    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    public void PlayGame ()
    {
        m_LoadingBanner.SetActive(true);
        this.transform.parent.gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame ()
    {
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1);
        Debug.Log("Quit");  
        //Application.Quit();
    }
    public void ChangeDifficulty()
    {
        SingletonOptions.m_Instance.m_HardModeOn = !SingletonOptions.m_Instance.m_HardModeOn;
        if (SingletonOptions.m_Instance.m_HardModeOn)
        {
            m_HardModeOffImage.SetActive(false);
            m_HardModeOnImage.SetActive(true);
            return;
        }
        m_HardModeOffImage.SetActive(true);
        m_HardModeOnImage.SetActive(false);
    }
}
