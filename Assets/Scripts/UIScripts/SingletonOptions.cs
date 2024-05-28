using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonOptions : MonoBehaviour
{
    // We store the instance of this class here
    public static SingletonOptions m_Instance;

    public int m_InitialResolutionIndex = 0;
    public int m_ResolutionIndex;
    public int m_QualityIndex = 3;
    public float m_SensitivityValue = 1f;
    public bool m_IsFullScreen = true;
    // To change/read it you can call SingletonSample.m_Instance.m_YourValue = whatevervalue;

    private void Awake()
    {
        // In the Awake method we check if the instance is null
        if (m_Instance == null)
        {
            // If it is null we assign this instance to the instance variable
            m_Instance = this;
            // We also make sure that this object is not destroyed when we load a new scene
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If the instance is not null we destroy this object because we only want one instance of this class
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        if(m_SensitivityValue == 0)
        {
            m_SensitivityValue = 1f;
        }
    }
}
