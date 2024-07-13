using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class JumpController : MonoBehaviour
{
    public float jumpPower = 1;
    public float duration = 1f;

    public bool IsJumping { get; private set; }

    private new Rigidbody rigidbody;
    private MovementController movementController;

    private void Awake()
    {
        movementController = GetComponent<MovementController>();
        rigidbody = GetComponent<Rigidbody>();
    }

    public void Jump(Vector3 target)
    {
        IsJumping = true;
        rigidbody.isKinematic = true;
        movementController.enabled = false;

        transform.DOJump(target, jumpPower, 1, duration).OnComplete(() =>
        {
            rigidbody.isKinematic = false;
            movementController.enabled = true;
            IsJumping = false;
        });
    }
}
