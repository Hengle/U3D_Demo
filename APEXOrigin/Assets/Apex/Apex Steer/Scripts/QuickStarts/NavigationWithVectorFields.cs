/* Copyright © 2014 Apex Software. All rights reserved. */

namespace Apex.QuickStarts
{
    using Apex.Steering;
    using Apex.Steering.Components;
    using Apex.Steering.VectorFields;
    using Apex.Units;
    using UnityEngine;

    /// <summary>
    /// Extended version of <see cref="NavigationQuickStart"/> that makes the unit support grouping with vector fields.
    /// </summary>
    [AddComponentMenu("Apex/QuickStarts/Navigating Unit with Vector Fields")]
    public class NavigationWithVectorFields : NavigationQuickStart
    {
        /// <summary>
        /// Extends this quick start with additional components.
        /// </summary>
        /// <param name="gameWorld">The game world.</param>
        protected override void Extend(GameObject gameWorld)
        {
            var go = this.gameObject;

            AddIfMissing<VectorFieldManagerComponent>(gameWorld, true);

            if (gameWorld.As<IUnitGroupingStrategyFactory>() == null)
            {
                gameWorld.AddComponent<SteeringUnitGroupingStrategyFactory>();
            }

            AddIfMissing<SteerForVectorFieldComponent>(go, 5);
            AddIfMissing<SteerForFormationComponent>(go, 10);
            AddIfMissing<SteerForBlockedCellRepulsionComponent>(go, 20);
            AddIfMissing<SteerForContainmentComponent>(go, 20);
            AddIfMissing<SteeringController>(go, false);
        }
    }
}