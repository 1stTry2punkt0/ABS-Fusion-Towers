using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTower : MonoBehaviour
{
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
    public bool canAttack = true;     // Global attack toggle

    public Transform shootPoint;

    protected readonly List<Enemy> enemiesInRange = new(); // Enemies currently inside the trigger radius
    public targetSelection targetSelectionType = targetSelection.first; // Targeting mode
    public Enemy targetEnemyData;     // Currently selected target

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
        Debug.Log($"Selected target: {targetEnemyData.name} with {targetEnemyData.currentHealth} HP");
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

    public void OnSelect(bool isSelected)
    {
        rangeIndicator.SetActive(isSelected);
    }

    // --- Abstract Methods (implemented by specific tower types) ---
    public abstract void Initialize();
    public abstract void Attack();
    public abstract void TargetHit(Enemy enemy);
    public abstract void OnFusion(BaseTower otherTower);
    public abstract void OnSell();
}

// Target selection modes
public enum targetSelection
{
    first,      // Enemy with highest progress
    last,       // Enemy with lowest progress
    strongest,  // Enemy with highest health
    weakest,    // Enemy with lowest health
}