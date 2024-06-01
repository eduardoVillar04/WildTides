using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Purchasing;

public class NavMeshSpawner : MonoBehaviour
{
    public Vector3 m_Position;
    public bool m_R;
    public int num;
    public GameObject m_BarrelPrefab;
    public GameObject m_PiratePrefab;
    public GameObject m_TentaclePrefab;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool GetRandomPosition (out Vector3 result)
    {
        for (int i = 0; i < 100; i++)
        {
            float rx = Random.Range(-380, 380);
            float rz = Random.Range(-470, 470);
            Vector3 v3 = new Vector3(rx, 0, rz);
            NavMeshHit hit;
            if (NavMesh.SamplePosition(v3, out hit, 2f, NavMesh.AllAreas))
            {
                num++;
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
    
    public void GenerateEnemies(int tideLevel)
    {
        
        for (int i = 0; i < tideLevel;i++)
        {
            m_R = GetRandomPosition(out m_Position);
            GameObject spawnObject1 = GameObject.Instantiate(m_PiratePrefab);
            spawnObject1.transform.position = m_Position;
            m_R = GetRandomPosition(out m_Position);
            GameObject spawnObject2 = GameObject.Instantiate(m_TentaclePrefab);
            spawnObject2.transform.position = m_Position;
            for (int j = 0; j < 5; j++)
            {
                m_R = GetRandomPosition(out m_Position);
                GameObject spawnObject3 = GameObject.Instantiate(m_BarrelPrefab);
                spawnObject3.transform.position = m_Position;
            }
        }

    }
    public void DestroyAllEnemies()
    {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Enemy")){
            Destroy(g);
        }
    }
}
