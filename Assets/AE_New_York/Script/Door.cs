using UnityEngine;

public class Door : MonoBehaviour
{
    private bool open; // Door open/close state
    public float smooth = 2.0f; // Speed of door rotation
    public float DoorOpenAngle = 90.0f; // Angle to open the door
    private Vector3 defaultRot; // Default rotation of the door
    private Vector3 openRot; // Open rotation of the door

    void Start()
    {
        defaultRot = transform.eulerAngles;
        openRot = new Vector3(defaultRot.x, defaultRot.y + DoorOpenAngle, defaultRot.z);
    }

    void Update()
    {
        // Smoothly rotate the door to the open or closed state
        if (open)
        {
            transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, openRot, Time.deltaTime * smooth);
        }
        else
        {
            transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, defaultRot, Time.deltaTime * smooth);
        }
    }

    public void ToggleDoor()
    {
        Debug.Log("Toggling Door: " + gameObject.name); // Debug info
        open = !open; // Switch the door's state
    }
}
