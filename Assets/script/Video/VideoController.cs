using UnityEngine;
using UnityEngine.Video;

public class VideoTrigger : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Reference to the Video Player component
    private bool isPlayerInside = false; // Tracks if the player is inside the trigger zone

    void Start()
    {
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer is not assigned!");
            return;
        }

        videoPlayer.gameObject.SetActive(false); // Ensure video player is hidden initially
        videoPlayer.loopPointReached += OnVideoEnd; // Register event when the video finishes
    }

    void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Playing video...");
            StartVideo();
        }

        if (videoPlayer.isPlaying && Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Stopping video...");
            StopVideo();
        }
    }

    public void StartVideo()
    {
        if (videoPlayer == null) return;

        videoPlayer.gameObject.SetActive(true); // Activate the video player
        videoPlayer.Play(); // Play the video
    }

    public void StopVideo()
    {
        if (videoPlayer == null) return;

        videoPlayer.Stop();
        videoPlayer.gameObject.SetActive(false); // Hide the video player
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        StopVideo();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }
}