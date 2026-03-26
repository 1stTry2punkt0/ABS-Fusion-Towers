using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Enemy : MonoBehaviour
{
    public EnemySO stats;
    private int level;
    public float currentHealth;
    public float heightOffset;
    public bool isDead => currentHealth <= 0;
    private ObjectPool<Enemy> pool;
    public bool movementEnabled = false;
    public int currentWaypointIndex = 0;
    public float distanceToTarget;
    private Vector3 targetPosition;
    public float progress => currentWaypointIndex * 100 - distanceToTarget;
    private float electrifyMultiplyer = 1;
    private StatusEffect currentStatus;
    private Coroutine statusRoutine;
    private ParticlePoolObj statusParticle;


    private void Awake()
    {
        heightOffset = GetComponent<CapsuleCollider>().height / 2 +1;
        if (stats.moveType == MoveType.Fly) heightOffset += 1;
    }

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
                if (stats.moveType == MoveType.Fly)
                    targetPosition = ConvertForFly(targetPosition);
            }
            else
            {
                // Enemy reached the end of the path, handle accordingly (e.g., damage player, return to pool)
                RessourceManager.instance.SpendRessource(stats.damage);
                Disappear();
            }
        }
    }

    public void Disappear()
    {
        if(statusParticle != null)
        {
            statusParticle.pool.Release(statusParticle);
            statusParticle = null;
        }
        movementEnabled = false;
        currentHealth = 0;
        WaveManager.instance.enemyCount--;
        pool.Release(this);
    }

    protected virtual void Die()
    {
        Disappear();
    }

    protected virtual void SetTarget()
    {
        transform.position = targetPosition;
        currentWaypointIndex++;
    }

    private Vector3 ConvertForFly(Vector3 position)
    {
        position.z = transform.position.z;
        return position;
    }

    public void SetPool(ObjectPool<Enemy> pool)
    {
        this.pool = pool;
    }

    public virtual void Initialize()
    {
        currentHealth = stats.maxHp;
        currentWaypointIndex = 0;
        targetPosition = GameManager.instance.GetWorldPosition(EnemySpawnManager.instance.enemyPath[currentWaypointIndex]);
        // Set other stats as needed
    }

    public float TakeDamage(float damage, DamageType dmgtype)
    {
        if (dmgtype == DamageType.weapon)
        {
            damage *= 1 - (stats.amor * electrifyMultiplyer)/100;
        }
        else if (dmgtype == DamageType.elemental)
        {
            damage *= 1 - (stats.resistance * electrifyMultiplyer)/ 100;
        }
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            damage += currentHealth; 
            Die();
        }
        return damage;
    }

    public void ApplyStatusEffect(StatusEffect effect, float duration, float effectiveness)
    {
        if (effect == StatusEffect.none) return;
        if (statusRoutine != null)
        {
            StopCoroutine(statusRoutine);
        }
        if(statusParticle != null)
        {
            statusParticle.pool.Release(statusParticle);
            statusParticle = null;
        }
        StatusEffect lastStatus = currentStatus;
        currentStatus = effect;
        // Implement status effect application logic here
        switch (effect)
        {
            case StatusEffect.electrify:
                
                electrifyMultiplyer = Mathf.Min(1-effectiveness, electrifyMultiplyer);
                statusParticle = ParticleSpawnManager.instance.SpawnParticle(ParticleType.Electrified, transform.position);
                statusParticle.gameObject.transform.SetParent(transform);

                StartCoroutine(Electryfied());
                statusRoutine = StartCoroutine(ResetStatusEffect(duration));
                break;
        }
    }

    private IEnumerator ResetStatusEffect(float duration)
    {
        yield return new WaitForSeconds(duration);
        currentStatus = StatusEffect.none;
        statusParticle.pool.Release(statusParticle);
        statusParticle = null;
    }

    private IEnumerator Electryfied()
    {
        while(StatusEffect.electrify == currentStatus)
        {
            yield return null;
        }
        electrifyMultiplyer = 1;
    }
    private IEnumerator Burned(float effectiveness)
    {
        while(StatusEffect.burn == currentStatus)
        {
            TakeDamage(effectiveness, DamageType.elemental);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
