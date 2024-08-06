using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class SpawnRecord
{
    public float time;
    public List<Transform> spawnPoints;
    public EnemyController enemyPrefab;
    public float timeToNextRecord;
    public bool spawned = false;

    [ShowInInspector, HideInEditorMode]
    public int Reward { get; set; }
}

public class SpawnTimeline : MonoBehaviour
{
    public List<SpawnRecord> timeline;
    public int remainingEnemies = 0;
    public float readyTime = 3f;

    public bool isActivated = false;
    public bool isCleared = false;

    [ShowInInspector, HideInEditorMode]
    public int TotalReward { get; set; }

    private int cnt = 0;
    private float timer = 0f;

    public void Activate()
    {
        DistributeRewardsToRecords();
        StartCoroutine(ActivateProgress());
        remainingEnemies = 0;
        cnt = 0;
        timer = 0f;
        isActivated = true;
    }

    private void DistributeRewardsToRecords()
    {
        int timelineCount = timeline.Count;
        int[] rewardDistribution = Utility.Distribute(TotalReward, timelineCount);
        for (int i = 0; i < timelineCount; ++i)
        {
            timeline[i].Reward = rewardDistribution[i];
        }
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
                    int enemyCount = timeline[cnt].spawnPoints.Count;
                    int[] coinDistribution = Utility.Distribute(timeline[cnt].Reward, enemyCount);
                    remainingEnemies += enemyCount;
                    for (int i = 0; i < enemyCount; ++i)
                    {
                        Transform trans = timeline[cnt].spawnPoints[i];
                        EnemyController enemy = Instantiate(timeline[cnt].enemyPrefab, trans.position, Quaternion.identity);
                        enemy.Reward = coinDistribution[i];
                        Debug.Log($"Assigned {enemy.Reward} coin(s) for enemy");
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
            yield return null;
        }

        isCleared = true;
        Debug.Log("Wave clear");
    }
}
