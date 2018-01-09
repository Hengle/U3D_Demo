using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Road : MonoBehaviour {

    [SerializeField]
    Dictionary<string, List<GameObject>> roadSegments = new Dictionary<string, List<GameObject>>();
   
    public GameObject[] hitResults = null;
    public float Radius = 0.0f;
	// Use this for initialization
	void Start () {
        BoxCollider bc = this.gameObject.GetComponent<BoxCollider>();
        Vector3 bcSize = bc.size;
        Radius = Mathf.Sqrt(bcSize.x * bcSize.x + bcSize.y * bcSize.y + bcSize.z * bcSize.z) / 2f;


        Vector3 originPos = this.transform.TransformPoint(bc.center);

        var hits = Physics.SphereCastAll(originPos, Radius, Vector3.up, 0f);
        List<GameObject> golist = new List<GameObject>();
        for(int i = 0; i < hits.Length; ++i )
        {
            golist.Add(hits[i].collider.gameObject);
        }
        hitResults = golist.ToArray();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.collider.gameObject;
        if( other.layer == LayerMask.NameToLayer("Terrain"))
        {
            if(!roadSegments.ContainsKey(other.name))
            {
                roadSegments.Add(other.name, new List<GameObject>());
            }
            roadSegments[other.name].Add(other);
        }
    }
}
