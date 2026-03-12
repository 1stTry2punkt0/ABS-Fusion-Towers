using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectileSpawnManager : MonoBehaviour
{
    public static ProjectileSpawnManager instance;

    [SerializeField] GameObject[] projecilePrefab;
    private BaseTower tower;

    private ObjectPool<Projectile> projectilePool;
    private Dictionary<ProjectileType, ObjectPool<Projectile>> projectilePools = new Dictionary<ProjectileType, ObjectPool<Projectile>>();

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
        for (int i = 0; i < projecilePrefab.Length; i++)
        {
            ProjectileType projectileType = (ProjectileType)i;
            GameObject prefab = projecilePrefab[i];
            ObjectPool<Projectile> pool = new ObjectPool<Projectile>(
                () => CreateProjectile(prefab, projectileType),
                OnGetProjectile,
                OnReleaseProjectile,
                OnDestroyProjectile,
                true, 10, 1000);
            projectilePools.Add(projectileType, pool);
        }
    }

    private Projectile CreateProjectile(GameObject prefab, ProjectileType type)
    {
        GameObject projObj = Instantiate(prefab, transform.position, transform.rotation);
        Projectile projectile = projObj.GetComponent<Projectile>();
        projectile.SetPool(projectilePools[type]);
        return projectile;
    }

    private void OnGetProjectile(Projectile projectile)
    {
        projectile.isActive = true;
        projectile.gameObject.SetActive(true);
    }

    private void OnReleaseProjectile(Projectile projectile)
    {
        projectile.isActive = false;
        projectile.gameObject.SetActive(false);
    }

    private void OnDestroyProjectile(Projectile projectile)
    {
        Destroy(projectile.gameObject);
    }

    public void SpawnProjectile(ProjectileType projectileType, BaseTower tower, Enemy target)
    {
        if (projectilePools.TryGetValue(projectileType, out var pool))
        {
            Projectile proj = pool.Get();
            proj.Initialize(tower, target);
        }
    }

}

public enum ProjectileType
{
    Arrow,
    Bolt,
    Bomb
}