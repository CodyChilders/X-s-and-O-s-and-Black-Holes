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

        bool deleteOnPlay = EditorGUILayout.Toggle("Delete on start", myTarget.deleteStringOnPlay);
        myTarget.deleteStringOnPlay = deleteOnPlay;
        if (deleteOnPlay)
        {
            myTarget.deleteOnlyInReleaseMode = EditorGUILayout.Toggle("Delete only in release", myTarget.deleteOnlyInReleaseMode);
        }
    }
}
