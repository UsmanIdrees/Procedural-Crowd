using UnityEngine;

public class SplineMover : MonoBehaviour
{
    public WaypointManager waypointSystem;
    public float speed = 5f;

    private int currentSegment = 1;
    private float t = 0f;

    private void Update()
    {
        if (waypointSystem == null || waypointSystem.Waypoints.Count < 4)
            return;

        // Get the waypoints for the current spline segment
        Vector3 p0 = waypointSystem.Waypoints[currentSegment - 1].position;
        Vector3 p1 = waypointSystem.Waypoints[currentSegment].position;
        Vector3 p2 = waypointSystem.Waypoints[currentSegment + 1].position;
        Vector3 p3 = waypointSystem.Waypoints[currentSegment + 2].position;

        // Move along the spline
        t += Time.deltaTime * speed / Vector3.Distance(p1, p2);
        transform.position = Spline.CatmullRom(p0, p1, p2, p3, t);

        // Move to the next segment if t exceeds 1
        if (t >= 1f)
        {
            t = 0f;
            currentSegment++;

            // Reset to loop back to the start
            if (currentSegment >= waypointSystem.Waypoints.Count - 2)
            {
                currentSegment = 1;
            }
        }
    }
}