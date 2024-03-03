using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public List<Transform> spawnPoints;
    public List<int> enemiesPerWave;

    [Range(0, 10)]public float spawnInterval = 2;
    [Range(0, 10)]public float timeBetweenWaves = 5;

    int enemiesLeft;
    int wave = 0;

    public UnityEvent onSpawn;
    public UnityEvent<int> onWaveStart;
    public UnityEvent<int> onWaveEnd;
    public UnityEvent onWavesCleared;

    public void Spawn()
    {
        var point = spawnPoints[Random.Range(0, spawnPoints.Count)];
        Instantiate(prefab, point.position, point.rotation);
        onSpawn.Invoke();
    }

    private async void Start()
    {
        foreach (var count in enemiesPerWave)
        {
            enemiesLeft = count;
            onWaveStart.Invoke(wave);

            while (enemiesLeft > 0)
            {
                await new WaitForSeconds(spawnInterval);
                Spawn();
                enemiesLeft--;
            }

            onWaveEnd.Invoke(wave);
            wave++;
            await new WaitForSeconds(timeBetweenWaves);
        }
        
        onWavesCleared.Invoke();
    }
}
