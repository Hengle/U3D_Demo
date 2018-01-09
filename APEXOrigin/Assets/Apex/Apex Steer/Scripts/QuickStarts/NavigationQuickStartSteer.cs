/* Copyright © 2014 Apex Software. All rights reserved. */

namespace Apex.QuickStarts
{
    using Apex.Steering;
    using Apex.Steering.Components;

    public partial class NavigationQuickStart
    {
        partial void ExtendForSteer()
        {
            var go = this.gameObject;

            NukeSingle<BasicScanner>(go);
            NukeSingle<SteerForBasicAvoidanceComponent>(go);

            AddIfMissing<SteeringScanner>(go, false);
            AddIfMissing<SteerForSeparationComponent>(go, 9);
            AddIfMissing<SteerForUnitAvoidanceComponent>(go, 15);
        }
    }
}
