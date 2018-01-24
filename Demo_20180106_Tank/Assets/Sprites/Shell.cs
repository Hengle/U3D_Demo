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
        Vector3 v = rigidbody.velocity;//获取子弹的当前速度
        //Debug.Log("x=" + v.y);
        double tanf = v.y / Math.Pow((Math.Pow(v.x, 2.0) + Math.Pow(v.z, 2.0)), 0.5);// 计算速度的tan值
        //Debug.Log("tan=" + tanf);
        xRota = Mathf.Atan((float)tanf) * 180 / Math.PI;// 通过tan值和反正切函数计算角度
        //Debug.Log("f=" + xRota);

        // this.transform.rotation = Quaternion.AngleAxis((float)xRota, Vector3.up);
        Quaternion q = this.transform.rotation;// 子弹的角度的四元素
        Vector3 qv = q.eulerAngles;// 转换成3元
        qv.x = (float)-xRota;// 设置以X为轴的旋转度数
        this.transform.rotation = Quaternion.Euler(qv);// 转换成4元

        //Debug.Log("x="+ q.z);
        //q[0] = (float)xRota;
    }

    private void OnTriggerEnter(Collider other)
    {

        GameObject.Instantiate(e, this.transform.position + Vector3.up, this.transform.rotation);
        GameObject.Destroy(this.gameObject);

        if (other.tag == "Tank")
        {
            other.SendMessage("TankD");

        }

    }
}
