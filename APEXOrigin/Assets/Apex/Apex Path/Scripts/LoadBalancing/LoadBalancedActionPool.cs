/* Copyright © 2014 Apex Software. All rights reserved. */
namespace Apex.LoadBalancing
{
    using System;
    using System.Collections.Generic;
    using Apex.Utilities;

    public static class LoadBalancedActionPool
    {
        private static Queue<RecycledOneTimeAction> _oneTimeActions;

        /// <summary>
        /// Executes the specified action once.
        /// </summary>
        /// <param name="lb">The load balancer.</param>
        /// <param name="action">The action.</param>
        /// <param name="delay">The delay until the action is executed.</param>
        public static void ExecuteOnce(this ILoadBalancer lb, Action action, float delay)
        {
            Ensure.ArgumentNotNull(action, "action");

            if (_oneTimeActions == null)
            {
                _oneTimeActions = new Queue<RecycledOneTimeAction>(1);
            }

            RecycledOneTimeAction ota;
            if (_oneTimeActions.Count > 0)
            {
                ota = _oneTimeActions.Dequeue();
                ota.action = action;
            }
            else
            {
                ota = new RecycledOneTimeAction(action);
            }

            lb.Add(ota, delay, true);
        }

        private static void Return(RecycledOneTimeAction action)
        {
            action.action = null;
            _oneTimeActions.Enqueue(action);
        }

        private class RecycledOneTimeAction : ILoadBalanced
        {
            private Action _action;

            internal RecycledOneTimeAction(Action action)
            {
                _action = action;
            }

            internal Action action
            {
                get { return _action; }
                set { _action = value; }
            }

            bool ILoadBalanced.repeat
            {
                get { return false; }
            }

            float? ILoadBalanced.ExecuteUpdate(float deltaTime, float nextInterval)
            {
                _action();
                LoadBalancedActionPool.Return(this);
                return null;
            }
        }
    }
}
