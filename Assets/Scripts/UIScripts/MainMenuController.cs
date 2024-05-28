using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject firstButton;

    [Header("AUDIO")]
    public AudioClip m_ClickSound;

    private void OnEnable()
    {
        //Audio
        SoundEffectsManager.instance.PlaySoundFXClip(m_ClickSound, transform, 0.99f);
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    public void PlayGame ()
    {
        //Audio
        SoundEffectsManager.instance.PlaySoundFXClip(m_ClickSound, transform, 0.99f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame ()
    {
        //Audio
        SoundEffectsManager.instance.PlaySoundFXClip(m_ClickSound, transform, 0.99f);
        Debug.Log("Quit");  
        Application.Quit();
    }
}
