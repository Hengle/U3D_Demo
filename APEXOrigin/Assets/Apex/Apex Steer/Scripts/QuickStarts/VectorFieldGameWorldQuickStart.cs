/* Copyright © 2014 Apex Software. All rights reserved. */

namespace Apex.QuickStarts
{
    using Apex.Steering;
    using Apex.Steering.VectorFields;
    using Apex.Units;
    using UnityEngine;

    /// <summary>
    /// Sets up the necessary components on the game world to support navigation with vector fields.
    /// </summary>
    [AddComponentMenu("Apex/QuickStarts/Game World with Vector Fields")]
    public class VectorFieldGameWorldQuickStart : GameWorldQuickStart
    {
        /// <summary>
        /// Extends this quick start with additional components.
        /// </summary>
        /// <param name="gameWorld">The game world.</param>
        protected override void Extend(GameObject gameWorld)
        {
            AddIfMissing<VectorFieldManagerComponent>(gameWorld, true);

            if (gameWorld.As<IUnitGroupingStrategyFactory>() == null)
            {
                gameWorld.AddComponent<SteeringUnitGroupingStrategyFactory>();
            }
        }
    }
}