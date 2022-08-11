using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace CodeWriter.ViewBinding.Editor
{
    public static class ViewContextGUI
    {
        private static readonly GUIContent ContextContent = new GUIContent("Context");

        public static void DrawContextField(
            SerializedObject serializedObject,
            [CanBeNull] SerializedProperty primaryContextProp = null,
            [CanBeNull] SerializedProperty extraContextsProp = null)
        {
            GUILayout.BeginHorizontal();

            EditorGUILayout.PrefixLabel(ContextContent);

            GUILayout.BeginVertical();
            EditorGUI.BeginDisabledGroup(true);

            if (primaryContextProp != null)
            {
                EditorGUILayout.PropertyField(primaryContextProp);
            }

            if (extraContextsProp != null)
            {
                for (var i = 0; i < extraContextsProp.arraySize; i++)
                {
                    EditorGUILayout.PropertyField(extraContextsProp.GetArrayElementAtIndex(i), GUIContent.none);
                }

                if (extraContextsProp.arraySize == 0)
                {
                    GUILayout.Label("None", EditorStyles.objectField);
                }
            }

            EditorGUI.EndDisabledGroup();
            GUILayout.EndVertical();

            if (GUILayout.Button("Fill Context", GUILayout.Width(100)))
            {
                FillContext(serializedObject, primaryContextProp, extraContextsProp);
            }

            GUILayout.EndHorizontal();
        }

        private static void FillContext(
            SerializedObject serializedObject,
            [CanBeNull] SerializedProperty primaryContextProp,
            [CanBeNull] SerializedProperty extraContextsProp)
        {
            if (serializedObject.targetObject is MonoBehaviour mb)
            {
                ViewContext primaryContext = null;

                if (primaryContextProp != null)
                {
                    primaryContext = mb.GetComponentInParent<ViewContext>();
                    primaryContextProp.objectReferenceValue = primaryContext;
                }

                if (extraContextsProp != null)
                {
                    var allValues = mb.GetComponentsInParent<ViewContextBase>();

                    var extraContexts = new List<ViewContextBase>();
                    extraContexts.AddRange(allValues);
                    extraContexts.RemoveAll(it => it == null || it == mb || it == primaryContext);

                    if (mb is ViewContextBase targetViewContext)
                    {
                        var selfValues = mb.GetComponents<ViewContextBase>();

                        extraContexts.RemoveAll(it =>
                        {
                            var selfIndex = Array.IndexOf(selfValues, targetViewContext);
                            var otherIndex = Array.IndexOf(selfValues, it);
                            return selfIndex < otherIndex;
                        });
                    }

                    extraContextsProp.arraySize = extraContexts.Count;
                    for (var i = 0; i < extraContexts.Count; i++)
                    {
                        extraContextsProp.GetArrayElementAtIndex(i).objectReferenceValue = extraContexts[i];
                    }
                }

                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}