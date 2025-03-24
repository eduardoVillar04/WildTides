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

    [Header("SPAWNER")]
    public NavMeshSpawner m_NavMeshSpawner;

    [Header("MISSION LOG TEXT")]
    public TextMeshProUGUI m_MissionText;

    [Header("COMPASS CONTROLLER")]
    public CompassController m_CompassController;

    [Header("SHIP DOCK SCORE")]
    public DockScore m_DockScore;

    [Header("AUDIO")]
    public AudioClip m_ArrivingSound;
    //In the awake, when the docks are still al active, we get their references
    private void Awake()
    {
        m_DockArray = GameObject.FindGameObjectsWithTag("Dock");
        m_NavMeshSpawner = FindObjectOfType<NavMeshSpawner>();
        m_TideLevelController =  FindObjectOfType<TideLevelController>();
        m_CompassController = FindObjectOfType<CompassController>();
        m_DockScore = FindObjectOfType<DockScore>();
    }

    //We disable the docks that are not the first oen
    private void Start()
    {
        //Deactivate every non first dock
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

            //Change the name of the port in mission log
            m_MissionText.text = "Go to: " + nextDock.GetComponent<DockController>().m_DockName;

            //Update compass target
            m_CompassController.objectiveObjectTransform = nextDock.transform;

            //Update dock visited count
            m_DockScore.m_NumDocksVisited++;

            //Audio
            SoundEffectsManager.instance.PlaySoundFXClip(m_ArrivingSound, transform, 0.6f);

            //Update tide level
            m_TideLevelController.AddTideLevel();

            if(!SingletonOptions.m_Instance.m_HardModeOn)
            {
                other.GetComponent<HealthController>().heal(1);
            }

            nextDock.SetActive(true);
            gameObject.SetActive(false);

            m_NavMeshSpawner.DestroyAllEnemies();
            m_NavMeshSpawner.GenerateEnemies(m_TideLevelController.m_TideLevel);
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
