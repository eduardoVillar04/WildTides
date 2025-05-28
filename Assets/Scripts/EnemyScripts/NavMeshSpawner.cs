using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions.Must;
using UnityEngine.Purchasing;

public class NavMeshSpawner : MonoBehaviour
{
    [Header("ENEMY PREFABS")]
    public GameObject m_BarrelPrefab;
    public GameObject m_PiratePrefab;
    public GameObject m_TentaclePrefab;
    public GameObject m_FishBank;

    [Header("NUMBER OF SPAWNS")]
    public int m_NumOfBarrelsPerTL;
    public int m_NumOfPiratesPerTL;
    public int m_NumOfTentaclesPerTL;
    public int m_NumOfFishBankPerTL;

    [Header("SPAWN VARIABLES")]
    public float m_MaxSpawnDistance;
    public Renderer m_SpawnSurface;
    public float m_SpawningYpos = 0;
    private NavMeshTriangulation m_NavmeshTriangulation;
    public Transform m_Player;

    private List<Vector3> m_SpawnPosList = new List<Vector3>();

    private void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player").transform;

        //Generate initial enemies
        GenerateEnemies(1);
        m_NavmeshTriangulation = NavMesh.CalculateTriangulation();
    }

    private void Update()
    {

#if UNITY_EDITOR
        //DEBUG GENERATE ENEMIES
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateEnemies(3);
        }
#endif

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
            float rz = Random.Range(m_SpawnSurface.bounds.min.z, m_SpawnSurface.bounds.max.z);
            Vector3 v3 = new Vector3(rx, m_SpawningYpos, rz);
            pointFound = NavMesh.SamplePosition(v3, out hit, m_MaxSpawnDistance, NavMesh.AllAreas);
        } while (!pointFound);

        return hit.position;
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
        } while (!CheckIfPathIsValid(spawnPos, m_Player.position) && i < 10);

        return spawnPos;
    }

    public void GenerateEnemies(int tideLevel)
    {
        //m_SpawnPosList.Clear();

        for (int i = 0; i < tideLevel; i++)
        {
            Vector3 randPos = Vector3.zero;
            for (int j = 0; j < m_NumOfPiratesPerTL; j++)
            {
                randPos = GetValidSpawnPoint();
                GameObject newEntity = EntitiesPoolManager.instance.GetEntityFromPool(EntityType.PIRATE);
                newEntity.transform.position = randPos;
            }
            for (int j = 0; j < m_NumOfTentaclesPerTL; j++)
            {
                randPos = GetValidSpawnPoint();
                GameObject newEntity = EntitiesPoolManager.instance.GetEntityFromPool(EntityType.TENTACLE);
                newEntity.transform.position = randPos;
            }
            for (int j = 0; j < m_NumOfBarrelsPerTL; j++)
            {
                randPos = GetValidSpawnPoint();
                GameObject newEntity = EntitiesPoolManager.instance.GetEntityFromPool(EntityType.EXPLOSIVE_BARREL);
                newEntity.transform.position = randPos;
                //The barrels need to be rotated
                newEntity.transform.rotation = Quaternion.Euler(-90, 0, 90);
            }
            for (int j = 0; j < m_NumOfFishBankPerTL; j++)
            {
                if((i * m_NumOfFishBankPerTL) <= EntitiesPoolManager.instance.m_MaxNumOfFishBanks) //Make sure there are not more than the allow num of banks
                {
                    randPos = GetValidSpawnPoint();
                    GameObject newEntity = EntitiesPoolManager.instance.GetEntityFromPool(EntityType.FISH_BANK);
                    newEntity.transform.position = randPos;

                }
            }
        }
    }



    public bool CheckIfPathIsValid(Vector3 initialPos, Vector3 endPos) 
    {
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(initialPos, endPos, NavMesh.AllAreas, path);

        return path.status == NavMeshPathStatus.PathComplete;
    }

    public void ReturnAllEnemiesToPools()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            EntitiesPoolManager.instance.ReturnEntityToPool(enemy);
        }
    }
}
