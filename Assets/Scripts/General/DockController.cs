using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class DockController : MonoBehaviour
{
    [Header("DOCK NAME")]
    public string m_DockName;
    
    [Header("DOCKS LIST")]
    public GameObject[] m_DockArray;

    [Header("FIRST DOCK")]
    public bool m_IsFirstDock;
    
    [Header("TIDE LEVEL CONTROLLER")]
    public TideLevelController m_TideLevelController;

    [Header("MISSION LOG TEXT")]
    public TextMeshProUGUI m_MissionText;

    [Header("COMPASS CONTROLLER")]
    public CompassController m_CompassController;


    //In the awake, when the docks are still al active, we get their references
    private void Awake()
    {
        m_DockArray = GameObject.FindGameObjectsWithTag("Dock");
    }

    //We disable the docks that are not the first oen
    private void Start()
    {
        if(!m_IsFirstDock)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            //Select random next dock
            GameObject nextDock = getRandomDock();

            //Update compass target
            m_CompassController.objectiveObjectTransform = nextDock.transform;

            //Update tide level
            m_TideLevelController.AddTideLevel();

            nextDock.SetActive(true);
            gameObject.SetActive(false);
        }

    }

    public GameObject getRandomDock()
    {
        GameObject dock;
    
        while (true)
        {
            int index = UnityEngine.Random.Range(0, m_DockArray.Length);
            dock = m_DockArray[index];
            if(dock!=this.gameObject)
            {
                return dock;
            }
        }
    }
}
