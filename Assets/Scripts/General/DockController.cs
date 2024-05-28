using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DockController : MonoBehaviour
{
    [Header("DOCK NAME")]
    public string m_DockName;

    [Header("DOCKS LIST")]
    public GameObject[] m_DockArray;

    [Header("COMPASS CONTROLLER")]
    public CompassController m_CompassController;

    [Header("FIRST DOCK")]
    public bool m_IsFirstDock;

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
            //We set the checkpoint for the boat
            other.gameObject.GetComponent<RespawnController>().m_Checkpoint = transform.position;

            //Select random next dock
            GameObject nextDock = getRandomDock();

            //Update compass target
            m_CompassController.objectiveObjectTransform = nextDock.transform;

            nextDock.SetActive(true);
            gameObject.SetActive(false);
        }

    }

    public GameObject getRandomDock()
    {
        GameObject dock = null;
    
        while (dock!=this.gameObject)
        {
            int index = UnityEngine.Random.Range(0, m_DockArray.Length);
            dock = m_DockArray[index];
        }

        return dock;
    }
}
