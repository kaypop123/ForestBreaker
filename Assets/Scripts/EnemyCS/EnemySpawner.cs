using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.Profiling;

[System.Serializable]
public class EnemyTypeData
{
    public GameObject prefab;
    public int unlockStage = 1;
    public int initialPoolSize = 5;
}

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Type Settings")]
    [SerializeField] private List<EnemyTypeData> enemyTypes = new List<EnemyTypeData>();

    [Header("Spawn Settings")]
    [SerializeField] private Transform poolParent;
    [SerializeField] private float spacing = 2f;

    // Dictionary 순회 가비지를 줄이기 위해 Key List 별도 보관 또는 캐시
    private Dictionary<GameObject, List<EnemyState>> enemyPools = new Dictionary<GameObject, List<EnemyState>>();
    private List<EnemyTypeData> cachedAvailableTypes = new List<EnemyTypeData>(); // GC 방지용 캐싱 리스트

    private int aliveEnemyCount = 0;

    public Action OnAllEnemiesCleared;
    private static readonly ProfilerMarker s_SpawnMarker = new ProfilerMarker("[MY_SPAWN] MonsterWave");

    private void Awake()
    {
        InitializePools();
    }

    private void InitializePools()
    {
        enemyPools.Clear();

        for (int t = 0; t < enemyTypes.Count; t++)
        {
            EnemyTypeData typeData = enemyTypes[t];
            if (typeData.prefab == null)
                continue;

            if (!enemyPools.ContainsKey(typeData.prefab))
            {
                // List Capacity를 미리 넉넉하게 지정하여 재할당 방지
                enemyPools.Add(typeData.prefab, new List<EnemyState>(typeData.initialPoolSize * 2));
            }

            for (int i = 0; i < typeData.initialPoolSize; i++)
            {
                CreateNewEnemyToPool(typeData.prefab);
            }
        }
    }

    private EnemyState CreateNewEnemyToPool(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab, poolParent);
        obj.SetActive(false);

        EnemyState enemyState = obj.GetComponent<EnemyState>();
        enemyPools[prefab].Add(enemyState);
        return enemyState;
    }

    public void StartNextStage(int stageNumber, int enemyCount)
    {
        using (s_SpawnMarker.Auto())
        {
            DeactivateAllEnemies();
            aliveEnemyCount = 0;

            for (int i = 0; i < enemyCount; i++)
            {
                GameObject selectedPrefab = GetEnemyPrefabForStage(stageNumber);
                SpawnEnemyFromPool(selectedPrefab, i);
            }
        }
#if UNITY_EDITOR
        Debug.Break();
#endif
    }

    private void DeactivateAllEnemies()
    {
        // Dictionary.Values 직접 foreach 대신 Key 컬렉션 기반 for 순회로 가비지 방지
        for (int t = 0; t < enemyTypes.Count; t++)
        {
            GameObject prefab = enemyTypes[t].prefab;
            if (prefab == null || !enemyPools.TryGetValue(prefab, out List<EnemyState> pool))
                continue;

            for (int i = 0; i < pool.Count; i++)
            {
                EnemyState enemy = pool[i];
                if (enemy != null && enemy.gameObject.activeSelf)
                {
                    enemy.gameObject.SetActive(false);
                }
            }
        }
    }

    private GameObject GetEnemyPrefabForStage(int stageNumber)
    {
        cachedAvailableTypes.Clear(); // new List 대신 재사용

        for (int i = 0; i < enemyTypes.Count; i++)
        {
            EnemyTypeData typeData = enemyTypes[i];
            if (typeData.prefab != null && typeData.unlockStage <= stageNumber)
            {
                cachedAvailableTypes.Add(typeData);
            }
        }

        if (cachedAvailableTypes.Count == 0)
            return null;

        int randomIndex = UnityEngine.Random.Range(0, cachedAvailableTypes.Count);
        return cachedAvailableTypes[randomIndex].prefab;
    }

    private void SpawnEnemyFromPool(GameObject prefab, int index)
    {
        if (prefab == null)
            return;

        EnemyState enemy = GetInactiveEnemyFromPool(prefab);

        // 풀에 남은 게 없으면 추가 생성
        if (enemy == null)
        {
            if (!enemyPools.ContainsKey(prefab))
                enemyPools.Add(prefab, new List<EnemyState>(10));

            enemy = CreateNewEnemyToPool(prefab);
        }

        SpawnEnemy(enemy, index);
    }

    private EnemyState GetInactiveEnemyFromPool(GameObject prefab)
    {
        if (!enemyPools.TryGetValue(prefab, out List<EnemyState> pool))
            return null;

        for (int i = 0; i < pool.Count; i++)
        {
            EnemyState enemy = pool[i];
            if (enemy != null && !enemy.gameObject.activeSelf)
            {
                return enemy;
            }
        }

        return null;
    }

    private void SpawnEnemy(EnemyState enemy, int index)
    {
        Vector3 spawnPos = transform.position + Vector3.right * (spacing * index);
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
            OnAllEnemiesCleared?.Invoke();
        }
    }

    public bool IsAllEnemiesCleared()
    {
        return aliveEnemyCount == 0;
    }
}