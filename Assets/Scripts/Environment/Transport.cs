using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Transport : MonoBehaviour
{
    public Vector3 Destination { get; set; }
    public Vector3 DestinationSpawnPoint { get; set; }
    public int TargetRoomId { get; set; }
    
    [SerializeField] private Transform visual;
    
    public float TransportPlayer(MovementController playerMovement)
    {
        StartCoroutine(DoTransportPlayer(Destination, playerMovement, DestinationSpawnPoint));
        return 4f;
    }

    public void DoEntrance(Vector3 gatePosition)
    {
        transform.position = gatePosition;
        transform.Translate(Vector3.down * 10f);
        transform.DOMoveY(0f, 1f).SetEase(Ease.OutBack);
    }

    private void Awake()
    {
        EventBus.Instance.LeaveToRoomEvent += OnLeaveToRoom;
    }

    private void OnDestroy()
    {
        EventBus.Instance.LeaveToRoomEvent -= OnLeaveToRoom;
    }

    private void OnLeaveToRoom(int roomId)
    {
        if (TargetRoomId != roomId)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator DoTransportPlayer(Vector3 destination, MovementController playerMovement, Vector3 spawnPosition)
    {
        playerMovement.SetColliderActive(false);
        playerMovement.ResetInput();
        playerMovement.movementPermission = MovementPermission.Auto;
        var player = playerMovement.transform;
        var position = transform.position;
        
        player.position = new Vector3(position.x, player.position.y, position.z);
        transform.DOMove(destination, 2f);
        playerMovement.transform.DOMove(new Vector3(destination.x, player.position.y, destination.z), 2f);
        
        yield return new WaitForSeconds(2f);
        
        playerMovement.movementPermission = MovementPermission.Player;
        player.position = spawnPosition;
        playerMovement.SetColliderActive(true);

        transform.DOMoveY(-10f, 1f).SetEase(Ease.InBack);

        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
