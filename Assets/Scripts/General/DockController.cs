using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockController : MonoBehaviour
{
    [Header("AUDIO")]

    [Header("NEXT DOCK")]
    public Transform m_NextDock;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            //TODO: Give next dock location to compass
            m_NextDock.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

    }
}
