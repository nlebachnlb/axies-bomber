using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private float shakeValue = 0f;
    private float timeLeft = 0f;

    public void Shake(float duration, float amplitude)
    {
        shakeValue = amplitude;
        timeLeft = duration;
    }

    void Update()
    {
        if (shakeValue > 0f)
        {
            float speed = shakeValue / timeLeft;
            shakeValue -= speed * Time.deltaTime;
            float randX = Random.Range(-shakeValue * 0.5f, shakeValue * 0.5f);
            float randY = Random.Range(-shakeValue * 0.5f, shakeValue * 0.5f);
            float randZ = Random.Range(-shakeValue * 0.5f, shakeValue * 0.5f);
            transform.position = new Vector3(randX, randY, randZ);
        }
    }
}
