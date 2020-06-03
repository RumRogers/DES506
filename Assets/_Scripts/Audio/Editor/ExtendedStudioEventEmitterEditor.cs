using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEditor;

[CustomEditor(typeof(GameAudio.ExtendedStudioEventEmitter))]
[CanEditMultipleObjects]
public class ExtendedStudioEventEmitterEditor : FMODUnity.StudioEventEmitterEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var list = serializedObject.FindProperty("m_gameEvents");
        EditorGUILayout.PropertyField(list, new GUIContent("Triggered by game events"), true);
        
        serializedObject.ApplyModifiedProperties();
    }
}
