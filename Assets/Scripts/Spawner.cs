using System;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public static event Action<int> onWaveChange;
    [SerializeField] private WaveData[] waves;
    private int currentWaveIndex = 0;
    private float spawnCounter;
    private int enemiesRemoved;
    private float spawnTimer;
    private float timeBetweenWaves = 2f;
    private float waveCooldown;
    private int waveCounter = 0;
    private bool isBetweenWaves = false;
    private WaveData currentWave => waves[currentWaveIndex];
    [SerializeField] private ObjectPooling lightEnemy;
    [SerializeField] private ObjectPooling mediumEnemy;
    [SerializeField] private ObjectPooling heavyEnemy;

    private Dictionary<EnemyType, ObjectPooling> poolDictionary;

    private void Awake()
    {
        poolDictionary = new Dictionary<EnemyType, ObjectPooling>()
        {
            {EnemyType.LightEnemy, lightEnemy},
            {EnemyType.MediumEnemy, mediumEnemy},
            {EnemyType.HeavyEnemy, heavyEnemy}
        };
    }

    private void OnEnable()
    {
        Enemy.onEnemyReachedEnd += HandleEnemyReachEnd;
        Enemy.onEnemyDestroyed += HandleEnemyDestroyed;
    }
    private void OnDisable()
    {
        Enemy.onEnemyReachedEnd -= HandleEnemyReachEnd;
        Enemy.onEnemyDestroyed -= HandleEnemyDestroyed;

    }

    private void Start()
    {
        onWaveChange?.Invoke(waveCounter);
    }

    void Update()
    {
        if (isBetweenWaves)
        {
            waveCooldown -= Time.deltaTime;
            if(waveCooldown <= 0 )
            {
                currentWaveIndex = (currentWaveIndex + 1) % waves.Length;
                waveCounter++;
                onWaveChange?.Invoke(waveCounter);
                spawnCounter = 0;
                enemiesRemoved = 0;
                spawnTimer = 0;
                isBetweenWaves = false;
            }
        }
        else
        {
            // Spawn timer interval
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0 && spawnCounter < currentWave.enemiesPerWave)
            {
                spawnTimer = currentWave.spawnInterval;
                SpawnEnemy();
                spawnCounter++;
            }
            else if (spawnCounter >= currentWave.enemiesPerWave && enemiesRemoved >= currentWave.enemiesPerWave)
            {
                isBetweenWaves = true;
                waveCooldown = timeBetweenWaves;
            }
        }
    }

    private void SpawnEnemy()
    {
        if (poolDictionary.TryGetValue(currentWave.enemyType, out var pool))
        {
        //Grabbing an enemy prefab and spawning it in the game 
        GameObject spawnedObject = pool.GetPooledObject();
        spawnedObject.transform.position = transform.position;
        spawnedObject.SetActive(true);

            float healthMultiplyer = 1f + (waveCounter * 0.1f);
            Enemy enemy = spawnedObject.GetComponent<Enemy>();
            enemy.Initialize(healthMultiplyer);
        }
    }


    private void HandleEnemyReachEnd(EnemyStats data)
    {
        enemiesRemoved++;
    }
    private void HandleEnemyDestroyed(Enemy enemy)
    {
        enemiesRemoved++;
    }
}
