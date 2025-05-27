using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(DialogBoxTriggerContainer))]
public class EditorDialogBox : Editor
{
    DialogBoxTriggerContainer wpScript;

    public override void OnInspectorGUI()
    {

        wpScript = (DialogBoxTriggerContainer)target;
        serializedObject.Update();

        EditorGUILayout.HelpBox("Create Waypoints By Shift + Left Mouse Button On Your Road", MessageType.Info);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("waypoints"), new GUIContent("Waypoints", "Waypoints"), true);

        foreach (Transform item in wpScript.transform)
        {

            if (item.gameObject.GetComponent<DialogBox>() == null)
                item.gameObject.AddComponent<DialogBox>();

        }

        if (GUILayout.Button("Delete Waypoints"))
        {

            foreach (DialogBox t in wpScript.waypoints)
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
        wpScript = (DialogBoxTriggerContainer)target;

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
                    wp.AddComponent<DialogBox>();
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

        wpScript.waypoints = new List<DialogBox>();

        DialogBox[] allTransforms = wpScript.transform.GetComponentsInChildren<DialogBox>();

        foreach (DialogBox t in allTransforms)
        {

            if (t != wpScript.transform)
                wpScript.waypoints.Add(t);

        }

    }

}

