using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seal : MonoBehaviour
{
    private Collider col;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        EventBus.Instance.RoomSealedEvent += OnRoomSealed;
        EventBus.Instance.RoomUnsealedEvent += OnRoomUnsealed;
        col = GetComponent<Collider>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnDestroy()
    {
        EventBus.Instance.RoomSealedEvent -= OnRoomSealed;
        EventBus.Instance.RoomUnsealedEvent -= OnRoomUnsealed;
    }

    private void OnRoomSealed(int roomId)
    {
        col.enabled = true;
        meshRenderer.enabled = true;
    }

    private void OnRoomUnsealed(int roomId)
    {
        col.enabled = false;
        meshRenderer.enabled = false;
    }
}
