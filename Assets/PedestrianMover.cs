using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianMover : MonoBehaviour
{
    public Grid grid;  // Reference to your Grid
    public float speed = 5f;  // Speed of movement
    private List<Node> path;  // The path found by the A* algorithm
    private int currentPathIndex;  // Track the current node in the path

    void Start()
    {
        // Initialize the path (retrieved from your grid after A* is executed)
        path = grid.path;

        // Set the starting index to 0 (the first node in the path)
        currentPathIndex = 0;

        // Start moving if a path exists
        if (path != null && path.Count > 0)
        {
            StartCoroutine(MoveAlongPath());
        }
    }

    // Coroutine to move the object smoothly along the path
    IEnumerator MoveAlongPath()
    {
        // Loop through each node in the path
        while (currentPathIndex < path.Count)
        {
            // Get the world position of the current node in the path
            Vector3 targetPosition = path[currentPathIndex].worldPosition;

            // Move the object towards the target position
            while (transform.position != targetPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;  // Wait for the next frame
            }

            // Move to the next node in the path
            currentPathIndex++;
        }

        // Path completed
        Debug.Log("Reached the destination!");
    }
}