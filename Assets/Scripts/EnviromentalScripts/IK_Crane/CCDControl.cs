using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class CCDControl : MonoBehaviour
{
    // ATTRIBUTES
    [Header("IK ATTRIBUTES")]

    public bool m_IsPureCCD = true;  // Pure CCD switch
    public bool m_IsSearchEnabled = false; // Switch controlling active search
    public float m_searchSpeed = 1.0f;  // Search speed
    public GameObject m_ChestTarget = null;  // Target
    public Transform[] m_Parts;                                // Object collection

    private Dictionary<Transform, Quaternion> m_DefaultOrientations; // Default orientations
    private Dictionary<Transform, Vector3> m_ChildlastPositions;  // Child last positions
    private Dictionary<Transform, Vector3> m_RotationSpeeds;      // RotationSpeeds


    [Header("HOOK")]
    public GameObject m_HookObject;
    public GameObject m_LastObject;
    public float m_HookSpeed = 2f;
    public GameObject m_ChestObject;
    public GameObject m_PlayerShip;
    public bool m_HasChest = false;
    public float m_MaxTimeWithChest = 5f; // Maximum time that the crane have to unload the chest, if the crane bugs
    [SerializeField] private float m_TimeWithChest = 0f;

    public float m_MinimumExtraHeight;
    public float m_MaximumExtraHeight;
    [SerializeField] private float m_ExtraHeight = 0;
    public Transform m_UnloadPosition;

    [Header("REPOSE")]

    public Transform m_ReposePosition;

    public GameObject m_BaseObject;

    public GameObject m_AllCrane;

    public  Vector3 m_CurrentTarget;

    public float m_CurrentSpeed;
    public float m_CraneSpeedWithLoad;

    public BaseController m_BaseController;
    public DockController m_DockController;

    public enum CraneState
    {
        IDLE,
        SEARCHING_SHIP,
        PICKING_CHEST,
        GOING_WITH_CHEST_TO_UNLOAD,
        UNLOADING_CHEST,
        GOING_REPOSE,
        RETURNING_HOOK_WITHOUT_CHEST
    }

    public CraneState m_CurrentState = CraneState.GOING_REPOSE;

    // Use this for initialization
    void Start()
    {
        m_Parts = GetComponentsInChildren<Transform>();
        // Default orientations, child last positions and rotation speeds
        m_DefaultOrientations = new Dictionary<Transform, Quaternion>();
        m_ChildlastPositions = new Dictionary<Transform, Vector3>();
        m_RotationSpeeds = new Dictionary<Transform, Vector3>();
        foreach (Transform part in m_Parts)
        {
            m_DefaultOrientations.Add(part, part.rotation);
            m_ChildlastPositions.Add(part, part.childCount > 0 ? part.GetChild(0).position : Vector3.zero);
            m_RotationSpeeds.Add(part, Vector3.zero);
        }
        ChangeState(CraneState.IDLE);
    }

    public void ChangeState(CraneState state)
    {
        switch (state)
        {
            case CraneState.IDLE:
                m_CurrentState = CraneState.IDLE;
                m_IsSearchEnabled = false;
                m_BaseController.m_ActualTarget = m_ReposePosition.transform;
                break;
            case CraneState.SEARCHING_SHIP:
                m_CurrentState = CraneState.SEARCHING_SHIP;
                m_IsSearchEnabled = true;
                m_HasChest = false;
                m_CurrentSpeed = m_searchSpeed;
                m_BaseController.m_ActualTarget = m_ChestTarget.transform;
                m_ExtraHeight = GetHigherTarget();
                break;
            case CraneState.PICKING_CHEST:
                m_CurrentState = CraneState.PICKING_CHEST;
                m_HasChest = false;
                break;
            case CraneState.GOING_WITH_CHEST_TO_UNLOAD:
                m_CurrentState = CraneState.GOING_WITH_CHEST_TO_UNLOAD;
                m_CurrentSpeed = m_CraneSpeedWithLoad;
                m_BaseController.m_ActualTarget = m_ReposePosition.transform;
                m_ChestObject.SetActive(true);
                m_CurrentTarget = m_ReposePosition.position;
                m_TimeWithChest = m_MaxTimeWithChest;
                if(m_DockController != null) { m_DockController.ChangeDock(m_PlayerShip); }
                break;
            case CraneState.UNLOADING_CHEST:
                m_CurrentState = CraneState.UNLOADING_CHEST;
                m_BaseController.m_ActualTarget = m_ReposePosition.transform;
                break;
            case CraneState.GOING_REPOSE:
                m_CurrentState = CraneState.GOING_REPOSE;
                m_ChestObject.SetActive(false);
                m_HasChest = false;
                m_CurrentTarget = m_ReposePosition.position;
                m_CurrentSpeed = m_searchSpeed;
                m_BaseController.m_ActualTarget = m_ReposePosition.transform;
                break;
            case CraneState.RETURNING_HOOK_WITHOUT_CHEST:
                m_CurrentState = CraneState.RETURNING_HOOK_WITHOUT_CHEST;
                m_ChestObject.SetActive(false);
                m_HasChest = false; 
                m_CurrentTarget = m_LastObject.transform.position;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    { 
        switch(m_CurrentState)
        {
            case CraneState.IDLE:
                m_HookObject.transform.position = m_LastObject.transform.position;
                break;
            case CraneState.SEARCHING_SHIP:
                m_HookObject.transform.position = m_LastObject.transform.position;
                m_CurrentTarget = new Vector3(m_ChestTarget.transform.position.x, m_ChestTarget.transform.position.y + m_ExtraHeight, m_ChestTarget.transform.position.z);
                if (Vector3.Distance(m_LastObject.transform.position, m_CurrentTarget) < 1f)
                {
                    ChangeState(CraneState.PICKING_CHEST);
                }
                CalculateIK();
                m_BaseController.MoveBase();
                break;
            case CraneState.PICKING_CHEST:
                if(!m_HasChest)
                {
                    m_HookObject.transform.position = Vector3.Lerp(m_HookObject.transform.position, m_ChestTarget.transform.position, Time.deltaTime * m_HookSpeed);
                    if (Vector3.Distance(m_HookObject.transform.position, m_ChestTarget.transform.position) < 0.1f) { m_HasChest = true; m_ChestObject.SetActive(true); }
                }
                else
                {
                    m_HookObject.transform.position = Vector3.Lerp(m_HookObject.transform.position, m_LastObject.transform.position, Time.deltaTime * m_HookSpeed);
                    if (Vector3.Distance(m_HookObject.transform.position, m_LastObject.transform.position) < 0.05f) { ChangeState(CraneState.GOING_WITH_CHEST_TO_UNLOAD); }
                }
                break;
            case CraneState.GOING_WITH_CHEST_TO_UNLOAD:
                m_HookObject.transform.position = m_LastObject.transform.position;
                m_BaseController.MoveBase();
                CalculateIK();
                if (Vector3.Distance(m_LastObject.transform.position, m_CurrentTarget) < 0.5f)
                {
                    ChangeState(CraneState.UNLOADING_CHEST);
                }
                m_TimeWithChest -= Time.deltaTime;
                if(m_TimeWithChest <= 0f) { ChangeState(CraneState.GOING_REPOSE); }
                break;
            case CraneState.UNLOADING_CHEST:
                if (m_HasChest)
                {
                    m_HookObject.transform.position = Vector3.Lerp(m_HookObject.transform.position, m_UnloadPosition.transform.position, Time.deltaTime * m_HookSpeed);
                    if (Vector3.Distance(m_HookObject.transform.position, m_UnloadPosition.transform.position) < 0.1f) { m_HasChest = false; m_ChestObject.SetActive(false); }
                }
                else
                {
                    m_HookObject.transform.position = Vector3.Lerp(m_HookObject.transform.position, m_LastObject.transform.position, Time.deltaTime * m_HookSpeed);
                    if (Vector3.Distance(m_HookObject.transform.position, m_LastObject.transform.position) < 0.05f) { ChangeState(CraneState.IDLE); }
                }
                break;
            case CraneState.GOING_REPOSE:
                m_HookObject.transform.position = m_LastObject.transform.position;
                CalculateIK();
                m_BaseController.MoveBase();
                if (Vector3.Distance(m_HookObject.transform.position, m_ReposePosition.position) < 0.1f) { ChangeState(CraneState.IDLE); }
                break;
            case CraneState.RETURNING_HOOK_WITHOUT_CHEST:
                m_HookObject.transform.position = Vector3.Lerp(m_HookObject.transform.position, m_LastObject.transform.position, Time.deltaTime * m_HookSpeed);
                if (Vector3.Distance(m_HookObject.transform.position, m_LastObject.transform.position) < 0.05f) { ChangeState(CraneState.GOING_REPOSE); }
                break;
        }
    }

    private void CalculateIK()
    {
        foreach (Transform part in m_Parts)
        {
            // Current direction
            Vector3 currentDirection =
              m_IsPureCCD ?
              (part.childCount == 0 ? part.forward : (m_Parts[m_Parts.Length - 1].position - part.position)) :
              part.forward;
            Vector3 goalDirection = m_CurrentTarget - part.position;
            // Goal orientation
            Quaternion goalOrientation =
              m_IsSearchEnabled ?
              Quaternion.FromToRotation(currentDirection, goalDirection) * part.rotation :
              m_DefaultOrientations[part];
            // New orientation
            Quaternion newOrientation = Quaternion.Slerp(part.rotation, goalOrientation, m_CurrentSpeed * Time.deltaTime);
            // Update
            part.rotation = newOrientation;
            // Child last position storage
            m_ChildlastPositions[part] = part.childCount > 0 ? part.GetChild(0).position : Vector3.zero;
        }
    }

    private float GetHigherTarget()
    {
        return Random.Range(m_MinimumExtraHeight, m_MaximumExtraHeight);
    }
}