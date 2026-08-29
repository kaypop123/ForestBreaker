using UnityEngine;

public class SmoothFollowCam : MonoBehaviour
{
    [SerializeField] private Transform currentCenterPoint;
    [SerializeField] private float smoothTime = 0.2f;

    private Vector3 velocity = Vector3.zero;

    public void SetCenterPoint(Transform newCenterPoint)
    {
        currentCenterPoint = newCenterPoint;

        if (currentCenterPoint != null)
        {
            Debug.Log($"카메라 중심점 변경됨: {currentCenterPoint.position}");
        }
    }

    private void LateUpdate()
    {
        if (currentCenterPoint == null) return;

        Vector3 desiredPos = new Vector3(
            currentCenterPoint.position.x,
            currentCenterPoint.position.y,
            transform.position.z
        );

        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPos,
            ref velocity,
            smoothTime
        );
    }
}