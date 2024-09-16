using UnityEngine;

public static class Spline
{
    // Catmull-Rom interpolation function
    public static Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        // Calculate basis functions
        float t2 = t * t;
        float t3 = t2 * t;

        Vector3 result =
            0.5f * ((2.0f * p1) +
                    (-p0 + p2) * t +
                    (2.0f * p0 - 5.0f * p1 + 4.0f * p2 - p3) * t2 +
                    (-p0 + 3.0f * p1 - 3.0f * p2 + p3) * t3);

        return result;
    }
}