using UnityEngine;
using UnityEngine.Pool;

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemySO stats;
    private int level;
    public float currentHealth;
    public bool isDead => currentHealth <= 0;
    private ObjectPool<Enemy> pool;
    public bool movementEnabled = false;
    public int currentWaypointIndex = 0;
    public float distanceToTarget;
    private Vector3 targetPosition;
    public float progress => currentWaypointIndex * 100 - distanceToTarget;


    // Update is called once per frame
    void Update()
    {
        if (!movementEnabled) return;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, stats.speed * Time.deltaTime);
        //Rotate towards the target position
        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            if (lookRotation != transform.rotation)
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
        distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        if (distanceToTarget < stats.speed * Time.deltaTime)
        {
            SetTarget();
            if (currentWaypointIndex < EnemySpawnManager.instance.enemyPath.Count)
            {
                targetPosition = GameManager.instance.GetWorldPosition(EnemySpawnManager.instance.enemyPath[currentWaypointIndex]);
            }
            else
            {
                // Enemy reached the end of the path, handle accordingly (e.g., damage player, return to pool)
                pool.Release(this);
            }
        }
    }

    private void SetTarget()
    {
        transform.position = targetPosition;
        currentWaypointIndex++;
    }

    public void SetPool(ObjectPool<Enemy> pool)
    {
        this.pool = pool;
    }

    public void SetStats()
    {
        currentHealth = stats.maxHp;
        currentWaypointIndex = 0;
        targetPosition = GameManager.instance.GetWorldPosition(EnemySpawnManager.instance.enemyPath[currentWaypointIndex]);
        // Set other stats as needed
    }

    public void TakeDamage(float damage, DamageType dmgtype)
    {
        if (dmgtype == DamageType.weapon)
        {
            damage *= 1 - stats.amor/100;
        }
        else if (dmgtype == DamageType.elemental)
        {
            damage *= 1 - stats.resistance/100;
        }
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            pool.Release(this);
        }
    }

    public void ApplyStatusEffect(StatusEffect effect)
    {
        // Implement status effect application logic here
    }
}
