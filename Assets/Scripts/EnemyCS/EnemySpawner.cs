using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public class EnemyTypeData
{
    public GameObject prefab;    
    public int unlockStage = 1;    // 몇 스테이지부터 등장할지
    public int initialPoolSize = 5;
}

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Type Settings")]
    [SerializeField] private List<EnemyTypeData> enemyTypes = new List<EnemyTypeData>();

    [Header("Spawn Settings")]
    [SerializeField] private Transform poolParent;
    [SerializeField] private float spacing = 2f;

    private Dictionary<GameObject, List<EnemyState>> enemyPools = new Dictionary<GameObject, List<EnemyState>>();
    private int aliveEnemyCount = 0;

    public Action OnAllEnemiesCleared;

    private void Awake()
    {
        InitializePools();
    }

    private void InitializePools()
    {
        enemyPools.Clear();

        foreach (EnemyTypeData typeData in enemyTypes)
        {
            if (typeData.prefab == null)
                continue;

            if (!enemyPools.ContainsKey(typeData.prefab))
                enemyPools.Add(typeData.prefab, new List<EnemyState>());

            for (int i = 0; i < typeData.initialPoolSize; i++)
            {
                GameObject obj = Instantiate(typeData.prefab, poolParent);
                obj.SetActive(false);

                EnemyState enemyState = obj.GetComponent<EnemyState>();
                enemyPools[typeData.prefab].Add(enemyState);
            }
        }
    }

    public void StartNextStage(int stageNumber, int enemyCount)
    {
        DeactivateAllEnemies();
        aliveEnemyCount = 0;

        for (int i = 0; i < enemyCount; i++)
        {
            GameObject selectedPrefab = GetEnemyPrefabForStage(stageNumber, i, enemyCount);
            SpawnEnemyFromPool(selectedPrefab, i);
        }
    }

    private void DeactivateAllEnemies()
    {
        foreach (var pool in enemyPools.Values)
        {
            foreach (EnemyState enemy in pool)
            {
                if (enemy != null && enemy.gameObject.activeInHierarchy)
                {
                    enemy.gameObject.SetActive(false);
                }
            }
        }
    }
    private GameObject GetEnemyPrefabForStage(int stageNumber, int spawnIndex, int totalEnemyCount)
    {
        List<EnemyTypeData> availableTypes = new List<EnemyTypeData>();

        foreach (EnemyTypeData typeData in enemyTypes)
        {
            if (typeData.prefab != null && typeData.unlockStage <= stageNumber)
            {
                availableTypes.Add(typeData);
            }
        }

        if (availableTypes.Count == 0)
        {
            return null;
        }

        int randomIndex = UnityEngine.Random.Range(0, availableTypes.Count);
        return availableTypes[randomIndex].prefab;
    }

    private void SpawnEnemyFromPool(GameObject prefab, int index)
    {
        if (prefab == null)
            return;

        EnemyState enemy = GetInactiveEnemyFromPool(prefab);

        if (enemy == null)
        {
            GameObject newObj = Instantiate(prefab, poolParent);
            newObj.SetActive(false);

            enemy = newObj.GetComponent<EnemyState>();

            if (!enemyPools.ContainsKey(prefab))
                enemyPools.Add(prefab, new List<EnemyState>());

            enemyPools[prefab].Add(enemy);
        }

        SpawnEnemy(enemy, index);
    }

    private EnemyState GetInactiveEnemyFromPool(GameObject prefab)
    {
        if (!enemyPools.ContainsKey(prefab))
            return null;

        foreach (EnemyState enemy in enemyPools[prefab])
        {
            if (enemy != null && !enemy.gameObject.activeInHierarchy)
            {
                return enemy;
            }
        }

        return null;
    }

    private void SpawnEnemy(EnemyState enemy, int index)
    {
        Vector3 spawnPos = transform.position + Vector3.right * spacing * index;
        enemy.transform.position = spawnPos;
        enemy.gameObject.SetActive(true);
        aliveEnemyCount++;
    }

    public void NotifyEnemyDead()
    {
        if (aliveEnemyCount <= 0)
            return;

        aliveEnemyCount--;

        if (aliveEnemyCount == 0)
        {
            Debug.Log("모든 적 처치 완료");
            OnAllEnemiesCleared?.Invoke();
        }
    }

    public bool IsAllEnemiesCleared()
    {
        return aliveEnemyCount == 0;
    }
}