using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SlashSlash))]
public class SlashSlashEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SlashSlash myTarget = (SlashSlash)target;

        bool deleteOnPlay = EditorGUILayout.Toggle("Free memory on play", myTarget.deleteStringOnPlay);
        myTarget.deleteStringOnPlay = deleteOnPlay;
        if (deleteOnPlay)
        {
            myTarget.deleteOnlyInReleaseMode = EditorGUILayout.Toggle("Free memory only in release mode", myTarget.deleteOnlyInReleaseMode);
        }
        //EditorGUILayout.LabelField("Level", myTarget.Level.ToString());
    }
}
