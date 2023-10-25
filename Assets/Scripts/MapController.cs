using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public string CurrentMapId { get; private set; }

    [SerializeField] private Transform root;

    public void Reload(string mapId)
    {
        CurrentMapId = mapId;
        GameObject map = AppRoot.Instance.Config.GetMap(CurrentMapId);
        Instantiate(map, root);
    }
}
