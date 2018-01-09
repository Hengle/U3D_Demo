/* Copyright © 2014 Apex Software. All rights reserved. */
namespace Apex.Editor
{
    using Apex.Steering.Components;
    using UnityEditor;
    using UnityEngine;

    public static partial class VersionUpgrader
    {
        static partial void UpdateSteer()
        {
            if (!EditorUtility.DisplayDialog("Apex Steer", "Do you wish to also upgrade the scene to use Apex Steer?", "Yes", "No"))
            {
                return;
            }

            var units = GetAllNonPrefabInstances<SteerForPathComponent>();
            foreach (var unit in units)
            {
                unit.gameObject.AddComponent<QuickStarts.NavigationWithVectorFields>();
            }

            Debug.Log("Scene upgraded to Apex Steer with Vector Fields for group steering");
        }
    }
}
