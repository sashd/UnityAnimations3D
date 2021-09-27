using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    public Vector3 Offset;
    public float SmoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;
 
    private void LateUpdate()
    {
        Vector3 targetPosition = Target.position + Offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, SmoothTime);
    }
}