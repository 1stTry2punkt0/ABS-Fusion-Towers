using UnityEngine;

public class TerrorBringer : BossEnemy
{
    public override void AuraEffect(Collider[] targets)
    {
        movementEnabled = false;
        foreach (Collider target in targets)
        {
            BaseTower tower = target.gameObject.GetComponent<BaseTower>();
            targetSelection randomTargeting;
            do
            {
                randomTargeting = (targetSelection)Random.Range(0, System.Enum.GetValues(typeof(targetSelection)).Length);

            } while (randomTargeting == tower.targetSelectionType);

            tower.SetTargetSelection(randomTargeting);
            //Maybe visualize heal
            ParticleSpawnManager.instance.SpawnParticle(ParticleType.ShortDebuff, tower.transform.position + new Vector3(0, 2, 0));
        }
        Invoke("EndEffect", 2.8f);
    }

    private void EndEffect()
    {
        movementEnabled = true;
        //cancelvisualize
    }
}