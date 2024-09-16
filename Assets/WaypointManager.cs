using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public List<Transform> Waypoints = new List<Transform>();
    public int resolution = 20;
    public void MakeNewPoint()
    {
        Vector3 SpawnPosition = Vector3.zero;
        if (Waypoints.Count <= 0)
        {
            SpawnPosition = GetCenterPosition();
        }
        else
        {
            SpawnPosition = Waypoints[^1].position + new Vector3(1, 0, 1);
        }

        GameObject _go = new GameObject("Unassigned_SpawnPoint");
        _go.transform.position = SpawnPosition;
        Selection.activeGameObject = _go;
        Waypoints.Add(_go.transform);
        _go.transform.SetParent(transform);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < Waypoints.Count; i++)
        {
            Gizmos.DrawSphere(Waypoints[i].position,.3f);
        }
        // if (Waypoints.Count < 2)
        //     return;  // Need at least 2 points to draw a line
        //
        // // Loop through the waypoints, drawing a spline between each set of 4 points
        // for (int i = 0; i < Waypoints.Count - 1; i++)
        // {
        //     // Get the four control points for the Catmull-Rom spline
        //     Vector3 p0 = Waypoints[Mathf.Max(i - 1, 0)].position; // Clamp to first point
        //     Vector3 p1 = Waypoints[i].position;
        //     Vector3 p2 = Waypoints[Mathf.Min(i + 1, Waypoints.Count - 1)].position; // Clamp to last point
        //     Vector3 p3 = Waypoints[Mathf.Min(i + 2, Waypoints.Count - 1)].position; // Clamp to last point
        //
        //     Vector3 previousPosition = p1;
        //
        //     // Draw the spline by interpolating between the points
        //     for (int j = 1; j <= resolution; j++)
        //     {
        //         float t = j / (float)resolution;
        //         Vector3 newPosition = CatmullRom(p0, p1, p2, p3, t);
        //
        //         Gizmos.DrawLine(previousPosition, newPosition);
        //         previousPosition = newPosition;
        //     }
        // }
    }

    // Catmull-Rom interpolation function
    // private Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    // {
    //     // Calculate basis functions
    //     float t2 = t * t;
    //     float t3 = t2 * t;
    //
    //     Vector3 result = 0.5f * (
    //         (2.0f * p1) +
    //         (-p0 + p2) * t +
    //         (2.0f * p0 - 5.0f * p1 + 4.0f * p2 - p3) * t2 +
    //         (-p0 + 3.0f * p1 - 3.0f * p2 + p3) * t3);
    //
    //     return result;
    // }
    
    private Vector3 GetCenterPosition()
    {
        Vector3 SpawnPos= Vector3.zero;
        // Check if sceneView is the current drawing one
        if (SceneView.currentDrawingSceneView == null){
            Debug.Log ("Click on scene view before adding a spawnPoint!");
            return SpawnPos;
        }
	
        //Cast a ray from the middle of the screen
        Camera sceneCam = SceneView.currentDrawingSceneView.camera;
        Vector3 rayPos = sceneCam.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, 1000f));
        Ray ray = new Ray (sceneCam.transform.position, rayPos);
        RaycastHit hit;
        if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
                if (hit.collider.tag == "Floor")
                    SpawnPos = new Vector3 (hit.point.x, hit.collider.bounds.center.y, hit.point.z);
        }
        return SpawnPos;
    }
    
}
