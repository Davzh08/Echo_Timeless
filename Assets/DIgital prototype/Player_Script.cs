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

        if (Camera.main == null)
        {
            Debug.LogError("No MainCamera found in the scene!");
        }

        // Ensure CameraFollow is correctly initialized
        if (cameraFollow != null)
        {
            cameraFollow.SetTarget(head);
        }
        else
        {
            Debug.LogError("CameraFollow script is not assigned in PlayerController!");
        }
    }

    void Update()
    {
        // �����������ת
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        head.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // ��������
        CheckForInteraction();
    }

    void FixedUpdate()
    {
        // ����ƶ�
        Vector3 newVelocity = Vector3.up * rb.linearVelocity.y;
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        newVelocity.x = Input.GetAxis("Horizontal") * speed;
        newVelocity.z = Input.GetAxis("Vertical") * speed;
        rb.linearVelocity = transform.TransformDirection(newVelocity);
    }

    private void CheckForInteraction()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            Debug.Log($"Raycast hit: {hit.collider.name}"); // 输出命中对象名称

            Door door = hit.collider.GetComponent<Door>();
            if (door != null && Input.GetKeyDown(KeyCode.E))
            {
                door.ToggleDoor();
                return;
            }

            ClockInteract clock = hit.collider.GetComponent<ClockInteract>();
            if (clock != null && Input.GetKeyDown(KeyCode.E))
            {
                clock.StartClockInteraction(cameraFollow);
                return;
            }

            VideoInteract video = hit.collider.GetComponent<VideoInteract>();
            if (video != null && Input.GetKeyDown(KeyCode.E))
            {
                video.PlayVideo();
                return;
            }

            PictureInteract picture = hit.collider.GetComponent<PictureInteract>();
            if (picture != null && Input.GetKeyDown(KeyCode.E))
            {
                picture.StartPictureInteraction(cameraFollow);
                return;
            }
        }
        else
        {
            Debug.Log("No object hit within interaction range.");
        }
    }
}