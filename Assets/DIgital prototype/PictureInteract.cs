using UnityEngine;

public class PictureInteract : MonoBehaviour
{
    public Transform pictureViewPoint; // Position to zoom into near the picture

    private CameraFollow cameraFollow; // Camera follow script reference
    private Transform originalTarget;  // Original target for the camera
    private bool isInteracting = false;

    public void StartPictureInteraction(CameraFollow follow)
    {
        cameraFollow = follow;

        if (cameraFollow == null)
        {
            Debug.LogError("CameraFollow script not found!");
            return;
        }

        // Save original camera target
        originalTarget = cameraFollow.target;

        // Change camera target to the picture
        cameraFollow.SetTarget(pictureViewPoint);

        isInteracting = true;

        Debug.Log($"Starting interaction with {gameObject.name}");
        Debug.Log($"Picture view point: {pictureViewPoint.name}");
    }

    void Update()
    {
        // Check if Escape is pressed to exit interaction
        if (isInteracting && (Input.GetKeyDown(KeyCode.Escape)))
        {
            ExitPictureInteraction();
        }
    }

    public void ExitPictureInteraction()
    {
        isInteracting = false;

        // Return camera to original target
        if (cameraFollow != null && originalTarget != null)
        {
            cameraFollow.SetTarget(originalTarget);
        }

        Debug.Log($"Exiting interaction with {gameObject.name}");
    }
}
