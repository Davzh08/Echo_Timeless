using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroVideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;  // Drag your VideoPlayer component here
    public string nextSceneName;     // Set the name of the scene to load after the video finishes

    void Start()
    {
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer not assigned!");
            return;
        }

        // Start playing the video at the beginning
        videoPlayer.Play();

        // Register callback for when the video finishes
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        // Unregister the callback
        videoPlayer.loopPointReached -= OnVideoFinished;

        // Load the next scene
        SceneManager.LoadScene(nextSceneName);
    }
}