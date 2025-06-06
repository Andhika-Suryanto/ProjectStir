using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(CarDirectionContainer))]
public class EditorCarDirection : Editor
{

    CarDirectionContainer wpScript;

    public override void OnInspectorGUI()
    {

        wpScript = (CarDirectionContainer)target;
        serializedObject.Update();

        EditorGUILayout.HelpBox("Create Waypoints By Shift + Left Mouse Button On Your Road", MessageType.Info);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("waypoints"), new GUIContent("Waypoints", "Waypoints"), true);

        foreach (Transform item in wpScript.transform)
        {

            if (item.gameObject.GetComponent<Transform>() == null)
                item.gameObject.AddComponent<Transform>();

        }

        if (GUILayout.Button("Delete Waypoints"))
        {

            foreach (Transform t in wpScript.waypoints)
                DestroyImmediate(t.gameObject);

            wpScript.waypoints.Clear();
            EditorUtility.SetDirty(wpScript);

        }

        if (GUI.changed)
            EditorUtility.SetDirty(wpScript);

        serializedObject.ApplyModifiedProperties();

    }

    private void OnSceneGUI()
    {

        Event e = Event.current;
        wpScript = (CarDirectionContainer)target;

        if (e != null)
        {

            if (e.isMouse && e.shift && e.type == EventType.MouseDown)
            {

                Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(ray, out hit, 5000.0f))
                {

                    Vector3 newTilePosition = hit.point;

                    GameObject wp = new GameObject("Waypoint " + wpScript.waypoints.Count.ToString());
                    wp.transform.position = newTilePosition;
                    wp.transform.SetParent(wpScript.transform);

                    GetWaypoints();

                }

            }

            if (wpScript)
                Selection.activeGameObject = wpScript.gameObject;

        }

        GetWaypoints();

    }

    public void GetWaypoints()
    {

        wpScript.waypoints = new List<Transform>();

        Transform[] allTransforms = wpScript.transform.GetComponentsInChildren<Transform>();

        foreach (Transform t in allTransforms)
        {

            if (t != wpScript.transform)
                wpScript.waypoints.Add(t);

        }

    }

}
