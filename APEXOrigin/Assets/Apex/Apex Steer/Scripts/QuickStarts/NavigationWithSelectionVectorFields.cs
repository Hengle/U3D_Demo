/* Copyright © 2014 Apex Software. All rights reserved. */

namespace Apex.QuickStarts
{
    using Apex.Steering;
    using Apex.Steering.Components;
    using Apex.Steering.VectorFields;
    using Apex.Units;
    using UnityEngine;

    /// <summary>
    /// Extended version of <see cref="NavigationWithSelectionQuickStart"/> that makes the unit support grouping with vector fields.
    /// </summary>
    [AddComponentMenu("Apex/QuickStarts/Navigating Unit with Selection and Vector Fields")]
    public class NavigationWithSelectionVectorFields : NavigationWithSelectionQuickStart
    {
        /// <summary>
        /// Extends this quick start with additional components.
        /// </summary>
        /// <param name="gameWorld">The game world.</param>
        protected override void Extend(GameObject gameWorld)
        {
            base.Extend(gameWorld);

            AddIfMissing<VectorFieldManagerComponent>(gameWorld, true);

            if (gameWorld.As<IUnitGroupingStrategyFactory>() == null)
            {
                gameWorld.AddComponent<SteeringUnitGroupingStrategyFactory>();
            }

            var go = this.gameObject;

            AddIfMissing<SteerForVectorFieldComponent>(go, 5);
            AddIfMissing<SteerForFormationComponent>(go, 10);            
            AddIfMissing<SteerForBlockedCellRepulsionComponent>(go, 20);
            AddIfMissing<SteerForContainmentComponent>(go, 20);
            AddIfMissing<SteeringController>(go, false);
        }
    }
}