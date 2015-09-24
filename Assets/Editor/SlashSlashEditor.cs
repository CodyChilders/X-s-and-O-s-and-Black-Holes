using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SlashSlash)), CanEditMultipleObjects]
public class SlashSlashEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SlashSlash myTarget = (SlashSlash)target;

        EditorGUILayout.LabelField("Comment");
        EditorStyles.textField.wordWrap = true;
        myTarget.comment = EditorGUILayout.TextField("", myTarget.comment, GUILayout.MaxHeight(100));

        bool deleteOnPlay = EditorGUILayout.Toggle("Free memory on play", myTarget.deleteStringOnPlay);
        myTarget.deleteStringOnPlay = deleteOnPlay;
        if (deleteOnPlay)
        {
            myTarget.deleteOnlyInReleaseMode = EditorGUILayout.Toggle("Free memory only in release mode", myTarget.deleteOnlyInReleaseMode);
        }
        //EditorGUILayout.LabelField("Level", myTarget.Level.ToString());
    }
}
