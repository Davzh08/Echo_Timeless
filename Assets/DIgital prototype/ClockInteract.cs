using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class ClockInteract : MonoBehaviour
{
    public Transform clockViewPoint;       // Position to zoom into near the clock
    public GameObject timeInputUI;         // UI panel for time input
    public Text inputText;                 // Text for displaying time input
    public Text feedbackText;              // Feedback text for correct/incorrect input
    public string correctTime = "2008";    // Correct time input without colon
    

    private Camera playerCamera;           // Player's main camera
    private Transform playerCameraOriginalParent;
    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;
    private bool isInteracting = false;
    private string playerInput = "";

    private bool isPlayerInRange = false;

    public void StartClockInteraction(Camera camera)
    {
        playerCamera = camera;
        playerCameraOriginalParent = camera.transform.parent;

        if (playerCamera == null)
        {
            Debug.LogError("Player camera not found! Ensure a Camera component is attached to the player.");
            return;
        }

        // Save original position and rotation
        originalCameraPosition = playerCamera.transform.position;
        originalCameraRotation = playerCamera.transform.rotation;

        isInteracting = true;
        StartCoroutine(ZoomInToClock());
    }

    private IEnumerator ZoomInToClock()
    {
        Vector3 initialPosition = playerCamera.transform.position;
        Quaternion initialRotation = playerCamera.transform.rotation;

        float zoomSpeed = 2f;
        float progress = 0f;

        while (progress < 1f)
        {
            playerCamera.transform.position = Vector3.Lerp(initialPosition, clockViewPoint.position, progress);
            playerCamera.transform.rotation = Quaternion.Lerp(initialRotation, clockViewPoint.rotation, progress);
            progress += Time.deltaTime * zoomSpeed;
            yield return null;
        }

        playerCamera.transform.SetParent(clockViewPoint); // Lock camera to clock
        timeInputUI.SetActive(true);
        feedbackText.text = ""; // Clear previous feedback
    }

    void Update()
    {
        if (isInteracting)
        {
            HandleTimeInput();
            if (Input.GetKeyDown(KeyCode.Return))
            {
                CheckTimeInput();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ExitClockInteraction();
            }

        }
    }




    private void HandleTimeInput()
    {
        foreach (char c in Input.inputString)
        {
            if (char.IsDigit(c) && playerInput.Length < 4)
            {
                playerInput += c;
            }
            else if (c == '\b' && playerInput.Length > 0)
            {
                playerInput = playerInput.Substring(0, playerInput.Length - 1); // Delete last character
            }
        }
        inputText.text = FormatTimeInput(playerInput);
    }

    private string FormatTimeInput(string input)
    {
        if (input.Length >= 2)
        {
            return input.Insert(2, ":");
        }
        return input;
    }

    private void CheckTimeInput()
    {
        //if (playerInput == correctTime)
        if (playerInput == correctTime)
        {
            feedbackText.text = "Correct!";
            inputText.color = Color.green;
        }
        else
        {
            feedbackText.text = "Incorrect! Try again.";
            feedbackText.color = Color.red;
        }
    }

    public void ExitClockInteraction()
    {
        timeInputUI.SetActive(false);
        isInteracting = false;
        playerInput = "";
        feedbackText.text = "";

        // Return camera to original position and re-parent it to the player
        playerCamera.transform.SetParent(playerCameraOriginalParent);
        playerCamera.transform.position = originalCameraPosition;
        playerCamera.transform.rotation = originalCameraRotation;
    }

   
}
