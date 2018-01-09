/* Copyright © 2014 Apex Software. All rights reserved. */
namespace Apex.Editor
{
    using System;
    using Apex.Steering.Behaviours;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(PatrolPointsComponent), false)]
    public class PatrolPointsComponentEditor : Editor
    {
        private SerializedProperty _points;
        private SerializedProperty _relativeToTransform;
        private SerializedProperty _pointColor;
        private SerializedProperty _textColor;

        private int _id;
        private bool _inPlacementMode;
        private bool _emphasize;
        private GUIStyle _numberStyle;

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(_pointColor);
            EditorGUILayout.PropertyField(_textColor);
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(_points, true);

            var relativeCurrent = _relativeToTransform.boolValue;
            EditorGUILayout.PropertyField(_relativeToTransform);
            this.serializedObject.ApplyModifiedProperties();

            if (_relativeToTransform.boolValue != relativeCurrent)
            {
                var p = this.target as PatrolPointsComponent;
                var points = p.points;
                var t = p.transform;

                if (_relativeToTransform.boolValue)
                {
                    for (int i = 0; i < points.Length; i++)
                    {
                        points[i] = t.InverseTransformPoint(points[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < points.Length; i++)
                    {
                        points[i] = t.TransformPoint(points[i]);
                    }
                }

                EditorUtility.SetDirty(p);
            }

            EditorGUILayout.Separator();
            if (_inPlacementMode)
            {
                var msg = "Use the number keys to place points 0-9 at the mouse cursor.";
                EditorGUILayout.HelpBox(msg, MessageType.Info);
                EditorGUILayout.Separator();

                if (GUILayout.Button("Done (Esc)"))
                {
                    _inPlacementMode = false;
                }
            }
            else if (GUILayout.Button("Edit Points"))
            {
                _inPlacementMode = true;

                if (SceneView.sceneViews.Count > 0)
                {
                    ((SceneView)SceneView.sceneViews[0]).Focus();
                }
            }

            var newEmphasize = GUILayout.Toggle(_emphasize, "Emphasize Points", GUI.skin.button);
            if (newEmphasize != _emphasize)
            {
                _emphasize = newEmphasize;
                if (_emphasize)
                {
                    EditorApplication.update += OnUpdate;
                }
                else
                {
                    EditorApplication.update -= OnUpdate;
                    OnUpdate();
                }
            }
        }

        private static void DrawActiveIndication(PatrolPointsComponent p)
        {
            Handles.color = Color.white;
            var radius = 0.5f + ((Mathf.Cos(Time.realtimeSinceStartup * 10f) + 1f) / 6f);

            var points = p.worldPoints;
            for (int i = 0; i < points.Length; i++)
            {
                Handles.DrawWireDisc(points[i], Vector3.up, radius);
            }
        }

        private void DrawNumberLabels(PatrolPointsComponent p)
        {
            if (_numberStyle == null)
            {
                _numberStyle = new GUIStyle(GUI.skin.label);
                _numberStyle.fontStyle = FontStyle.Bold;
            }

            _numberStyle.normal.textColor = p.textColor;
            var points = p.worldPoints;
            for (int i = 0; i < points.Length; i++)
            {
                var pos = points[i];
                pos.x -= 0.1f;
                pos.y += 1f;
                pos.z += 0.3f;
                Handles.Label(pos, i.ToString(), _numberStyle);
            }
        }

        private void OnEnable()
        {
            _points = this.serializedObject.FindProperty("points");
            _relativeToTransform = this.serializedObject.FindProperty("relativeToTransform");
            _pointColor = this.serializedObject.FindProperty("pointColor");
            _textColor = this.serializedObject.FindProperty("textColor");

            _id = GUIUtility.GetControlID(this.GetHashCode(), FocusType.Passive);
        }

        private void OnUpdate()
        {
            var sv = SceneView.lastActiveSceneView;
            if (sv != null)
            {
                sv.Repaint();
            }
        }

        private void OnSceneGUI()
        {
            var evt = Event.current;
            var p = this.target as PatrolPointsComponent;

            if (evt.type == EventType.Repaint)
            {
                if (_emphasize && GUIUtility.hotControl != _id)
                {
                    DrawActiveIndication(p);
                }

                DrawNumberLabels(p);
                return;
            }

            if (!_inPlacementMode)
            {
                return;
            }

            var groundRect = new Plane(Vector3.up, new Vector3(0f, 0f, 0f));

            if (evt.type == EventType.KeyDown && evt.keyCode >= KeyCode.Alpha0 && evt.keyCode <= KeyCode.Alpha9)
            {
                GUIUtility.hotControl = _id;

                evt.Use();
            }
            else if (evt.type == EventType.KeyUp && evt.keyCode >= KeyCode.Alpha0 && evt.keyCode <= KeyCode.Alpha9)
            {
                evt.Use();

                Vector3 point;
                if (!EditorUtilities.MouseToWorldPoint(groundRect, out point))
                {
                    GUIUtility.hotControl = 0;
                    return;
                }

                if (p.relativeToTransform)
                {
                    point = p.transform.InverseTransformPoint(point);
                }

                int idx = ((int)evt.keyCode) - 48;
                var points = p.points;
                if (points.Length < idx + 1)
                {
                    var tmp = new Vector3[idx + 1];
                    Array.Copy(points, tmp, points.Length);
                    points = tmp;
                    p.points = tmp;
                }

                points[idx] = point;
                EditorUtility.SetDirty(p);
                GUIUtility.hotControl = 0;
            }
            else if (evt.type == EventType.KeyDown && evt.keyCode == KeyCode.Escape)
            {
                GUIUtility.hotControl = _id;
                evt.Use();
            }
            else if (evt.type == EventType.KeyUp && evt.keyCode == KeyCode.Escape)
            {
                GUIUtility.hotControl = 0;
                evt.Use();
                _inPlacementMode = false;
                this.Repaint();
            }
        }
    }
}
