using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;

public class NavAgent_Script : MonoBehaviour
{
    public List<Transform> waypoints; // List of waypoints to follow
    public Transform player; // Reference to the player
    public float stopDistance = 5f; // Distance within which the NPC will wait for the player
    public float fadeSpeed = 1f; // Speed at which the NPC fades out
    public float movementSpeed = 3.5f; // Movement speed for the NPC

    private NavMeshAgent agent;
    private Renderer npcRenderer;
    private int currentWaypointIndex = 0; // Index of the current waypoint
    private bool isFadingOut = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        npcRenderer = GetComponent<Renderer>();

        // Set the NPC's movement speed
        agent.speed = movementSpeed;

        // Start by setting the NPC's destination to the first waypoint
        if (waypoints.Count > 0)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    void Update()
    {
        // Check distance to player and stop if too far away
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > stopDistance)
        {
            // Stop the NPC and make it face the player
            agent.isStopped = true;

            // Rotate to face the player smoothly
            Vector3 directionToPlayer = player.position - transform.position;
            directionToPlayer.y = 0; // Keep rotation on the horizontal plane
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5.0f); // Adjust rotation speed as desired

            return;
        }
        else
        {
            agent.isStopped = false;
        }

        // Move to the next waypoint if the NPC is close to the current one
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            if (currentWaypointIndex < waypoints.Count - 1)
            {
                currentWaypointIndex++;
                agent.SetDestination(waypoints[currentWaypointIndex].position);
            }
            else
            {
                // Start fade out if NPC has reached the last waypoint
                StartCoroutine(FadeOut());
            }
        }
    }

    // Coroutine to fade out the NPC's material transparency
    private IEnumerator FadeOut()
    {
        if (isFadingOut) yield break;
        isFadingOut = true;

        // Assuming the shader has a property for transparency control, such as "_Color" or "_MainColor"
        Material npcMaterial = npcRenderer.material;
        Color initialColor = npcMaterial.color;

        for (float t = 0; t < 1f; t += Time.deltaTime * fadeSpeed)
        {
            Color newColor = initialColor;
            newColor.a = Mathf.Lerp(1f, 0f, t); // Gradually reduce the alpha value to fade out
            npcMaterial.color = newColor;
            yield return null;
        }

        // Set the final transparency to 0 and deactivate the game object
        npcMaterial.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
        gameObject.SetActive(false); // Hide the NPC after fading out
    }
}