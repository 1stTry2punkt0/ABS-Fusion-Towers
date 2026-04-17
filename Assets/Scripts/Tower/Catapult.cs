using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Catapult : BaseTower
{
    private float attackCooldown;
    [SerializeField] GameObject shuffel;
    [SerializeField] GameObject bomb;
    [SerializeField] CatapultFusionSO[] fusions;
    private ProjectileType projectile = ProjectileType.Bomb;
    public List<ElementalGround> activeGrounds = new List<ElementalGround>();

    public override void Initialize()
    {
        base.Initialize();

        attackCooldown = 0f;
    }

    private void FixedUpdate()
    {
        // Cooldown handling
        if (attackCooldown > 0f)
        {
            attackCooldown -= Time.fixedDeltaTime;
            if (attackCooldown < (1 / attackSpeed) / 3 && !bomb.activeSelf) bomb.SetActive(true);
        }
    }

    public override void Attack()
    {
        if (targetEnemyData == null)
            return;

        if (attackCooldown > 0f)
            return;
        ProjectileSpawnManager.instance.SpawnProjectile(projectile, this, targetEnemyData); // Spawn an arrow projectile
        bomb.SetActive(false);
        StartCoroutine(AnimateShuffle());
        // Reset cooldown
        attackCooldown = 1f / attackSpeed;
    }

    public override void Fuse(ElementalTower otherTower)
    {
        base.Fuse(otherTower);
        CatapultFusionSO fusionTower = null;
        switch (otherTower.elementalAttack)
        {
            case ParticleType.FireErruption:
                fusionTower = Instantiate(fusions[0]);
                projectile = ProjectileType.FireBomb;
                break;
            case ParticleType.IceErruption:
                fusionTower = Instantiate(fusions[1]);
                projectile = ProjectileType.IceBomb;
                break;
            case ParticleType.LightningStrike:
                fusionTower = Instantiate(fusions[2]);
                projectile = ProjectileType.LightningBomb;
                break;
        }

        fusionTower.SetStats(this, otherTower);
        stats = fusionTower;
        Initialize();
        level = 7;
        Debug.Log("Sellvalue: " + sellValue.amount);
    }

    private IEnumerator AnimateShuffle()
    {

        Quaternion startRot = shuffel.transform.localRotation;
        Quaternion targetRot = Quaternion.Euler(35, 0, 0);

        // Hin-Rotation
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / ((1/attackSpeed)/5);
            shuffel.transform.localRotation = Quaternion.Lerp(startRot, targetRot, t);
            yield return null;
        }

        // Zurück-Rotation
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / ((1 / attackSpeed) / 3);
            shuffel.transform.localRotation = Quaternion.Lerp(targetRot, startRot, t);
            yield return null;
        }
    }
}
