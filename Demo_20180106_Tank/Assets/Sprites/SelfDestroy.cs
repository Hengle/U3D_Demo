using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{

    // 生存时间
    public float time = 5;

    // Use this for initialization
    void Start()
    {
        GameObject.Destroy(this.gameObject, this.time);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
