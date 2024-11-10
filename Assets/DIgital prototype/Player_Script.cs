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
    public float interactionRange = 3f; // 玩家与门的交互距离

    private float xRotation = 0f;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // 控制摄像机视角
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        head.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);

        // 检测门的交互
        CheckForDoorInteraction();
    }

    void FixedUpdate()
    {
        Vector3 newVelocity = Vector3.up * rb.linearVelocity.y;
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        newVelocity.x = Input.GetAxis("Horizontal") * speed;
        newVelocity.z = Input.GetAxis("Vertical") * speed;
        rb.linearVelocity = transform.TransformDirection(newVelocity);
    }

    private void CheckForDoorInteraction()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2)); // 从屏幕中心发射射线
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            Door door = hit.collider.GetComponent<Door>();
            if (door != null && Input.GetKeyDown(KeyCode.E))
            {
                door.ToggleDoor(); // 按下E时调用Door的开关方法
            }
        }
    }
}