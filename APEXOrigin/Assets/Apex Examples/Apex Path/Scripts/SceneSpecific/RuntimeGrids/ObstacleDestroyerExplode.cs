/* Copyright © 2014 Apex Software. All rights reserved. */
#pragma warning disable 1591
namespace Apex.Examples.SceneSpecific.RuntimeGrids
{
    using Apex.LoadBalancing;
    using Apex.WorldGeometry;
    using UnityEngine;

    public class ObstacleDestroyerExplode : MonoBehaviour
    {
        public float fragmentSize = 0.5f;
        public float explosionForce = 1200f;
        public GameObject fragmentMold;

        private void OnCollisionEnter(Collision col)
        {
            var other = col.collider;

            if (!Layers.InLayer(other.gameObject, Layers.blocks))
            {
                return;
            }

            var affectedBounds = other.bounds;

            GridManager.instance.Update(affectedBounds, 10);

            Destroy(other.gameObject);
            Destroy(this.gameObject);

            //Obviously this next bit is a simplified logic that only works if the obstacle is a rectangular shape and axis aligned.
            //It also assumes the fragment mold is a 1x1x1 cube.
            var size = affectedBounds.size;
            var fragmentsX = (int)(size.x / this.fragmentSize);
            var sizeX = size.x / fragmentsX;

            var fragmentsY = (int)(size.y / this.fragmentSize);
            var sizeY = size.y / fragmentsY;

            var fragmentsZ = (int)(size.z / this.fragmentSize);
            var sizeZ = size.z / fragmentsZ;

            var start = new Vector3(
                affectedBounds.center.x - affectedBounds.extents.x + (sizeX / 2f),
                affectedBounds.center.y - affectedBounds.extents.y + (sizeY / 2f),
                affectedBounds.center.z - affectedBounds.extents.z + (sizeZ / 2f));

            var actualSize = new Vector3(sizeX, sizeY, sizeZ);
            var epiCenter = col.contacts[0].point + col.contacts[0].normal;

            for (int x = 0; x < fragmentsX; x++)
            {
                for (int y = 0; y < fragmentsY; y++)
                {
                    for (int z = 0; z < fragmentsZ; z++)
                    {
                        var pos = new Vector3(start.x + (x * sizeX), start.y + (y * sizeY), start.z + (z * sizeZ));
                        var f = Instantiate(this.fragmentMold, pos, Quaternion.identity) as GameObject;

                        f.transform.localScale = actualSize;
                        f.SetActive(true);

                        var rb = f.GetComponent<Rigidbody>();
                        rb.AddExplosionForce(this.explosionForce, epiCenter, 6f);
                    }
                }
            }
        }
    }
}
