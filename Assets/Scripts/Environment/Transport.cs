using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Transport : MonoBehaviour
{
    [SerializeField] private Transform visual;
    
    private HashSet<(int, GateWayDirection)> registeredRoomIds = new();

    public void RegisterRoom(int roomId, GateWayDirection gateWay)
    {
        registeredRoomIds.Add((roomId, gateWay));
    }

    public void UnregisterRoom(int roomId, GateWayDirection gateWay)
    {
        registeredRoomIds.Remove((roomId, gateWay));
    }
    
    public void TransportPlayerTo(Vector3 destination, MovementController playerMovement, Vector3 spawnPosition)
    {
        StartCoroutine(DoTransportPlayer(destination, playerMovement, spawnPosition));
    }

    public void OnCalledByRoom(int callerRoomId, GateWayDirection gateWayDirection, Vector3 gatePosition)
    {
        if (!registeredRoomIds.Contains((callerRoomId, gateWayDirection))) return;

        gameObject.SetActive(true);
        transform.position = gatePosition;
        transform.Translate(Vector3.down * 10f);
        transform.DOMoveY(0f, 1f).SetEase(Ease.OutBack);
    }

    private IEnumerator DoTransportPlayer(Vector3 destination, MovementController playerMovement, Vector3 spawnPosition)
    {
        playerMovement.SetColliderActive(false);
        playerMovement.movementPermission = MovementPermission.Auto;
        var player = playerMovement.transform;
        var position = transform.position;
        
        player.position = new Vector3(position.x, player.position.y, position.z);
        transform.DOMove(destination, 3f);
        playerMovement.transform.DOMove(new Vector3(destination.x, player.position.y, destination.z), 3f);
        yield return new WaitForSeconds(3f);
        playerMovement.movementPermission = MovementPermission.Player;
        player.position = spawnPosition;
        playerMovement.SetColliderActive(true);
    }
}
