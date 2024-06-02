using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomFirstDock : MonoBehaviour
{
    public GameObject[] m_DockArray;
    public TextMeshProUGUI m_MissionText;
    public CompassController m_CompassController;

    private void Awake()
    {
        m_CompassController = FindAnyObjectByType<CompassController>();
        m_DockArray = GameObject.FindGameObjectsWithTag("Dock");

        int randNum = UnityEngine.Random.Range(0, m_DockArray.Length);

        DockController randDock = m_DockArray[randNum].GetComponent<DockController>();

        randDock.m_IsFirstDock = true;

        m_MissionText.text = "Go to: " + randDock.m_DockName;
        m_CompassController.objectiveObjectTransform = randDock.transform;
    }
}
