using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class JumpController : MonoBehaviour
{
    [SerializeField] CameraFollow cameraFollow;

    public bool IsJumping { get; private set; }

    private new Rigidbody rigidbody;
    private MovementController movementController;

    private void Awake()
    {
        movementController = GetComponent<MovementController>();
        rigidbody = GetComponent<Rigidbody>();
    }

    public void Jump(Vector3 target, float jumpPower, float duration, Action onCompleted = null)
    {
        IsJumping = true;
        rigidbody.isKinematic = true;
        movementController.enabled = false;
        cameraFollow.freezeY = true;

        transform.DOJump(target, jumpPower, 1, duration).OnComplete(() =>
        {
            rigidbody.isKinematic = false;
            movementController.enabled = true;
            cameraFollow.freezeY = false;
            IsJumping = false;
            onCompleted?.Invoke();
        });
    }
}
