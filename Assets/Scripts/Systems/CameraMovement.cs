using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Target & Follow")]
    public Transform target;
    public float followSpeed = 5f;

    [Header("Mouse Influence")]
    public float mouseSensitivity = 0.5f;
    public float maxMouseDistance = 3f;

    [Header("Camera Distance")]
    public float cameraDistance = 10f;

    private Camera cam;
    private Vector3 smoothVelocity;

    private bool forceFocusOnTarget = false;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition;

        if (forceFocusOnTarget)
        {
            desiredPosition = target.position;
        }
        else
        {
            Vector3 mouseScreenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraDistance);
            Vector3 mouseWorldPos = cam.ScreenToWorldPoint(mouseScreenPos);

            Vector3 targetToMouse = mouseWorldPos - target.position;
            targetToMouse.z = 0;

            if (targetToMouse.magnitude > maxMouseDistance)
            {
                targetToMouse = targetToMouse.normalized * maxMouseDistance;
            }

            desiredPosition = target.position + (targetToMouse * mouseSensitivity);
        }

        desiredPosition.z = target.position.z - cameraDistance;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref smoothVelocity,
            followSpeed * Time.deltaTime
        );
    }

    public void FocusOnlyOnTarget(bool Bool)
    {
        forceFocusOnTarget = Bool;
    }
}
