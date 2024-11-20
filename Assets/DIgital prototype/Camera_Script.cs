using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The target the camera follows
    public float followSpeed = 5f;

    private Vector3 offset;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("CameraFollow: Target is not assigned!");
            return;
        }

        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

            transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, followSpeed * Time.deltaTime);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        if (newTarget == null)
        {
            Debug.LogError("CameraFollow: New target is null!");
            return;
        }

        target = newTarget;
    }
}
