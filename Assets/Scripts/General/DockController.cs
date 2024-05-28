using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockController : MonoBehaviour
{
    [Header("DOCK NAME")]
    public string m_DockName;

    [Header("NEXT DOCK")]
    public Transform m_NextDock;

    [Header("COMPASS CONTROLLER")]
    public CompassController m_CompassController;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            //We set the checkpoint for the boat
            other.gameObject.GetComponent<RespawnController>().m_Checkpoint = transform.position;

            //Update compass target
            m_CompassController.objectiveObjectTransform = m_NextDock;

            m_NextDock.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

    }
}
