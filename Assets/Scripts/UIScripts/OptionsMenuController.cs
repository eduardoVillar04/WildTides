using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionsMenuController: MonoBehaviour
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
}
