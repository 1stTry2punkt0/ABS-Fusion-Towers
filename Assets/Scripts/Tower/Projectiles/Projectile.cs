using UnityEngine;
using UnityEngine.Pool;

public abstract class Projectile : MonoBehaviour
{
    protected ObjectPool<Projectile> pool; // Reference to the object pool for this projectile type
    public BaseTower parentTower;

    protected Enemy targetEnemy; // The enemy this projectile is currently targeting
    protected Vector3 targetPosition; // Cached transform of the target for efficient movement calculations

    [SerializeField] protected float speed; // Speed at which the projectile moves towards its target
    public bool isActive = false; // Indicates whether the projectile is currently active in the scene

    [SerializeField] AudioClip hitSound;
    [SerializeField] float volume = 1;
    private float maxLifetime = 5f; // Maximum lifetime of the projectile to prevent it from existing indefinitely

    public virtual void Initialize(BaseTower tower, Enemy target)
    {
        parentTower = tower;
        targetEnemy = target;
        if(tower.shootPoint != null)
            transform.position = tower.shootPoint.position;
        else
            transform.position =  new Vector3(targetEnemy.transform.position.x, 1, targetEnemy.transform.position.z);

        targetPosition = targetEnemy.transform.position; // Cache the target's transform for movement
        isActive = true;
        maxLifetime = 5f; // Reset lifetime when initialized
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
        if (Vector3.Distance(transform.position, targetPosition) < 1f)
        {
            PlayHitSound();
            if (targetEnemy != null)
            {
                parentTower.TargetHit(targetEnemy);
            }
            if (!FindNextTarget())
                Disable();// Return to pool after hitting the target
        }
        // Lifetime check
        maxLifetime -= Time.deltaTime;
        if (maxLifetime <= 0)
        {
            Disable(); // Return to pool if lifetime expires
        }
    }

    protected virtual void PlayHitSound()
    {
        if (hitSound != null)
            AudioManager.instance.PlaySoundFXClip(hitSound, transform, volume);
    }

    protected virtual void Disable()
    {
        pool.Release(this);
    }

    public abstract void SetTarget();

    public void SetPool(ObjectPool<Projectile> pool)
    {
        this.pool = pool;
    }

    protected virtual bool FindNextTarget()
    {
        return false;
    }
}
