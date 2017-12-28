using UnityEngine;
using System.Collections;
/**
    子弹超过界面范围消除
*/
public class DestroyByBoundary : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerExit(Collider other) {
        Destroy(other.gameObject);
    }
}
