using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartWave : MonoBehaviour
{

    [SerializeField] private GameObject enemyPrefab;
    // Start is called before the first frame update
    void Start()
    {
        StartWaveUI.StartWaveEvent += SpawnEnemies;
    }

    private void SpawnEnemies()
    {
        Instantiate(enemyPrefab, new Vector2(15f, -1f), Quaternion.identity);
    }
}
