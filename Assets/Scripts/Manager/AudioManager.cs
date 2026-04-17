using UnityEngine;
using UnityEngine.Pool;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("SFX")]
    [SerializeField] private AudioSource soundFXObject; // Prefab
    private ObjectPool<AudioSource> soundFXPool;
    private AudioSource lastSoundFXObject;

    [Header("Music")]
    private AudioSource backgroundMusic;
    private bool playBackgroundMusic = true;
    Coroutine backgroundMusicCoroutine;
    [SerializeField] private AudioClip[] musicClips;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        backgroundMusic = GetComponent<AudioSource>();

        CreateSFXPool();
        if(backgroundMusicCoroutine == null)
            backgroundMusicCoroutine = StartCoroutine(BackgroundMusicLoop());
    }

    private void CreateSFXPool()
    {
        soundFXPool = new ObjectPool<AudioSource>(
            // Create
            () =>
            {
                AudioSource src = Instantiate(soundFXObject);
                src.gameObject.transform.parent = transform;
                src.gameObject.SetActive(false);
                return src;
            },

            // On Get
            src =>
            {
                src.gameObject.SetActive(true);
            },

            // On Release
            src =>
            {
                if(src != null)
                {
                    src.Stop();
                    src.clip = null;
                    src.gameObject.SetActive(false);
                }
            },

            // On Destroy
            src => Destroy(src.gameObject),

            collectionCheck: true,
            defaultCapacity: 10,
            maxSize: 1000
        );
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusicCoroutine == null)
            backgroundMusicCoroutine = StartCoroutine(BackgroundMusicLoop());
    }

    private IEnumerator BackgroundMusicLoop()
    {
        while (playBackgroundMusic)
        {
            int random = Random.Range(0, musicClips.Length);
            backgroundMusic.clip = musicClips[random];
            backgroundMusic.volume = 0.3f;
            backgroundMusic.loop = false;
            backgroundMusic.Play();
            while (backgroundMusic.isPlaying)
                yield return null;
            backgroundMusic.Stop();
            yield return new WaitForSeconds(0.5f); // kleine Sicherheitspause

        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = soundFXPool.Get();

        audioSource.transform.position = spawnTransform.position;
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();

        lastSoundFXObject = audioSource;

        StartCoroutine(ReleaseAfterDelay(audioSource, audioClip.length));
    }

    private IEnumerator ReleaseAfterDelay(AudioSource src, float delay)
    {
        yield return new WaitForSeconds(delay);
        soundFXPool.Release(src);
    }

    // Stop last SFX
    public void StopSoundFXClip()
    {
        if (lastSoundFXObject != null)
        {
            lastSoundFXObject.Stop();
            soundFXPool.Release(lastSoundFXObject);
            lastSoundFXObject = null;
        }
    }
}
