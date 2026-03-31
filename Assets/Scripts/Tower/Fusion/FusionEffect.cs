using System.Collections;
using UnityEngine;

public class FusionEffect : MonoBehaviour
{
    [SerializeField] float spawnEffectTime = 3;
    [SerializeField] AnimationCurve fadeIn;

    [SerializeField] ParticleSystem ps;
    float timer = 0;
    [SerializeField] Renderer _renderer;

    int shaderProperty;
    [SerializeField] float speed = 60f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shaderProperty = Shader.PropertyToID("_cutoff");

        var main = ps.main;
        main.duration = spawnEffectTime;

        ps.Play();
        StartCoroutine(FuseEffect());
    }

    void Update()
    {
        transform.Rotate(0f, speed * Time.deltaTime, 0f);
    }


    private IEnumerator FuseEffect()
    {
        while (timer < spawnEffectTime)
        {
            timer += Time.deltaTime;
            _renderer.material.SetFloat(shaderProperty, fadeIn.Evaluate(Mathf.InverseLerp(0, spawnEffectTime, timer)));
            yield return null;
        }
    }
}
