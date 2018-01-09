#pragma warning disable 1591
namespace Apex.Examples.Extensibility
{
    using Apex.PathFinding;
    using Apex.PathFinding.MoveCost;
    using UnityEngine;

    [AddComponentMenu("Apex/Examples/Extensibility/Path Smoother Factory Example")]
    public class PathSmootherFactoryExample : MonoBehaviour, IPathSmootherFactory
    {
        /// <summary>
        /// Creates the path smoother.
        /// </summary>
        /// <returns>
        /// The path smoother
        /// </returns>
        public ISmoothPaths CreateSmoother()
        {
            return new PathSmootherExample();
        }
    }
}
