using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public Transform playerSpawn;
    public List<SpawnTimeline> spawnWaves;

    private int currentWave = 0;

    private void Start()
    {
        StartCoroutine(WaveProgression());
    }

    private IEnumerator WaveProgression()
    {
        while (true)
        {
            var curWave = spawnWaves[currentWave];
            if (!curWave.isActivated)
            {
                curWave.Activate();
            }

            if (curWave.isCleared)
            {
                curWave.gameObject.SetActive(false);
                currentWave++;
            }

            if (currentWave >= spawnWaves.Count)
                break;

            yield return new WaitForEndOfFrame();
        }

        Debug.Log("Room clear");
    }
}
