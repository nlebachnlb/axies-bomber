using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Module.MapGeneration.Type;

public class SpawnController : MonoBehaviour
{
    public Transform playerSpawn;
    public List<SpawnTimeline> spawnWaves;
    public bool rewardAbilityPool;
    public GameObject exitGate;
    public SkillPoolEntrance poolEntrance;
    public int coin = 50;

    private int currentWave = 0;

    public void OnEnterRoom()
    {
        StartCoroutine(WaveProgression());
    }

    private void Awake()
    {
        EventBus.onPickSkill += OnPickSkill;
    }

    private void OnDestroy()
    {
        EventBus.onPickSkill -= OnPickSkill;
    }

    private void Start()
    {
        DistributeCoinsToWaves();
        // StartCoroutine(WaveProgression());
    }

    private void DistributeCoinsToWaves()
    {
        int waveCount = spawnWaves.Count;
        int[] coinDistribution = Utility.Distribute(coin, waveCount);
        for (int i = 0; i < waveCount; ++i)
        {
            int coin = coinDistribution[i];
            spawnWaves[i].TotalReward = coin;
            Debug.Log($"Wave {i} receives {coin} coin(s)");
        }
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
        StartCoroutine(ClearProgression());
    }

    private IEnumerator ClearProgression()
    {
        Time.timeScale = 0.4f;
        yield return new WaitForSecondsRealtime(0.8f);

        EventBus.RaiseOnRoomClear();
        Time.timeScale = 1f;

        yield return new WaitForSecondsRealtime(2f);
        // SkillPoolEntrance entrance = Instantiate(poolEntrance, playerSpawn.transform.position, Quaternion.identity);
        // entrance.isAbilityPool = rewardAbilityPool;
    }

    private void OnPickSkill(SkillConfig skill)
    {
        StartCoroutine(OpenGateProgression());
    }

    private IEnumerator OpenGateProgression()
    {
        yield return new WaitForSeconds(1.2f);
        exitGate.transform.DOMoveY(-4f, 1.5f).SetEase(Ease.OutBack);
        exitGate.GetComponent<Collider>().enabled = false;
    }
}
