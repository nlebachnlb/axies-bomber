using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapRoom : MonoBehaviour
{
    [SerializeField] GameObject topDoor, bottomDoor, leftDoor, rightDoor;
    public Vector2Int RoomIndex { get; set; }

    public void OpenDoor(Vector2Int direction)
    {
        if (direction == Vector2Int.up) topDoor.SetActive(true);
        if (direction == Vector2Int.down) bottomDoor.SetActive(true);
        if (direction == Vector2Int.left) leftDoor.SetActive(true);
        if (direction == Vector2Int.right) rightDoor.SetActive(true);
    }
}
