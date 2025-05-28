using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneRangeDetection : MonoBehaviour
{
    public CCDControl m_CraneController;
    
    // Start is called before the first frame update
    void Start()
    {
        m_CraneController = GetComponentInChildren<CCDControl>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // If the crane in repose or is searching for a ship, set the target to the player and start searching for it
            if(m_CraneController.m_CurrentState == CCDControl.CraneState.IDLE || 
                m_CraneController.m_CurrentState == CCDControl.CraneState.SEARCHING_SHIP ||
                m_CraneController.m_CurrentState == CCDControl.CraneState.GOING_REPOSE)
            {
                m_CraneController.m_ChestTarget = other.GetComponent<CraneShipController>().m_ChestShip;
                m_CraneController.m_PlayerShip = other.gameObject;
                m_CraneController.ChangeState(CCDControl.CraneState.SEARCHING_SHIP);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            // If the crane is searching for a ship go to repose
            if(m_CraneController.m_CurrentState == CCDControl.CraneState.SEARCHING_SHIP)
            {
                m_CraneController.ChangeState(CCDControl.CraneState.GOING_REPOSE);
            }

            // If the crane is picking going to pick up a chest, cancels the picking up animation and then will go repose
            if (m_CraneController.m_CurrentState == CCDControl.CraneState.PICKING_CHEST && !m_CraneController.m_HasChest)
            {
                m_CraneController.ChangeState(CCDControl.CraneState.RETURNING_HOOK_WITHOUT_CHEST);
            }
        }
    }
}
