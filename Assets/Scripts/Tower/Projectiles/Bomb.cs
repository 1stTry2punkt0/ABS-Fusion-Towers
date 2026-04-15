using System.Collections;
using UnityEngine;

public class Bomb : Projectile
{
    [SerializeField] AnimationCurve curve;
    [SerializeField] float arcHeight = 4f;
    [SerializeField] ParticleType explosionEffect = ParticleType.DustExplosion;
    private Vector3 startPos;

    public override void Initialize(BaseTower tower, Enemy target)
    {
        base.Initialize(tower, target);
        startPos = transform.position;
        targetPosition += target.transform.forward;
        targetPosition.y = 1;
        arcHeight = Mathf.Max(3, Vector3.Distance(startPos, targetPosition) /2); // Adjust arc height based on distance
    }

    public override void SetTarget()
    {

    }

    protected override bool FindNextTarget()
    {
        ParticlePoolObj explosion = ParticleSpawnManager.instance.SpawnParticle(explosionEffect, transform.position);
        PlayHitSound();
        explosion.ScaleEffect(parentTower.damageRange / 2);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, parentTower.damageRange, GameManager.instance.enemyLayer);
        float multiplier = 1;
        if (explosionEffect != ParticleType.LightningExplosion)
        {
            explosion.GetComponent<ElementalGround>()?.Initialize(parentTower);
        }
        else
        {
            ChainLightUser.PlayChain(hitColliders, ChainBehavior.Ordered);
            multiplier += hitColliders.Length * 0.1f; // Increase damage by 10% for each additional enemy hit
            Debug.Log("Damage multiplier: " + multiplier);
        }

        foreach (Collider hit in hitColliders)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy == null || enemy == targetEnemy)
            {
                if (enemy == targetEnemy && explosionEffect == ParticleType.LightningExplosion)
                    parentTower.TargetHit(enemy, multiplier -1);
                continue;
            }
            parentTower.TargetHit(enemy, multiplier);
        }
        return false;
    }

    protected override void Update()
    {
        // 1. Horizontale Bewegung wie vorher
        Vector3 flatStart = startPos;
        Vector3 flatTarget = targetPosition;

        flatStart.y = 0;
        flatTarget.y = 0;

        Vector3 flatDir = (flatTarget - flatStart).normalized;

        // horizontale Bewegung
        transform.position += flatDir * speed * Time.deltaTime;

        // 2. Fortschritt entlang der Strecke (0–1)
        float traveled = Vector3.Distance(flatStart, new Vector3(transform.position.x, 0, transform.position.z));
        float total = Vector3.Distance(flatStart, flatTarget);
        float t = Mathf.Clamp01(traveled / total);

        // 3. Parabelhöhe
        float height = curve.Evaluate(t) * arcHeight;

        // 4. Position entlang der Linie + Höhe
        Vector3 pos = transform.position;

        // Interpolierte Grundhöhe zwischen Start und Ziel
        float baseY = Mathf.Lerp(startPos.y, targetPosition.y, t);

        pos.y = baseY + height;
        transform.position = pos;

        // 5. Rotation optional
        transform.rotation = Quaternion.LookRotation(flatDir);

        // Hit detection
        if (Vector3.Distance(transform.position, targetPosition) < 0.2f)
        {
            if (targetEnemy != null || !targetEnemy.isDead)
            {
                parentTower.TargetHit(targetEnemy);
            }
            if (!FindNextTarget())
                Disable();// Return to pool after hitting the target
        }
    }
}
