using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;
    [SerializeField] Wave[] waves;

    public bool autoplay = true;
    public bool startAvailable = true;
    [SerializeField] GameObject startAvailableImage;
    public int currentWave = 0;
    public int lastWave = 12;
    private List<Coroutine> SpawnRoutines = new List<Coroutine>();
    public int enemyCount = 0;

    [SerializeField] Merchant merchant;
    [SerializeField] TMPro.TextMeshProUGUI waveUI;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else 
            Destroy(gameObject);
    }

    public void NewGame()
    {
        foreach(EnemyGroup group in waves[currentWave].enemyGroups)
        {
            EnemySpawnManager.instance.ResetAllEnemies(group.enemyType);
        }
        merchant.StopMerchant();

        currentWave = 0;
        lastWave = waves.Length;
        startAvailable = true;
        startAvailableImage.SetActive(true);
        UpdateUI(currentWave);
        SpawnRoutines.Clear();
        StopAllCoroutines();
    }

    private void UpdateUI(int number)
    {
        waveUI.text = number.ToString() + "/" + lastWave.ToString();

    }

    public void StartWave()
    {
        if(GameManager.instance.gameState != GameState.Fighting)
        {
            GameManager.instance.gameState = GameState.Fighting;
        }
        if (!startAvailable) return;
        startAvailable = false;
        startAvailableImage.SetActive(false);
        UpdateUI(currentWave + 1);
        GameManager.instance.UpgradeEnemys();
        merchant.StartRound();
        foreach (EnemyGroup group in waves[currentWave].enemyGroups)
        {
            if (group.boss)
            {
                //maybe something else
            }
            Coroutine c = StartCoroutine(SpawnWaveGroup(group));
            SpawnRoutines.Add(c);
        }
        faithRoutine = StartCoroutine(FaithGain());
    }

    public void EndWave()
    {
        foreach(Coroutine c in SpawnRoutines)
        {
            StopCoroutine(c);
        }
        SpawnRoutines.Clear();
        currentWave++;
        StartCoroutine(WaitForEnemysDeath());
    }

    private IEnumerator WaitForEnemysDeath()
    {
        while (enemyCount > 0)
        {
            yield return new WaitForSeconds(2f);
        }
        StopCoroutine(faithRoutine);
        if (currentWave < waves.Length)
        {
            startAvailable = true;
            startAvailableImage.SetActive(true);
            if (!autoplay) yield break;
            yield return new WaitForSeconds(1f);
            StartWave();
        }
        else
        {
            Debug.Log("Victory!");
            GameManager.instance.EndGame(true);
        }
    }

    private IEnumerator SpawnWaveGroup(EnemyGroup group)
    {
        yield return new WaitForSeconds(waves[currentWave].delay + group.firstSpawnDelay);
        while(true)
        {
            int count = 0;
            while(count < group.groupSize)
            {
                count++;
                enemyCount++;
                EnemySpawnManager.instance.SpawnEnemy(group.enemyType);
                yield return new WaitForSeconds(group.spawnInterval);
            }
            yield return new WaitForSeconds(group.groupInterval);
            if(group.boss)yield break;
        }
    }

    [SerializeField] Cost faithProducion;
    [SerializeField] float productionInterval = 5f;
    private Coroutine faithRoutine;
    private IEnumerator FaithGain()
    {
        while(true)
        {
            yield return new WaitForSeconds(productionInterval);
            RessourceManager.instance.GainRessource(faithProducion);
        }
    }
}

[System.Serializable]
public class Wave
{
    public float delay;
    public EnemyGroup[] enemyGroups;
}

[System.Serializable]
public class EnemyGroup
{
    public EnemyType enemyType;
    public float firstSpawnDelay;
    public float spawnInterval;
    public float groupInterval;
    public int groupSize;
    public bool boss;
}