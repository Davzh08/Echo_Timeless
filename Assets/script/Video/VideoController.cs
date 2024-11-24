using UnityEngine;
using UnityEngine.Video;

public class VideoTrigger : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Reference to the Video Player component
    public GameObject promptUI; // UI element to prompt the player (e.g., "Press E to play video")

    private bool isPlayerInside = false; // Tracks if the player is inside the trigger zone

    void Start()
    {
        // Hide the prompt UI at the start
        if (promptUI != null)
        {
            promptUI.SetActive(false);
        }

        // Hide the video player and register a callback for when the video ends
        if (videoPlayer != null)
        {
            videoPlayer.gameObject.SetActive(false); // Ensure video player is hidden initially
            videoPlayer.loopPointReached += OnVideoEnd; // Register event when the video finishes
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true; // Mark that the player is inside the trigger zone
            if (promptUI != null)
            {
                promptUI.SetActive(true); // Show the prompt UI
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the object exiting the trigger is the player
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false; // Mark that the player has left the trigger zone
            if (promptUI != null)
            {
                promptUI.SetActive(false); // Hide the prompt UI
            }
        }
    }

    void Update()
    {
        // Check if the player is in the trigger zone and presses the E key
        if (isPlayerInside && Input.GetKeyDown(KeyCode.E))
        {
            StartVideo(); // Start the video playback
        }
    }

    void StartVideo()
    {
        if (videoPlayer != null)
        {
            videoPlayer.gameObject.SetActive(true); // Activate the video player
            videoPlayer.Play(); // Play the video
            if (promptUI != null)
            {
                promptUI.SetActive(false); // Hide the prompt UI while the video plays
            }
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        // Callback function when the video finishes playing
        if (videoPlayer != null)
        {
            videoPlayer.Stop(); // Stop the video
            videoPlayer.gameObject.SetActive(false); // Hide the video player
            if (promptUI != null)
            {
                promptUI.SetActive(true); // Show the prompt UI again
            }
        }
    }
}