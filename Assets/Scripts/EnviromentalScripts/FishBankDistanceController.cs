using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

//This class teleports fishbanks to designated points in the map if they go too far out of bounds
public class FishBankDistanceController : MonoBehaviour
{
    [SerializeField] private List<Transform> m_TpPositionList = new List<Transform>();
    List<Transform> m_ChildTransforms = new List<Transform>(); //References to the children (fish)
    private Bounds m_MapBounds = new Bounds();

    [Header("CONTROL PARAMS")]
    public float m_TimeBetweenChecks = 5f;
    [SerializeField] private float m_TimeUntilNextCheck = 0.0f;

    private void Start()
    {
        Transform tpListParentTransform = GameObject.FindGameObjectWithTag("BoidsRespawnPoints").transform;

        m_MapBounds = tpListParentTransform.GetComponent<Collider>().bounds;

        //Get all transforms to where the Banks will tp
        for(int i = 0; i < tpListParentTransform.childCount; i++)
        {
            m_TpPositionList.Add(tpListParentTransform.GetChild(i).transform);
        }

        for(int i = 0; i < transform.childCount; i++)
        {
            m_ChildTransforms.Add(transform.GetChild(i).transform);
        }
    
        m_TimeUntilNextCheck = m_TimeBetweenChecks;
    }

    private void Update()
    {
        m_TimeUntilNextCheck -= Time.deltaTime;

        if(m_TimeUntilNextCheck < 0)
        {
            CheckIfBankOutOfBounds();
            m_TimeUntilNextCheck = m_TimeBetweenChecks;
        }
    }

    private void CheckIfBankOutOfBounds()
    {
        Debug.LogWarning("Checking");
        if (!m_MapBounds.Contains(m_ChildTransforms[0].position)) //We check the position of one of the childs since the parent does not move
        {
            Debug.LogWarning("[Teleporting]");
            ResetTransformToBeInChild();
            int randomIndex = Random.Range(0, m_TpPositionList.Count - 1);
            transform.position = m_TpPositionList[randomIndex].position;
        }
    }
    
    private void ResetTransformToBeInChild()
    {
        //Make all fish not have a parent
        foreach(Transform child in m_ChildTransforms)
        {
            child.parent = null;
        }

        transform.position = m_ChildTransforms[0].transform.position;

        //Reattach fish to this object
        foreach (Transform child in m_ChildTransforms)
        {
            child.parent = transform;
        }
    }
}
