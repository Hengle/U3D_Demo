using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    //要跟随的目标
    public GameObject target;
    //初始位置
    private Vector3 offset; 

	// Use this for initialization
	void Start () {
        //记录摄像机的初始位置
        offset = transform.position;
    }
	
	// Update is called once per frame
	void LateUpdate ()
    {
        transform.position = target.transform.position + offset;
    }
}
