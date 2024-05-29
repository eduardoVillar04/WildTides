using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;

    public GameObject firstButton;

    public GameObject pauseMenuUI;
    public GameObject pauseMainMenu;
    public GameObject pauseOptionsMenu;
    public GameObject PauseMobileButton;

    public GameObject deathMenu;

    private void Start()
    {
#if UNITY_ANDROID
        PauseMobileButton.SetActive(true);
#endif
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }else 
            {
                Pause();
            }
        }
        if (deathMenu.activeSelf) { gameObject.SetActive(false); }
    }

    public void Resume()
    {
        pauseMainMenu.SetActive(true);
        pauseOptionsMenu.SetActive(false);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void Pause()
    {
        pauseMainMenu.SetActive(true);
        pauseOptionsMenu.SetActive(false);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    public void PauseMobile()
    {
        if (gameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
