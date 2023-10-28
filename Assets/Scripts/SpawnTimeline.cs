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

    private int cnt = 0;
    private float timer = 0f;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);
        Activate();
    }

    public void Activate()
    {
        StartCoroutine(TimelineProgression());
        remainingEnemies = 0;
        cnt = 0;
        timer = 0f;
    }

    private IEnumerator TimelineProgression()
    {
        while (true)
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
            else if (remainingEnemies > 0 || timer < timeline[cnt].timeToNextRecord)
            {
                timer += Time.deltaTime;
            }
            else
            {
                cnt++;
            }

            if (cnt >= timeline.Count)
                break;
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("Wave clear");
    }
}
