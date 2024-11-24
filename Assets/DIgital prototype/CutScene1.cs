using UnityEngine;
using UnityEngine.Video;

public class VideoInteract1 : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Reference to the VideoPlayer component


    private bool isInteracting = false; // Tracks if the player is interacting with the video

    void Start()
    {
        if (videoPlayer == null)
        {
            Debug.LogError($"VideoPlayer or MainCamera is not assigned on {gameObject.name}");
            return;
            videoPlayer.gameObject.SetActive(false); // Ensure video player is hidden initially

        }
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
            StopVideo(); // 当视频播放结束或按下 Escape 时停止播放
        }
    }

    void PlayVideo()
    {
        if (videoPlayer == null) return;
        {
            videoPlayer.gameObject.SetActive(true); // Activate the video player
            videoPlayer.Play(); // 开始播放视频
        }
        
       
        // 可选：禁用玩家控制
        LockPlayerControls();
    }

    public void StopVideo()
    {
        if (videoPlayer == null) return;

        videoPlayer.Stop(); // 停止播放
        isInteracting = false;

        // 恢复玩家控制
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
