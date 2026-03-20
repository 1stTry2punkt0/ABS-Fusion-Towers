using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEditor.Rendering.CameraUI;
using static UnityEngine.GraphicsBuffer;

public class ParticleSpawnManager : MonoBehaviour
{
    public static ParticleSpawnManager instance;

    [SerializeField] GameObject[] particlePrefabs;

    private ObjectPool<ParticlePoolObj> particlePool;
    private Dictionary<ParticleType, ObjectPool<ParticlePoolObj>> particlePools = new Dictionary<ParticleType, ObjectPool<ParticlePoolObj>>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        CreateDictionary();
    }
    public void CreateDictionary()
    {
        for (int i = 0; i < particlePrefabs.Length; i++)
        {
            ParticleType particleType = (ParticleType)i;
            GameObject prefab = particlePrefabs[i];
            ObjectPool<ParticlePoolObj> pool = new ObjectPool<ParticlePoolObj>(
                () => Create(prefab, particleType),
                OnGet,
                OnRelease,
                OnDestroyParticle,
                true, 10, 1000);
            particlePools.Add(particleType, pool);
        }
    }

    private ParticlePoolObj Create(GameObject prefab, ParticleType pool)
    {
        ParticlePoolObj ppo = Instantiate(prefab).GetComponent<ParticlePoolObj>();
        ppo.SetPool(particlePools[pool]);
        return ppo;
    }

    private void OnGet(ParticlePoolObj particle)
    {
        particle.gameObject.SetActive(true);
    }

    private void OnRelease(ParticlePoolObj particle)
    {
        particle.gameObject.SetActive(false);
    }

    private void OnDestroyParticle(ParticlePoolObj particle)
    {
        Destroy(gameObject);
    }

    public ParticlePoolObj SpawnParticle(ParticleType type, Vector3 pos)
    {
        if (particlePools.TryGetValue(type, out var pool))
        {
            ParticlePoolObj particle = pool.Get();
            particle.transform.position = pos;
            float delay = particle.Play();
            StartCoroutine(ReturnParticle(particle, delay));
            return particle;
        }
        return null;
    }

    private IEnumerator ReturnParticle(ParticlePoolObj particle, float delay)
    {
        yield return new WaitForSeconds(delay);
        particle.pool.Release(particle);
    }
}

public enum ParticleType
{
    DustExplosion,
}