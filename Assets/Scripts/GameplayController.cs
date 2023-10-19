using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    [Header("Player")]
    public AxieConfigReader playerConfig;
    public StatsModifier statsModifier;

    [Header("Config & Stats")]
    public AxieConfig axieConfig;
    public AxieStats axieBaseStats;
    public BombStats bombBaseStats;

    [Header("Camera")]
    [SerializeField] private CameraShake cameraShaker;

    private void Awake()
    {
        EventBus.onBombFuse += PlayShake;
    }

    private void Start()
    {
        playerConfig.Load(axieConfig);
        statsModifier.axieStats = axieBaseStats;
        statsModifier.bombStats = bombBaseStats;
    }

    public void PlayShake()
    {
        cameraShaker.Shake(0.2f, 0.2f);
    }
}
