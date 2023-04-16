using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartWave : MonoBehaviour
{

    [SerializeField] private GameObject enemyPrefab;

    private List<GameObject> enemies = new List<GameObject>();

    public delegate void StartWaveWithoutArgs();

    public static event StartWaveWithoutArgs WaveStarted;
    public static event StartWaveWithoutArgs WaveEnded;

    private const float spawnDelay = 2f;
    private int enemiesPerWave;
    // Start is called before the first frame update
    void Start()
    {
        StartWaveUI.StartWaveEvent += SpawnEnemies;
        EnemyMovement.DiedEvent += EnemyDied;
    }

    private void SpawnEnemies()
    {
        WaveStarted?.Invoke();
        enemiesPerWave += 3 * WaveCounter.currentWave;
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
        if (enemies.Count == 0)
        {
            WaveEnded?.Invoke();
        }
    }
}
