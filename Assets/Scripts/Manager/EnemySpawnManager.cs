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
    private Dictionary<string, ObjectPool<Enemy>> enemyPools = new Dictionary<string, ObjectPool<Enemy>>();

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
            string enemyType = enemyPrefab[i].name;
            GameObject prefab = enemyPrefab[i];
            ObjectPool<Enemy> pool = new ObjectPool<Enemy>(
                () => CreateEnemy(prefab, enemyType),
                OnGetEnemy,
                OnReleaseEnemy,
                OnDestroyEnemy,
                true, 10, 100);
            enemyPools.Add(enemyType, pool);
        }
    }

    private Enemy CreateEnemy( GameObject prefab, string name)
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
        // Set enemy stats from enemyStatSO
    }

    private void OnReleaseEnemy(Enemy enemy)
    {
        enemy.movementEnabled = false;
        enemy.gameObject.SetActive(false);
        // Reset enemy state if necessary
    }

    private void OnDestroyEnemy(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }

    void Start()
    {

        // Example: Spawn an enemy of type "EnemyType1"
        InvokeRepeating(nameof(TestspawnSkeleton), 2f, 5f);
    }


    private void TestspawnSkeleton()
    {
        SpawnEnemy("Skeleton");

    }

    public void SpawnEnemy(string enemyType)
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
