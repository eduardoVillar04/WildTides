using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Purchasing;

public class NavMeshSpawner : MonoBehaviour
{
    public Vector3 m_Position;
    public bool m_R;
    public int num;

    [Header("ENEMY PREFABS")]
    public GameObject m_BarrelPrefab;
    public GameObject m_PiratePrefab;
    public GameObject m_TentaclePrefab;

    [Header("NUMBER OF SPAWNS")]
    public int m_NumOfBarrelsPerTL;
    public int m_NumOfPiratesPerTL;
    public int m_NumOfTentaclesPerTL;

    [Header("SPAWN VARIABLES")]
    public float m_MaxSpawnDistance;

    private Transform m_Player;

    private void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player").transform;

        //Generate initial enemies
        GenerateEnemies(1);
    }


    private void Update()
    {
        //TODO QUITAR (DEBUG)
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GenerateEnemies(5);
        }
    }

    //TODO QUITAR (OLD)
    //private bool GetRandomPosition (out Vector3 result)
    //{
    //    for (int i = 0; i < 100; i++)
    //    {
    //        float rx = Random.Range(-380, 380);
    //        float rz = Random.Range(-470, 470);
    //        Vector3 v3 = new Vector3(rx, -0.5f, rz);
    //        NavMeshHit hit;
    //        if (NavMesh.SamplePosition(v3, out hit, m_MaxSpawnDistance, NavMesh.AllAreas))
    //        {
    //            num++;
    //            result = hit.position;
    //            return true;
    //        }
    //    }
    //    result = Vector3.zero;
    //    return false;
    //}

    /// <summary>
    /// Returns random position from navmesh
    /// </summary>
    /// <returns></returns>
    private Vector3 GetRandPos()
    {
        bool pointFound = false;
        NavMeshHit hit;
        
        do
        {
            float rx = Random.Range(-380, 380);
            float rz = Random.Range(-470, 470);
            Vector3 v3 = new Vector3(rx, -0.5f, rz);
            pointFound = NavMesh.SamplePosition(v3, out hit, 10f, NavMesh.AllAreas);
        } while (!pointFound);

        return hit.position;
    }
    
    public void GenerateEnemies(int tideLevel)
    {    
        for (int i = 0; i < tideLevel;i++)
        {
            Vector3 randPos = Vector3.zero;
            for (int j = 0; j < m_NumOfPiratesPerTL; j++)
            {
                do
                {
                    randPos = GetRandPos();
                } while (!CheckIfPathIsValid(randPos, m_Player.position));

                GameObject.Instantiate(m_PiratePrefab, randPos, Quaternion.identity);
            }
            for (int j = 0; j < m_NumOfTentaclesPerTL; j++)
            {
                do
                {
                    randPos = GetRandPos();
                } while (!CheckIfPathIsValid(randPos, m_Player.position)) ;

                GameObject.Instantiate(m_TentaclePrefab, randPos, Quaternion.identity);
            }
            for (int j = 0; j < m_NumOfBarrelsPerTL; j++)
            {
                do
                {
                    randPos = GetRandPos();
                } while (!CheckIfPathIsValid(randPos, m_Player.position));

                GameObject.Instantiate(m_BarrelPrefab, randPos, Quaternion.identity);
            }
        }

    }

    //Gets random positions until there is a valid one and returns it
    public Vector3 GetValidSpawnPoint()
    {
        Vector3 spawnPos;
        do
        {
            spawnPos = GetRandPos();
        } while (!CheckIfPathIsValid(spawnPos, m_Player.position));

        return spawnPos;
    }

    public bool CheckIfPathIsValid(Vector3 initialPos, Vector3 endPos) 
    {
        return NavMesh.CalculatePath(initialPos, endPos, NavMesh.AllAreas, new NavMeshPath());
    }
    public void DestroyAllEnemies()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")){
            Destroy(enemy);
        }
    }
}
