using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitiesPoolManager : MonoBehaviour
{
    public static EntitiesPoolManager instance;

    [Header("ENTITIES PREFABS")]
    public GameObject m_PiratePrefab = null;
    public GameObject m_TentaclePrefab = null;
    public GameObject m_ExplosiveBarrelPrefab = null;
    public GameObject m_FishBankPrefab = null;

    [Header("POOL SIZES")]
    public int m_PiratesPoolSize = 0;
    public int m_TentaclesPoolSize = 0;
    public int m_ExplosiveBarrelPoolSize = 0;
    public int m_FishBankPoolSize = 0;

    [Header("CONTROL PARAMS")]
    public int m_MaxNumOfFishBanks = 0; //Never spawn more that these number of banks, for performance

    //Pools
    public List<GameObject> m_PiratePool = new List<GameObject>();
    public List<GameObject> m_TentaclePool = new List<GameObject>();
    public List<GameObject> m_ExplosiveBarrelPool = new List<GameObject>();
    public List<GameObject> m_FishBankPool = new List<GameObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("EntitiesPoolManager already exists, destroying object");
            Destroy(this);
        }

        PopulatePool(m_PiratePool, m_PiratePrefab, m_PiratesPoolSize);
        PopulatePool(m_TentaclePool, m_TentaclePrefab, m_TentaclesPoolSize);
        PopulatePool(m_ExplosiveBarrelPool, m_ExplosiveBarrelPrefab, m_ExplosiveBarrelPoolSize);
        PopulatePool(m_FishBankPool, m_FishBankPrefab, m_FishBankPoolSize);
    }


    private void PopulatePool(List<GameObject> poolToPopulate, GameObject poolEntity, int poolSize)
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject newEntity = Instantiate(poolEntity);
            newEntity.SetActive(false);
            poolToPopulate.Add(newEntity);
        }
    }

    public GameObject GetEntityFromPool(EntityType entityType)
    {
        List<GameObject> entityPool = new List<GameObject>();

        switch (entityType)
        {
            case EntityType.PIRATE:
                entityPool = m_PiratePool;
                break;
            case EntityType.TENTACLE:
                entityPool = m_TentaclePool;
                break;
            case EntityType.EXPLOSIVE_BARREL:
                entityPool = m_ExplosiveBarrelPool;
                break;
            case EntityType.FISH_BANK:
                entityPool = m_FishBankPool;
                break;
            default:
                Debug.LogError("[ENTITY TYPE NOT VALID]");
                break;
        }

        //If the pool is empty, instantiate a new object and add it to the pool
        if(entityPool.Count == 0)
        {
            GameObject newEntityPrefab = null;

            switch (entityType)
            {
                case EntityType.PIRATE:
                    newEntityPrefab = m_PiratePrefab;
                    break;
                case EntityType.TENTACLE:
                    newEntityPrefab = m_TentaclePrefab;
                    break;
                case EntityType.EXPLOSIVE_BARREL:
                    newEntityPrefab = m_ExplosiveBarrelPrefab;
                    break;
                case EntityType.FISH_BANK:
                    newEntityPrefab = m_FishBankPrefab;
                    break;
            }

            if (newEntityPrefab != null)
            {
                GameObject newEntityObject = Instantiate(newEntityPrefab);
                newEntityObject.SetActive(false);
                entityPool.Add(newEntityObject);
            }
        }

        //Get the first entity of the pool, enable it, remove it from the pool and return it
        GameObject entityToReturn = entityPool[0];
        entityPool.RemoveAt(0);
        entityToReturn.SetActive(true);
        return entityToReturn;
    }

    public void ReturnEntityToPool(GameObject entity)
    {
        //Disable the entity
        entity.SetActive(false);

        List<GameObject> entityPool = CheckPoolOfEntity(entity);

        if(entityPool != null)
        {
            entityPool.Add(entity);
        }
    }

    public List<GameObject> CheckPoolOfEntity(GameObject entity)
    {
        Debug.LogWarning("CHECKING POOL OF: [" +  entity.name + "]");
        //Check which entity type it is by finding their base components
        if (entity.GetComponent<PirateController>())
        {
            return m_PiratePool;
        }
        if (entity.GetComponent<TentacleController>())
        {
            return m_TentaclePool;
        }
        if (entity.GetComponent<ExplosiveBarrelController>())
        {
            return m_ExplosiveBarrelPool;
        }
        if (entity.GetComponentInChildren<BoidFishController>() != null)
        {
            return m_FishBankPool;
        }
        else
        {
            Debug.LogError("[ENTITY HAS NO POOL ASSOCIATED]");
            return null;
        }
    }

}

public enum EntityType
{
    NONE = -1,
    PIRATE,
    TENTACLE,
    EXPLOSIVE_BARREL,
    FISH_BANK
}
