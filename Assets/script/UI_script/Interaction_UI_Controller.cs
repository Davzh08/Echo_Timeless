using UnityEngine;

public class ClockInteractionUI : MonoBehaviour
{
    public GameObject interactionUI; // UI GameObject for interaction prompt
    public bool showUIOnce = false; // Control if the UI should only show once
    private bool isPlayerNear = false;
    private bool hasInteracted = false; // Track if interaction has happened

    void Start()
    {
        interactionUI.SetActive(false); // Initially hide the UI
    }

    void Update()
    {
        // Check if the player presses E and is in the trigger area
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E) && !hasInteracted)
        {
            // Set interaction to true if showing UI once
            if (showUIOnce) hasInteracted = true;

            interactionUI.SetActive(false); // Hide the UI after interaction
            Debug.Log("Interaction triggered, performing action...");
            // Place any additional interaction actions here
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && (!hasInteracted || !showUIOnce))
        {
            isPlayerNear = true;
            interactionUI.SetActive(true); // Show the UI if conditions are met
            Debug.Log("Player entered the trigger area, UI shown.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            interactionUI.SetActive(false); // Hide the UI when the player leaves
            Debug.Log("Player left the trigger area, UI hidden.");
        }
    }
}