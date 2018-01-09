namespace Apex.Editor
{
    using Apex.Steering.VectorFields;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(VectorFieldManagerComponent), false), CanEditMultipleObjects]
    public class VectorFieldManagerComponentEditor : Editor
    {
        private SerializedProperty _vectorFieldOptions;

        public override void OnInspectorGUI()
        {
            if (Application.isPlaying)
            {
                EditorGUILayout.HelpBox("These settings cannot be edited in play mode.", MessageType.Info);
                return;
            }

            this.serializedObject.Update();
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(_vectorFieldOptions);
            this.serializedObject.ApplyModifiedProperties();
        }

        private void OnEnable()
        {
            _vectorFieldOptions = this.serializedObject.FindProperty("vectorFieldOptions");
        }

        private void OnDestroy()
        {
            if (this.target == null)
            {
                EditorUtilities.CleanupComponentMaster();
            }
        }
    }
}