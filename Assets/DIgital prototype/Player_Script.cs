using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb;
    public Transform head;
    public Camera camera;

    [Header("Configurations")]
    public float walkSpeed;
    public float runSpeed;
    public float mouseSensitivity = 2f;
    public float interactionRange = 3f; // Player interaction range

    private float xRotation = 0f;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (!camera.transform.IsChildOf(head)) return; // Skip camera rotation if detached
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        head.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);

        CheckForInteraction();
    }

    void FixedUpdate()
    {
        Vector3 newVelocity = Vector3.up * rb.linearVelocity.y;
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        newVelocity.x = Input.GetAxis("Horizontal") * speed;
        newVelocity.z = Input.GetAxis("Vertical") * speed;
        rb.linearVelocity = transform.TransformDirection(newVelocity);
    }

    private void CheckForInteraction()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Door door = hit.collider.GetComponent<Door>();
                if (door != null)
                {
                    door.ToggleDoor();
                    return;
                }

                ClockInteract clock = hit.collider.GetComponent<ClockInteract>();
                if (clock != null)
                {
                    clock.StartClockInteraction(camera); // Send the camera to ClockInteract
                }
            }
        }
    }
}
