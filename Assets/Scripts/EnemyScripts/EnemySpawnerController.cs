using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    public GameObject m_ExplosiveBarrelPrefab;
    public GameObject m_PirateEnemyPrefab;

    public float m_SpawnRadius;
    
    // Start is called before the first frame update
    void Start()
    {
        m_SpawnRadius = gameObject.GetComponent<SphereCollider>().radius;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space)) { GenerateEnemies(5); }
    }
    
    public void GenerateEnemies(int TideLevel)
    {
        for (int i = 0; i < TideLevel; i++) 
        {
            for(int j = 0; j < 5; j++) 
            {
                SpawnEnemy(m_ExplosiveBarrelPrefab);
            }
            SpawnEnemy(m_PirateEnemyPrefab);
        }
    }

    void SpawnEnemy(GameObject prefab)
    {
        GameObject spawnObject = GameObject.Instantiate(prefab);
        spawnObject.transform.position = transform.position + RandomOffsetPolar(m_SpawnRadius);
    }
    
    Vector3 RandomOffsetPolar(float radius)
    {
        float distance = CalculateDistance(radius);
        float angleDegrees = Random.Range(0.0f, 360.0f);

        return new Vector3(distance * Mathf.Cos(angleDegrees * Mathf.Deg2Rad), 0, distance * Mathf.Sin(angleDegrees * Mathf.Deg2Rad));
    }

    float CalculateDistance(float radius)
    {
        return radius * Random.Range(0f, 1f);
    }

}
