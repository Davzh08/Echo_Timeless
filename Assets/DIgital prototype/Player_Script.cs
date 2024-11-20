using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb;
    public Transform head; // Target for camera follow
    public CameraFollow cameraFollow; // Reference to CameraFollow script
    public float walkSpeed;
    public float runSpeed;
    public float mouseSensitivity = 2f;
    public float interactionRange = 3f; // Player interaction range

    private float xRotation = 0f;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // 自动获取场景中的 Main Camera
        CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
        if (cameraFollow != null)
        {
            cameraFollow.SetTarget(head); // 将相机目标设置为 Head
        }
        else
        {
            Debug.LogError("CameraFollow script is not assigned or Camera.main is missing!");
        }
    }

    void Update()
    {
        // Control camera rotation
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        head.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // Handle interactions
        CheckForInteraction();
    }

    void FixedUpdate()
    {
        // Move player
        Vector3 newVelocity = Vector3.up * rb.linearVelocity.y;
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        newVelocity.x = Input.GetAxis("Horizontal") * speed;
        newVelocity.z = Input.GetAxis("Vertical") * speed;
        rb.linearVelocity = transform.TransformDirection(newVelocity);
    }

    private void CheckForInteraction()
    {
        if (Camera.main == null)
        {
            Debug.LogError("Main Camera is not found in the scene!");
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            // 处理门交互
            Door door = hit.collider.GetComponent<Door>();
            if (door != null && Input.GetKeyDown(KeyCode.E))
            {
                door.ToggleDoor();
                return;
            }

            // 处理钟表交互
            ClockInteract clock = hit.collider.GetComponent<ClockInteract>();
            if (clock != null && Input.GetKeyDown(KeyCode.E))
            {
                clock.StartClockInteraction(Camera.main.GetComponent<CameraFollow>());
                return;
            }
        }
    }
}
