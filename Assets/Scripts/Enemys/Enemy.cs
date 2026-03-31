using System;
using System.Collections;
using System.Collections.Generic;
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
    private float electrifyMultiplier = 1;
    private float freezeMultiplier = 1;
    private StatusEffectData[] statusEffectDatas;
    private Transform meshTransform;


    private void Awake()
    {
        heightOffset = GetComponent<CapsuleCollider>().height / 2 +1;
        if (stats.moveType == MoveType.Fly) heightOffset += 1;

        statusEffectDatas = new StatusEffectData[Enum.GetValues(typeof(StatusEffect)).Length];

        for (int i = 0; i < statusEffectDatas.Length; i++)
        {
            statusEffectDatas[i] = new StatusEffectData
            {
                effect = (StatusEffect)i,
                isActive = false,
                duration = 0,
                effectiveness = 0,
                particle = null,
                resetRoutine = null
            };
        }
        meshTransform = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!movementEnabled) return;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, stats.speed * freezeMultiplier * Time.deltaTime);
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
        foreach(StatusEffectData data in statusEffectDatas)
        {
            if (data.isActive) EndStatusEffect(data);
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
        if(currentHealth <= 0) return 0;
        float electrifyDmg = 0;
        if (dmgtype == DamageType.weapon)
        {
            damage *= 1 - stats.amor/100;
            if(statusEffectDatas[(int)StatusEffect.electrify].isActive)
                electrifyDmg = damage * (1 - (stats.amor * electrifyMultiplier)/100) - damage;
            Debug.Log("Electrify Dmg: " + electrifyDmg);
        }
        else if (dmgtype == DamageType.elemental)
        {
            damage *= 1 - stats.resistance/ 100;
            if (statusEffectDatas[(int)StatusEffect.electrify].isActive)
                electrifyDmg = damage * (1 - (stats.resistance * electrifyMultiplier)/ 100) - damage;
            Debug.Log("Electrify Dmg: " + electrifyDmg);
        }
        currentHealth -= damage + electrifyDmg;
        if(electrifyDmg != 0)
            statusEffectDatas[(int)StatusEffect.electrify].appliedBy.dmgDealt += electrifyDmg;
        if (currentHealth <= 0)
        {
            damage += currentHealth; 
            Die();
        }
        return damage;
    }

    public void ApplyStatusEffect(BaseTower tower, StatusEffect effect, float duration, float effectiveness)
    {
        var data = statusEffectDatas[(int)effect];

        // Wenn schon aktiv resetten
        if (data.isActive)
        {
            if (data.resetRoutine != null)
                StopCoroutine(data.resetRoutine);

            if (data.particle != null)
            {
                data.particle.pool.Release(data.particle);
                data.particle = null;
            }
        }

        // Runtime-Daten setzen
        data.appliedBy = tower;
        data.isActive = true;
        data.duration = duration;
        data.effectiveness = effectiveness;

        // Partikel starten
        Vector3 position = meshTransform.position;
        position.y = heightOffset;
        data.particle = ParticleSpawnManager.instance.SpawnParticle(GetParticleType(effect), position);
        data.particle.transform.SetParent(transform);

        // Coroutine starten
        data.resetRoutine = StartCoroutine(RunStatusEffect(data));
    }

    private ParticleType GetParticleType(StatusEffect effect)
    {
        switch(effect)
        {
            case StatusEffect.electrify:
                return ParticleType.Electrified;
            case StatusEffect.freeze:
                return ParticleType.Frozen;
            case StatusEffect.burn:
                return ParticleType.Burn;
        }
        return ParticleType.Electrified;
    }

    private IEnumerator RunStatusEffect(StatusEffectData data)
    {
        float timer = 0f;
        switch (data.effect)
        {
            case StatusEffect.electrify:
                electrifyMultiplier = 1 - data.effectiveness;
                break;
            case StatusEffect.freeze:
                freezeMultiplier = 1 - data.effectiveness;
                break;
            default:
                break;
        }

        while (timer < data.duration)
        {
            switch (data.effect)
            {
                case StatusEffect.burn:
                    data.appliedBy.dmgDealt += TakeDamage(data.effectiveness, DamageType.elemental);
                    yield return new WaitForSeconds(0.2f);
                    timer += 0.2f;
                    break;

                default:
                    yield return null;
                    timer += Time.deltaTime;
                    break;
            }
        }
        EndStatusEffect(data);
    }

    private void EndStatusEffect(StatusEffectData data)
    {
        // Effekt zurücksetzen
        data.isActive = false;
        data.appliedBy = null;

        if (data.particle != null)
        {
            data.particle.pool.Release(data.particle);
            data.particle = null;
        }
        switch (data.effect)
        {
            case StatusEffect.electrify:
                electrifyMultiplier = 1;
                break;
            case StatusEffect.freeze:
                freezeMultiplier = 1;
                break;
            default:
                break;
        }

        data.resetRoutine = null;

    }

}

[System.Serializable]
public class StatusEffectData
{
    public BaseTower appliedBy;
    public StatusEffect effect;
    public float duration;
    public float effectiveness;
    public ParticlePoolObj particle;
    public Coroutine resetRoutine;
    public bool isActive;
}
