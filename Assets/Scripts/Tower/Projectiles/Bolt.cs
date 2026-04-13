using UnityEngine;

public class Bolt : Projectile
{
    public int pearcing = 2;

    public override void Initialize(BaseTower tower, Enemy target)
    {
        base.Initialize(tower, target);
        pearcing = 2;
    }

    public override void SetTarget()
    {
        if (targetEnemy == null || targetEnemy.isDead)
        {
            if(FindNextTarget()) return;
            targetEnemy = null;
            targetPosition.y = 1f; // Default height if no target or target is dead
            return;
        }
        targetPosition = targetEnemy.hitTransform.position;
    }

    protected override bool FindNextTarget()
    {
        if (pearcing <= 0) return false;

        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, parentTower.damageRange, GameManager.instance.enemyLayer);
        Debug.Log(hits.Length);
        if(hits.Length == 0) return false;

        System.Array.Sort(hits, (a,b) => a.distance.CompareTo(b.distance));

        foreach(RaycastHit hit in hits)
        {
            Enemy e = hit.collider.gameObject.GetComponent<Enemy>();
            if (e == targetEnemy) continue;
            if (e == null) continue;
            pearcing--;
            targetEnemy = e;
            return true;
        }
        return false;
    }
}
