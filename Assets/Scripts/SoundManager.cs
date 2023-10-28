using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource IngameBGM, MenuBGM, BombSource, UISFX, IngameSFX;
    public AudioClip SetBombClip, BombExplodeClip, BeingHitClip, VictoryClip, LoseClip, BtnClickClip;
    public enum AudioType
    {
        IngameBGMType,
        MenuBGMType,
        BombSetType,
        BombExplodeType,
        BeingHitType,
        VictoryType,
        LoseType,
        BtnClickType,
    }

    public void PlayAudio(AudioType type)
    {
        AudioSource targetSource = new AudioSource();
        switch (type)
        {
            
            case AudioType.IngameBGMType:
                targetSource = IngameBGM;
                MenuBGM.Stop();
                break;
            case AudioType.MenuBGMType:
                targetSource = MenuBGM;
                IngameSFX.Stop();
                break;
            case AudioType.BombSetType:
                targetSource = BombSource;
                targetSource.clip = SetBombClip;
                break;
            case AudioType.BombExplodeType:
                targetSource = BombSource;
                targetSource.clip = BombExplodeClip;
                break;
            case AudioType.BeingHitType:
                targetSource = IngameSFX;
                targetSource.clip = BeingHitClip;
                break;
            case AudioType.VictoryType:
                targetSource = IngameSFX;
                targetSource.clip = VictoryClip;
                break;
            case AudioType.LoseType:
                targetSource = IngameSFX;
                targetSource.clip = LoseClip;
                break;
            case AudioType.BtnClickType:
                targetSource = UISFX;
                targetSource.clip = BtnClickClip;
                break;
        }
        targetSource.Play();
    }
}
