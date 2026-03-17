using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager instance;
    public List<Vector2Int> enemyPath;

    [SerializeField] GameObject[] enemyPrefab;
    [SerializeField] Transform spawnPoint;

    private ObjectPool<Enemy> enemyPool;
    private Dictionary<EnemyType, ObjectPool<Enemy>> enemyPools = new Dictionary<EnemyType, ObjectPool<Enemy>>();

    public int waveIndex = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        CreateDictionary();
    }
    public void CreateDictionary()
    {
        for (int i = 0; i < enemyPrefab.Length; i++)
        {
            GameObject prefab = enemyPrefab[i];
            EnemyType enemyType = prefab.GetComponent<Enemy>().stats.enemyType;
            ObjectPool<Enemy> pool = new ObjectPool<Enemy>(
                () => CreateEnemy(prefab, enemyType),
                OnGetEnemy,
                OnReleaseEnemy,
                OnDestroyEnemy,
                true, 50, 1000);
            enemyPools.Add(enemyType, pool);
        }
    }

    private Enemy CreateEnemy( GameObject prefab, EnemyType name)
    {
        GameObject enemyObj = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        Enemy enemy = enemyObj.GetComponent<Enemy>();
        enemy.SetPool(enemyPools[name]);
        return enemy;
    }

    private void OnGetEnemy(Enemy enemy)
    {
        enemy.transform.position = spawnPoint.position;
        enemy.transform.rotation = spawnPoint.rotation;

        enemy.SetStats();
        enemy.movementEnabled = true;
        enemy.gameObject.SetActive(true);
    }

    private void OnReleaseEnemy(Enemy enemy)
    {
        enemy.movementEnabled = false;
        enemy.gameObject.SetActive(false);
    }

    private void OnDestroyEnemy(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }

    public void SpawnEnemy(EnemyType enemyType)
    {
        if (enemyPools.ContainsKey(enemyType))
        {
            enemyPools[enemyType].Get();
        }
        else
        {
            Debug.LogWarning("No pool found for enemy type: " + enemyType);
        }
    }
}

public enum EnemyType
{
    Skeleton,
    example
}