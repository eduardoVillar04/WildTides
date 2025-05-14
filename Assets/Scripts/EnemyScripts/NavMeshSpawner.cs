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
    public GameObject m_FishBank;

    [Header("NUMBER OF SPAWNS")]
    public int m_NumOfBarrelsPerTL;
    public int m_NumOfPiratesPerTL;
    public int m_NumOfTentaclesPerTL;
    public int m_NumOfFishBank;

    [Header("SPAWN VARIABLES")]
    public float m_MaxSpawnDistance;
    public Renderer m_SpawnSurface;

    public Transform m_Player;
    

    private void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player").transform;

        //Generate initial enemies
        GenerateEnemies(1);

    }

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
            //The position coordinates must be inside the spawn surface
            float rx = Random.Range(m_SpawnSurface.bounds.min.x, m_SpawnSurface.bounds.max.x);
            float rz = Random.Range(m_SpawnSurface.bounds.min.z, m_SpawnSurface.bounds.max.x);
            Vector3 v3 = new Vector3(rx, -0.5f, rz);
            pointFound = NavMesh.SamplePosition(v3, out hit, m_MaxSpawnDistance, NavMesh.AllAreas);
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
                randPos = GetValidSpawnPoint();
                GameObject.Instantiate(m_PiratePrefab, randPos, Quaternion.identity);
            }
            for (int j = 0; j < m_NumOfTentaclesPerTL; j++)
            {
                randPos = GetValidSpawnPoint();
                GameObject.Instantiate(m_TentaclePrefab, randPos, Quaternion.identity);
            }
            for (int j = 0; j < m_NumOfBarrelsPerTL; j++)
            {
                randPos = GetValidSpawnPoint();
                //The barrels need to be rotated
                GameObject.Instantiate(m_BarrelPrefab, randPos, Quaternion.Euler(-90, 0, 90));
            }
        }

    }

    //Gets random positions until there is a valid one and returns it
    public Vector3 GetValidSpawnPoint()
    {
        Vector3 spawnPos;
        //Security meassure against infinite loop
        int i = 0;
        do
        {
            i++;
            spawnPos = GetRandPos();
        } while (!CheckIfPathIsValid(spawnPos, m_Player.position) && i<10);

        return spawnPos;
    }


    public bool CheckIfPathIsValid(Vector3 initialPos, Vector3 endPos) 
    {
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(initialPos, endPos, NavMesh.AllAreas, path);

        if (path.status == NavMeshPathStatus.PathComplete)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DestroyAllEnemies()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);   
        }
    }
}
