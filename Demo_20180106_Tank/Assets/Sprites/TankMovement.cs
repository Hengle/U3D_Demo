using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour
{

    public float speed = 5;

    private Rigidbody rigidbody;

    // Use this for initialization
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {   // 表示垂直键（前后键）,vertical 在-1到1之间，这里的vertical是一个渐进的值，可以用这个特点做类似加速度之类的效果，来区分一下不同类型的坦克
        // 或者设计车重，在根据发动机功率计算加速度。
        float vertical = Input.GetAxis("Vertical");

        // 自身向前的方向 * 按键的 * 速度
        rigidbody.velocity = this.transform.forward * vertical * speed;



    }
}
