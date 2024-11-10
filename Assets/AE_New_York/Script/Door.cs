using UnityEngine;

public class Door : MonoBehaviour
{
    private bool open; // open 控制门的开关状态
    public float smooth = 2.0f;
    public float DoorOpenAngle = 90.0f;
    private Vector3 defaultRot;
    private Vector3 openRot;

    void Start()
    {
        defaultRot = transform.eulerAngles;
        openRot = new Vector3(defaultRot.x, defaultRot.y + DoorOpenAngle, defaultRot.z);
    }

    void Update()
    {
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
        open = !open; // 切换门的开关状态
    }
}