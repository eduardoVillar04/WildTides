using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForAndroidScript : MonoBehaviour
{
    public Canvas m_AndroidCanvas;

    private void Awake()
    {
        //If the user is using android, the normal canvas is disabled and the android canvas is enabled
#if UNITY_ANDROID

        m_AndroidCanvas.gameObject.SetActive(true);
        this.gameObject.SetActive(false);

#endif
    }
}
