using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DragonNightmare : BossEnemy
{
    [SerializeField] float heal;
    public override void AuraEffect(Collider[] targets)
    {
        movementEnabled = false;
        foreach (Collider target in targets)
        {
            Enemy enemy = target.gameObject.GetComponent<Enemy>();
            enemy.currentHealth += heal;
            if(enemy.currentHealth > enemy.stats.maxHp)
                enemy.currentHealth = enemy.stats.maxHp;
            //Maybe visualize heal
        }
        Invoke("EndEffect", 2.8f);
    }

    private void EndEffect()
    {
        movementEnabled = true;
        //cancelvisualize
    }
}
