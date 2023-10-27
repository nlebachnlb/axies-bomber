using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MapController : MonoBehaviour
{
    public string CurrentMapId { get; private set; }

    [SerializeField] private Transform root;
    [SerializeField] private Transform player;
    [SerializeField] private MovementController movement;
    [SerializeField] private Transform cameraRoot;

    private GameObject currentMapObject;

    private void Awake()
    {
        EventBus.onMapChange += Reload;
    }

    private void OnDestroy()
    {
        EventBus.onMapChange -= Reload;
    }

    public void Reload(string mapId)
    {
        StartCoroutine(ReloadProgress(mapId));
    }

    private IEnumerator ReloadProgress(string mapId)
    {
        AppRoot.Instance.fastTransitionController.TransitionIn();
        movement.enabled = false;
        cameraRoot.DOLocalMoveZ(2f, 0.4f);
        cameraRoot.DOLocalMoveY(2f, 0.4f);
        yield return new WaitForSeconds(0.4f);

        var pos = cameraRoot.position;
        pos.z = -2f;
        pos.y = -2f;
        cameraRoot.position = pos;

        if (currentMapObject != null)
            Destroy(currentMapObject);

        CurrentMapId = mapId;
        GameObject map = AppRoot.Instance.Config.GetMap(CurrentMapId);
        currentMapObject = Instantiate(map, root);

        player.position = map.GetComponent<SpawnController>().playerSpawn.position;
        movement.ResetInput();

        movement.enabled = true;
        AppRoot.Instance.fastTransitionController.TransitionOut();
        cameraRoot.DOLocalMoveZ(0f, 0.4f);
        cameraRoot.DOLocalMoveY(0f, 0.4f);
    }
}
