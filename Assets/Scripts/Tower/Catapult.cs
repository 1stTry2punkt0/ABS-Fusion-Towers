using UnityEngine;
using System.Collections;

public class Catapult : BaseTower
{
    private float attackCooldown;
    [SerializeField] GameObject shuffel;
    [SerializeField] GameObject bomb;

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
        ProjectileSpawnManager.instance.SpawnProjectile(ProjectileType.Bomb, this, targetEnemyData); // Spawn an arrow projectile
        bomb.SetActive(false);
        StartCoroutine(AnimateShuffle());
        // Reset cooldown
        attackCooldown = 1f / attackSpeed;
    }


    public override void OnFusion(BaseTower otherTower)
    {
        // Fusion logic comes later
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
