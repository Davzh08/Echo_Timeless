using UnityEngine;
using UnityEngine.Video;

public class VideoInteract : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Reference to the VideoPlayer component
    public Camera mainCamera;       // Main camera for near plane rendering
    private bool isInteracting = false; // Tracks if the player is interacting with the video

    void Start()
    {
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer is not assigned!");
        }

        if (mainCamera == null)
        {
            Debug.LogError("MainCamera is not assigned!");
        }

        // 确保 VideoPlayer 初始状态为禁用
        videoPlayer.enabled = false;
        videoPlayer.Stop(); // 停止任何默认播放的内容
    }

    public void PlayVideo()
    {
        if (videoPlayer == null || mainCamera == null) return;

        // Enable the VideoPlayer and start playing
        videoPlayer.enabled = true;
        videoPlayer.Play();

        // Set the interaction flag
        isInteracting = true;

        // Optional: Lock player controls while video plays
        LockPlayerControls();
    }

    void Update()
    {
        // Stop the video if it's finished playing
        if (isInteracting && (!videoPlayer.isPlaying || Input.GetKeyDown(KeyCode.Escape)))
        {
            StopVideo();
        }
    }

    public void StopVideo()
    {
        if (videoPlayer == null) return;

        // Stop and disable the VideoPlayer
        videoPlayer.Stop();
        videoPlayer.enabled = false;

        // Reset the interaction flag
        isInteracting = false;

        // Unlock player controls
        UnlockPlayerControls();
    }

    private void LockPlayerControls()
    {
        // Optional: Implement logic to disable player controls while the video plays
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void UnlockPlayerControls()
    {
        // Optional: Implement logic to re-enable player controls after the video ends
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
