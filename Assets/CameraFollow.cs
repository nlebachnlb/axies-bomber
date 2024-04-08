using UnityEngine;

public class CameraFollow : MonoBehaviour

{
    public Transform target;
    public float smoothSpeed = 0.125f;

    public Vector3 minPosition, maxPosition;

    public void BoundWithRoom(Room room)
    {
        minPosition = room.MinPosition;
        maxPosition = room.MaxPosition;
    }
    
    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position;
        if (desiredPosition.x < minPosition.x)
            desiredPosition.x = minPosition.x;
        else if (desiredPosition.x > maxPosition.x)
            desiredPosition.x = maxPosition.x;

        if (desiredPosition.z < minPosition.z)
            desiredPosition.z = minPosition.z;
        else if (desiredPosition.z > maxPosition.z)
            desiredPosition.z = maxPosition.z;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}