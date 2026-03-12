using UnityEngine;
using UnityEngine.Pool;

public abstract class Projectile : MonoBehaviour
{
    protected ObjectPool<Projectile> pool; // Reference to the object pool for this projectile type
    public BaseTower parentTower;

    protected Enemy targetEnemy; // The enemy this projectile is currently targeting
    protected Vector3 targetPosition; // Cached transform of the target for efficient movement calculations

    [SerializeField] float speed; // Speed at which the projectile moves towards its target
    public bool isActive = false; // Indicates whether the projectile is currently active in the scene

    public virtual void Initialize(BaseTower tower, Enemy target)
    {
        parentTower = tower;
        targetEnemy = target;
        transform.position = tower.shootPoint.position;

        targetPosition = targetEnemy.transform.position; // Cache the target's transform for movement
        isActive = true;
    }

    protected virtual void Update()
    {
        SetTarget();
        // Move towards target
        Vector3 dir = (targetPosition - transform.position).normalized;
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(dir);

        transform.position += dir * speed * Time.deltaTime;

        // Hit detection
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            parentTower.TargetHit(targetEnemy);
            pool.Release(this); // Return to pool after hitting the target
        }
    }

    public abstract void SetTarget();

    public void SetPool(ObjectPool<Projectile> pool)
    {
        this.pool = pool;
    }
}
