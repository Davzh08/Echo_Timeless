using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Camera targetCamera; // 新增公共变量来指定摄像机

    void Start()
    {
        // 如果没有指定摄像机，默认使用主摄像机
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    void Update()
    {
        if (targetCamera != null)
        {
            // 使文字面向指定的摄像机
            transform.forward = targetCamera.transform.forward;
        }
    }
}