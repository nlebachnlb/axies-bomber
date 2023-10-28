using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnRecord
{
    public float time;
    public List<Transform> spawnPoints;
    public EnemyController enemyPrefab;
    public float timeToNextRecord;
    public bool spawned = false;
}

public class SpawnTimeline : MonoBehaviour
{
    public List<SpawnRecord> timeline;
    public int remainingEnemies = 0;
    public float readyTime = 3f;

    public bool isActivated = false;
    public bool isCleared = false;

    private int cnt = 0;
    private float timer = 0f;

    public void Activate()
    {
        StartCoroutine(ActivateProgress());
        remainingEnemies = 0;
        cnt = 0;
        timer = 0f;
        isActivated = true;
    }

    private IEnumerator ActivateProgress()
    {
        yield return new WaitForSeconds(readyTime);
        StartCoroutine(TimelineProgression());
    }

    private IEnumerator TimelineProgression()
    {
        while (true)
        {
            if (cnt < timeline.Count)
            {
                if (!timeline[cnt].spawned)
                {
                    timer = 0f;
                    remainingEnemies += timeline[cnt].spawnPoints.Count;
                    foreach (var trans in timeline[cnt].spawnPoints)
                    {
                        EnemyController enemy = Instantiate(timeline[cnt].enemyPrefab, trans.position, Quaternion.identity);
                        enemy.onDeath = () =>
                        {
                            remainingEnemies--;
                        };
                    }
                    timeline[cnt].spawned = true;
                }
                else
                {
                    timer += Time.deltaTime;
                    if (remainingEnemies < 1 || timer >= timeline[cnt].timeToNextRecord)
                    {
                        cnt++;
                    }
                }
            }

            if (cnt >= timeline.Count && remainingEnemies < 1)
                break;
            yield return new WaitForEndOfFrame();
        }

        isCleared = true;
        Debug.Log("Wave clear");
    }
}
