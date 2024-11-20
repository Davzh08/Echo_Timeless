using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClockInteract : MonoBehaviour
{
    public Transform clockViewPoint; // Position to zoom into near the clock
    public GameObject timeInputUI;   // UI panel for time input
    public Text inputText;           // Text for displaying time input
    public Text feedbackText;        // Feedback text for correct/incorrect input
    public string correctTime = "2008"; // Correct time input without colon

    private CameraFollow cameraFollow; // Camera follow script reference
    private Transform originalTarget;  // Original target for the camera
    private bool isInteracting = false;
    private string playerInput = "";

    public void StartClockInteraction(CameraFollow follow)
    {
        cameraFollow = follow;

        if (cameraFollow == null)
        {
            Debug.LogError("CameraFollow script not found!");
            return;
        }

        // Save original camera target
        originalTarget = cameraFollow.target;

        // Change camera target to the clock
        cameraFollow.SetTarget(clockViewPoint);

        isInteracting = true;
        timeInputUI.SetActive(true);
        feedbackText.text = ""; // Clear previous feedback

        Debug.Log($"Starting interaction with {gameObject.name}");
        Debug.Log($"Clock view point: {clockViewPoint.name}");
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

            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape))
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
        return input.Length >= 2 ? input.Insert(2, ":") : input;
    }

    private void CheckTimeInput()
    {
        if (playerInput == correctTime)
        {
            feedbackText.text = "Correct!";
            feedbackText.color = Color.green;
        }
        else
        {
            feedbackText.text = "Incorrect! Try again.";
            feedbackText.color = Color.red;
        }
    }

    public void ExitClockInteraction()
    {
        isInteracting = false;
        playerInput = "";
        timeInputUI.SetActive(false);
        feedbackText.text = "";

        // Return camera to original target
        if (cameraFollow != null && originalTarget != null)
        {
            cameraFollow.SetTarget(originalTarget);
        }
    }
}
