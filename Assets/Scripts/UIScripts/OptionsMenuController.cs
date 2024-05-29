using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionsMenuController: MonoBehaviour
{
    public GameObject firstButton;
    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(firstButton);
    }
}
