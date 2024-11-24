using UnityEngine;
using UnityEngine.Video;

public class VideoInteract : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Reference to the VideoPlayer component

    public Camera mainCamera;       // Main camera for near plane rendering
    private bool isInteracting = false; // Tracks if the player is interacting with the video

    void Start()
    {
        if (videoPlayer == null || mainCamera == null)
        {
            Debug.LogError($"VideoPlayer or MainCamera is not assigned on {gameObject.name}");
            return;
            videoPlayer.gameObject.SetActive(false); // Ensure video player is hidden initially

        }

        videoPlayer.Stop(); // ȷ����Ƶֹͣ����
    }

    public void PlayVideo()
    {
        if (videoPlayer == null || mainCamera == null) return;

        videoPlayer.Play(); // ��ʼ������Ƶ
        isInteracting = true;
        videoPlayer.gameObject.SetActive(true); // Ensure video player is hidden initially

        // ��ѡ��������ҿ���
        LockPlayerControls();
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag("Player"))
        {
            isInteracting = true; // Mark that the player is inside the trigger zone
       
        }
    }

    void Update()
    {
        if (isInteracting && Input.GetKeyDown(KeyCode.E))
        {
            PlayVideo(); // Start the video playback
        }

        if (isInteracting && (!videoPlayer.isPlaying || Input.GetKeyDown(KeyCode.Escape)))
        {
            StopVideo(); // ����Ƶ���Ž������� Escape ʱֹͣ����
        }
    }

    public void StopVideo()
    {
        if (videoPlayer == null) return;

        videoPlayer.Stop(); // ֹͣ����
        isInteracting = false;

        // �ָ���ҿ���
        UnlockPlayerControls();
    }

    private void LockPlayerControls()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void UnlockPlayerControls()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
