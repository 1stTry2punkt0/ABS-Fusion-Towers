using UnityEngine;

public static class ChainLightUser
{
    public static void PlayChain(Collider[] colliders, ChainBehavior behavior)
    {
        if (colliders.Length < 2) return; // Need at least two objects to create a chain
        for (int i = 0; i < colliders.Length - 1; i++)
        {
            Enemy startEnemy = colliders[i].GetComponent<Enemy>();
            Transform start = startEnemy.hitTransform;
            Enemy endEnemy = behavior == ChainBehavior.Random ? colliders[Random.Range(0, colliders.Length)].GetComponent<Enemy>() : colliders[i + 1].GetComponent<Enemy>();
            Transform end = endEnemy.hitTransform;
            if (start != end)
            {
                // Instantiate and initialize the chain lightning effect between start and end
                var particle = ParticleSpawnManager.instance.SpawnParticle(ParticleType.ChainLightning, Vector3.zero);

                ChainLightEffect chainEffect = particle.GetComponent<ChainLightEffect>();

                chainEffect.Initialize(start, end);
            }
        }

    }
}

public enum ChainBehavior
{
    Random,
    Ordered
}