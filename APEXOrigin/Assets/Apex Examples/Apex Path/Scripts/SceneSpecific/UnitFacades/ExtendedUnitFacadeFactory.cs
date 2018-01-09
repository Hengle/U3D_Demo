#pragma warning disable 1591
namespace Apex.Examples.SceneSpecific.UnitFacades
{
    using Apex.Units;
    using UnityEngine;

    [AddComponentMenu("Apex/Examples/SceneSpecific/UnitFacades/Extended Unit Facade Factory")]
    public class ExtendedUnitFacadeFactory : MonoBehaviour, IUnitFacadeFactory
    {
        public IUnitFacade CreateUnitFacade()
        {
            return new ExtendedUnitFacade();
        }
    }
}
