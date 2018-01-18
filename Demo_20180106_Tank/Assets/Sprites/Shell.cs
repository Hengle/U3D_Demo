using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Shell : MonoBehaviour
{


    public GameObject e;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        double xRota = 0;// 子弹在x轴上的旋转角度
        Rigidbody rigidbody = this.GetComponent<Rigidbody>();
        Vector3 v = rigidbody.velocity;
        //Debug.Log("x=" + v.y);
        double tanf = v.y / Math.Pow((Math.Pow(v.x, 2.0) + Math.Pow(v.z, 2.0)), 0.5);
        //Debug.Log("tan=" + tanf);
        xRota = Mathf.Atan((float)tanf) * 180 / Math.PI;
        //Debug.Log("f=" + xRota);

        // this.transform.rotation = Quaternion.AngleAxis((float)xRota, Vector3.up);
        Quaternion q = this.transform.rotation;
        Vector3 qv = q.eulerAngles;
        qv.x = (float)-xRota;
        this.transform.rotation = Quaternion.Euler(qv);

        //Debug.Log("x="+ q.z);
        //q[0] = (float)xRota;
    }

    private void OnTriggerEnter(Collider other)
    {

        GameObject.Instantiate(e, this.transform.position, this.transform.rotation);
        GameObject.Destroy(this.gameObject);
    }
}
