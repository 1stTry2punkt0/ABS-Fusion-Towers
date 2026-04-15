using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class BaseTower : MonoBehaviour, IOnTopObj
{
    public MapTile mapTile { get; set; }


    // --- Base Stats ---
    public TowerStatSO stats;          // Reference to the ScriptableObject for this tower type
    public TowerType towerName;          // Display name of the tower
    public int level = 1;                 // Current upgrade level
    public int[] optionlvl = new int[2];        // Level of the upgrade options
    public float range;               // Attack radius
    public SphereCollider rangeCollider;        // Collider used for detecting enemies in range
    public GameObject rangeIndicator;        // Visual representation of the tower's range (optional)
    public float damage;              // Base damage value
    public float attackSpeed;         // Attacks per second or cooldown modifier
    public float damageRange;

    public float duration;
    public float effectiveness;

    public Cost sellValue { get; set; }            // Value returned to the player when selling the tower

    public bool canAttack = true;     // Global attack toggle

    public Transform shootPoint;
    [SerializeField] GameObject weapon;

    protected readonly List<Enemy> enemiesInRange = new(); // Enemies currently inside the trigger radius
    public targetSelection targetSelectionType = targetSelection.first; // Targeting mode
    public Enemy targetEnemyData;     // Currently selected target

    public ulong dmgDealt = 0;
    protected bool isSelected;

    // --- Unity Lifecycle ---
    protected virtual void Start()
    {
        // Initialization logic shared by all towers
        rangeCollider = GetComponent<SphereCollider>();
        Initialize();
    }

    protected virtual void Update()
    {
        // Shared update logic for all towers
        if (canAttack && enemiesInRange.Count > 0)
        {
            FindTarget();   // Determine which enemy to attack
            RotateWeaponToTarget();
            Attack();       // Execute the attack logic
        }
    }


    // --- Trigger Handling ---
    protected virtual void OnTriggerEnter(Collider other)
    {
        // Add enemy when it enters the tower's range
        if (other.TryGetComponent(out Enemy enemy))
            enemiesInRange.Add(enemy);
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        // Remove enemy when it leaves the tower's range
        if (other.TryGetComponent(out Enemy enemy))
            enemiesInRange.Remove(enemy);
    }

    // --- Targeting Logic ---
    public void FindTarget()
    {
        // Remove dead or destroyed enemies
        enemiesInRange.RemoveAll(e => e == null || e.isDead);
        targetEnemyData = null;

        // If no enemies remain, clear target and exit
        if (enemiesInRange.Count == 0)
            return;

        // Sort only when needed (first/last based on progress)
        if (targetSelectionType == targetSelection.first ||
            targetSelectionType == targetSelection.last)
        {
            enemiesInRange.Sort((a, b) => b.progress.CompareTo(a.progress));
        }

        // Select target based on targeting mode
        switch (targetSelectionType)
        {
            case targetSelection.first:
                targetEnemyData = enemiesInRange[0];
                break;

            case targetSelection.last:
                targetEnemyData = enemiesInRange[^1];
                break;

            case targetSelection.strongest:
                targetEnemyData = GetStrongest();
                break;

            case targetSelection.weakest:
                targetEnemyData = GetWeakest();
                break;
        }
    }

    private float currentYRotation;
    private float rotationVelocity;
    public void RotateWeaponToTarget()
    {
        if (weapon == null || targetEnemyData == null) return;

        Vector3 dir = targetEnemyData.transform.position - transform.position;
        dir.y = 0;

        float targetAngle = Quaternion.LookRotation(dir).eulerAngles.y;

        currentYRotation = Mathf.SmoothDampAngle(
            currentYRotation,
            targetAngle,
            ref rotationVelocity,
            0.15f // smooth time
        );

        weapon.transform.rotation = Quaternion.Euler(0, currentYRotation, 0);
    }

    public void SetTargetSelection(targetSelection selection)
    {
        targetSelectionType = selection;
    }

    // Returns the enemy with the highest health (stable order on ties)
    private Enemy GetStrongest()
    {
        Enemy best = null;
        float bestValue = float.MinValue;

        foreach (var e in enemiesInRange)
        {
            if (e.currentHealth > bestValue)
            {
                bestValue = e.currentHealth;
                best = e;
            }
        }
        return best;
    }

    // Returns the enemy with the lowest health (stable order on ties)
    private Enemy GetWeakest()
    {
        Enemy best = null;
        float bestValue = float.MaxValue;

        foreach (var e in enemiesInRange)
        {
            if (e.currentHealth < bestValue)
            {
                bestValue = e.currentHealth;
                best = e;
            }
        }
        return best;
    }

    public void SetCanAttack(bool value)
    {
        canAttack = value;
    }

    public void Upgrade(int index)
    {
        if (level > 5)
            return;
        if (!RessourceManager.instance.SpendRessource(stats.upgradeCosts[index]))
        {
            GameManager.instance.Invalid(GameManager.instance.invalidMessages[1]);
            return;
        }
        sellValue.amount += Mathf.CeilToInt(stats.upgradeCosts[index].amount * 0.7f);
        UpgradeOption option = stats.upgradeOption[index];
        float increaseAmount = option.increaseAmount * GetMultipyer(optionlvl[index]);
        switch (option.statToUpgrade)
        {
            case Stats.damage:
                damage += increaseAmount;
                break;

            case Stats.attackSpeed:
                attackSpeed += increaseAmount;
                break;
            case Stats.range:
                range += increaseAmount;
                rangeCollider.radius = range; // Update collider radius to match new range
                Vector3 indicatorScale = Vector3.one * range * 2; // Calculate the scale for the range indicator
                indicatorScale.y = 0.01f; // Keep the Y scale thin for a flat indicator
                rangeIndicator.transform.localScale = indicatorScale; // Scale the indicator to match the range
                break;
            case Stats.damageRange:
                damageRange += increaseAmount;
                break;
            case Stats.duration:
                duration += increaseAmount;
                break;
            case Stats.effectiveness:
                effectiveness += increaseAmount;
                break;

        }

        level++;
        optionlvl[index]++;

    }

    private float GetMultipyer(int currentlvl)
    {
        float multiplyer = 0f;
        switch (currentlvl)
        {
            case 0:
                multiplyer = 0.26f;
                break;
            case 1:
                multiplyer = 0.07f;
                break;
            case 2:
                multiplyer = 0.13f;
                break;
            case 3:
                multiplyer = 0.2f;
                break;
            case 4:
                multiplyer = 0.34f;
                break;
        }
        return multiplyer;
    }
    public virtual void Initialize()
    {
        towerName = stats.towerName;
        level = 1;

        // Base stats
        range = stats.baseRange;
        rangeCollider.radius = range; // Ensure the collider matches the range
        Vector3 indicatorScale = Vector3.one * range * 2; // Calculate the scale for the range indicator
        indicatorScale.y = 0.01f; // Keep the Y scale thin for a flat indicator
        rangeIndicator.transform.localScale = indicatorScale; // Scale the indicator to match the range

        damage = stats.baseDamage;
        attackSpeed = stats.baseAttackSpeed; // attacks per second
        damageRange = stats.baseDamageRange;
        duration = stats.duration;
        effectiveness = stats.effectiveness;
        sellValue = new Cost
        {
            amount = stats.baseCost.amount,
            ressourceType = stats.baseCost.ressourceType
        };
        sellValue.amount = Mathf.CeilToInt(sellValue.amount * 0.7f);


    }


    public virtual void TargetHit(Enemy enemy, float multiplier = 1)
    {
        if (enemy == null || enemy.isDead)
            return;
        enemy.ApplyStatusEffect(this, stats.statusEffect, duration, effectiveness);
        dmgDealt += (ulong)enemy.TakeDamage(damage * multiplier, stats.damageType);
        if (isSelected)
            TowerMenu.instance.UpdateDmg();
    }

    // --- Abstract Methods (implemented by specific tower types) ---
    public abstract void Attack();
    public virtual void Fuse(ElementalTower otherTower)
    {
        GameObject fusion = Instantiate(otherTower.FusionPrefab);
        fusion.transform.position = transform.position;
        fusion.transform.SetParent(transform);
    }

    public virtual void OnFusion()
    {
        // Fusion logic comes later
        GameManager.instance.Fusion(this);
    }

    public void OnSell()
    {
        if(GameManager.instance.gameState == GameState.Preparing)
        {
            sellValue.amount = Mathf.CeilToInt(sellValue.amount / 0.7f); 
            RessourceManager.instance.GainRessource(sellValue);
        }
        else
        {
            RessourceManager.instance.GainRessource(sellValue);
        }
        TowerMenu.instance.CloseMenu(false, this);
        GameManager.instance.towerList.Remove(this);
        GameManager.instance.SellBuilding(gameObject);
    }

    public void OnSelect()
    {
        if(GameManager.instance.gameState == GameState.Fusing)
        {
            if(level != 6)
            {
                GameManager.instance.Invalid(GameManager.instance.invalidMessages[4]);
                return;
            }
            OnFusion();
            return;
        }
        rangeIndicator.SetActive(true);
        TowerMenu.instance.OpenMenu(this);
        isSelected = true;
    }

    public void DeSelect()
    {
        rangeIndicator.SetActive(false);
        TowerMenu.instance.CloseMenu(false, this);
        isSelected = false;
    }

}

// Target selection modes
public enum targetSelection
{
    first,      // Enemy with highest progress
    last,       // Enemy with lowest progress
    strongest,  // Enemy with highest health
    weakest,    // Enemy with lowest health
}