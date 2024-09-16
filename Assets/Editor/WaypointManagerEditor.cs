using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WaypointManager))]
public class WaypointManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        WaypointManager _target = (WaypointManager)target;

        if (GUILayout.Button("Make New Point"))
        {
            _target.MakeNewPoint();
        }
        
    }
}
