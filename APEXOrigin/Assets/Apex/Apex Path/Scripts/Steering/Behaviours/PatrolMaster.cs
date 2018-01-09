/* Copyright © 2014 Apex Software. All rights reserved. */
namespace Apex.Steering.Behaviours
{
    using System.Collections.Generic;
    using Apex.LoadBalancing;
    using Apex.Messages;
    using Apex.Services;
    using Apex.Units;
    using UnityEngine;

    /// <summary>
    /// MAnager class that keeps track of patrolling units.
    /// </summary>
    public static class PatrolMaster
    {
        private static readonly Dictionary<GameObject, PatrolClient> _clients = new Dictionary<GameObject, PatrolClient>();
        private static readonly Dictionary<GameObject, PatrolClient> _pausedClients = new Dictionary<GameObject, PatrolClient>();
        private static Handler _handler;

        public static void Patrol(this IUnitFacade unit, Vector3[] patrolPoints, bool randomize = false, bool reverse = false, float lingerForSeconds = 0f)
        {
            if (_handler == null)
            {
                _handler = new Handler();
            }

            var client = new PatrolClient
            {
                route = patrolPoints,
                randomize = randomize,
                reverseRoute = reverse,
                lingerForSeconds = lingerForSeconds,
                unit = unit
            };

            _clients[unit.gameObject] = client;

            client.Start();
        }

        /// <summary>
        /// Stops the unit's patrol.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public static void StopPatrol(this IUnitFacade unit)
        {
            PatrolClient client;
            if (!_clients.TryGetValue(unit.gameObject, out client))
            {
                if (!_pausedClients.TryGetValue(unit.gameObject, out client))
                {
                    return;
                }
            }

            _clients.Remove(unit.gameObject);
            _pausedClients.Remove(unit.gameObject);

            if (_clients.Count == 0)
            {
                _handler.Stop();
                _handler = null;
            }

            client.Stop();
        }

        /// <summary>
        /// Pauses the patrol.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public static void PausePatrol(this IUnitFacade unit)
        {
            PatrolClient client;
            if (!_clients.TryGetValue(unit.gameObject, out client))
            {
                return;
            }

            _clients.Remove(unit.gameObject);
            _pausedClients[unit.gameObject] = client;

            client.Stop();
        }

        /// <summary>
        /// Resumes the patrol.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns><c>true</c> if the patrol could be resumed; otherwise <c>false</c></returns>
        public static bool ResumePatrol(this IUnitFacade unit)
        {
            PatrolClient client;
            if (!_pausedClients.TryGetValue(unit.gameObject, out client))
            {
                return false;
            }

            _pausedClients.Remove(unit.gameObject);
            _clients[unit.gameObject] = client;

            client.Resume();
            return true;
        }

        /// <summary>
        /// Determines whether the unit is currently patrolling. Units who are paused will still return true here.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns><c>true</c> if the unit is associated with a patrol; otherwise <c>false</c></returns>
        public static bool IsOnPatrol(this IUnitFacade unit)
        {
            return _clients.ContainsKey(unit.gameObject) || _pausedClients.ContainsKey(unit.gameObject);
        }

        private static void Continue(GameObject go)
        {
            PatrolClient client;
            if (!_clients.TryGetValue(go, out client))
            {
                return;
            }

            if (client.lingerForSeconds > 0f)
            {
                LoadBalancer.defaultBalancer.ExecuteOnce(client.Continue, client.lingerForSeconds);
            }
            else
            {
                client.Continue();
            }
        }

        private class PatrolClient
        {
            private int _currentIndex;
            private int _nextIndex;
            private bool _stopped;

            internal Vector3[] route;
            internal IUnitFacade unit;
            internal bool randomize;
            internal bool reverseRoute;
            internal float lingerForSeconds;

            internal void Start()
            {
                _nextIndex = this.reverseRoute ? this.route.Length : -1;
                IncrementNext();

                _currentIndex = _nextIndex;
                IncrementNext();

                Resume();
            }

            internal void Stop()
            {
                this.unit.Stop();
                _stopped = true;
            }

            internal void Resume()
            {
                _stopped = false;

                this.unit.MoveTo(route[_currentIndex], false);

                if (this.lingerForSeconds == 0f)
                {
                    this.unit.MoveTo(route[_nextIndex], true);
                }
            }

            internal void Continue()
            {
                if (_stopped)
                {
                    return;
                }

                _currentIndex = _nextIndex;
                IncrementNext();

                if (this.lingerForSeconds == 0f)
                {
                    this.unit.MoveTo(route[_nextIndex], true);
                }
                else
                {
                    this.unit.MoveTo(route[_currentIndex], false);
                }
            }

            private void IncrementNext()
            {
                if (this.randomize)
                {
                    var tmp = _nextIndex;
                    while (tmp == _nextIndex)
                    {
                        _nextIndex = Random.Range(0, route.Length);
                    }
                }
                else if (this.reverseRoute)
                {
                    _nextIndex = (--_nextIndex + route.Length) % route.Length;
                }
                else
                {
                    _nextIndex = ++_nextIndex % route.Length;
                }
            }
        }

        private class Handler : IHandleMessage<UnitNavigationEventMessage>
        {
            public Handler()
            {
                GameServices.messageBus.Subscribe(this);
            }

            public void Stop()
            {
                GameServices.messageBus.Unsubscribe(this);
            }

            void IHandleMessage<UnitNavigationEventMessage>.Handle(UnitNavigationEventMessage message)
            {
                if (message.isHandled)
                {
                    return;
                }

                switch (message.eventCode)
                {
                    case UnitNavigationEventMessage.Event.WaypointReached:
                    case UnitNavigationEventMessage.Event.DestinationReached:
                    {
                        message.isHandled = true;

                        PatrolMaster.Continue(message.entity);
                        break;
                    }

                    case UnitNavigationEventMessage.Event.Stuck:
                    {
                        message.isHandled = true;

                        //PatrolMaster.Start(message.entity);
                        break;
                    }
                }
            }
        }
    }
}
