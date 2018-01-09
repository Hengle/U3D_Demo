/* Copyright © 2014 Apex Software. All rights reserved. */
namespace Apex.QuickStarts
{
    using Apex.Steering.Behaviours;
    using UnityEngine;

    /// <summary>
    /// A component that adds a new Game Object with a <see cref="PatrolPointsComponent"/> to the currently selected Game Object.
    /// </summary>
    [AddComponentMenu("Apex/QuickStarts/Patrol Points QS")]
    public class PatrolPointsQuickStart : QuickStartBase
    {
        /// <summary>
        /// Sets up component on which the quick start is attached.
        /// </summary>
        protected override void Setup()
        {
            var go = this.gameObject;

            var routeIndex = go.GetComponentsInChildren<PatrolPointsComponent>(true).Length + 1;

            var parent = go.transform;
            go = new GameObject("Patrol Route " + routeIndex);
            go.transform.parent = parent;

            go.AddComponent<PatrolPointsComponent>();
        }
    }
}
