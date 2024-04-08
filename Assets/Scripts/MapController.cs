using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Module.MapGeneration.Controller;
using Module.MapGeneration.Data;
using Module.MapGeneration.View;

public class MapController : MonoBehaviour
{
    public int CurrentRoomId { get; private set; }

    [SerializeField] private MapData dataModel;
    [SerializeField] private MapView mapView;
    [SerializeField] private Transform root;
    [SerializeField] private Transform player;
    [SerializeField] private MovementController movement;
    [SerializeField] private Transform cameraRoot;
    [SerializeField] private MapGenerator mapGenerator;

    private GameObject currentMapObject;

    public void LoadMap()
    {
        mapGenerator.StartRoomGeneration();
    }
    
    private void Awake()
    {
        EventBus.onRoomChange += OnRoomChange;
        EventBus.Instance.EnterRoomEvent += OnEnterRoom;
    }
    
    private void OnDestroy()
    {
        EventBus.onRoomChange -= OnRoomChange;
        EventBus.Instance.EnterRoomEvent -= OnEnterRoom;
    }

    private void OnEnterRoom(int roomId)
    {
        mapView.OnEnterRoom(roomId);
        CurrentRoomId = roomId;
    }
    
    private void OnRoomChange(int roomId, Vector2Int fromDirection)
    {
        mapView.OnRoomChange(CurrentRoomId, roomId, fromDirection);
        CurrentRoomId = roomId;
        // StartCoroutine(ReloadProgress(roomId));
    }

    private IEnumerator ReloadProgress(int mapId)
    {
        AppRoot.Instance.fastTransitionController.TransitionIn();
        
        // movement.enabled = false;
        // cameraRoot.DOLocalMoveZ(2f, 0.4f);
        // cameraRoot.DOLocalMoveY(2f, 0.4f);
        yield return new WaitForSeconds(0.4f);
        //
        // var pos = cameraRoot.position;
        // pos.z = -2f;
        // pos.y = -2f;
        // cameraRoot.position = pos;
        //
        // if (currentMapObject != null)
        //     Destroy(currentMapObject);

        // CurrentMapId = mapId;
        // GameObject map = AppRoot.Instance.Config.GetMap(CurrentMapId);
        // currentMapObject = Instantiate(map, root);
        //
        // player.position = map.GetComponent<SpawnController>().playerSpawn.position;
        // movement.ResetInput();
        //
        // movement.enabled = true;
        // AppRoot.Instance.fastTransitionController.TransitionOut();
        // cameraRoot.DOLocalMoveZ(0f, 0.4f);
        // cameraRoot.DOLocalMoveY(0f, 0.4f);
    }
}
