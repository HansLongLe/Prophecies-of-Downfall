using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartWave : MonoBehaviour
{

    [SerializeField] private GameObject enemyPrefab;

    private readonly List<GameObject> enemies = new List<GameObject>();

    public delegate void StartWaveWithoutArgs();

    public static event StartWaveWithoutArgs WaveStarted;
    public static event StartWaveWithoutArgs WaveEnded;

    private const float spawnDelay = 2f;
    private int enemiesPerWave = 3;
    // Start is called before the first frame update
    private void Start()
    {
        StartWaveUI.StartWaveEvent += SpawnEnemies;
        EnemyMovement.DiedEvent += EnemyDied;
    }

    private void SpawnEnemies()
    {
        if (this == null) return;
        if (WaveCounter.currentWave == 0)
        {
            enemiesPerWave = 2;
        }
        else
        {
            enemiesPerWave *= WaveCounter.currentWave;
        }

        WaveStarted?.Invoke();
        StartCoroutine(DelaySpawn());
    }

    private IEnumerator DelaySpawn()
    {
        var enemiesSpawned = 0;
        while (enemiesSpawned < enemiesPerWave)
        {
            var delayTimer = 0f; 
            while (delayTimer < spawnDelay)
            {
                delayTimer += Time.deltaTime;
                yield return null;
            }
            enemiesSpawned++;
            enemies.Add(Instantiate(enemyPrefab, new Vector2(20f, -1f), Quaternion.identity));
        }
    }

    private void EnemyDied(GameObject enemyObject)
    {
        enemies.Remove(enemyObject);
        if (enemies.Count != 0) return;
        enemiesPerWave = 3;
        WaveEnded?.Invoke();
    }
}
