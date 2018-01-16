using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour
{

    public float speed = 5;
    public float aApeed = 10;// 每秒10度
    public float num = 1;
    public AudioSource moveAudio;// 声音组件
    public AudioClip engineIdle;// 引擎待机声
    public AudioClip engineRun;// 引擎启动声

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
        float vertical = Input.GetAxis("VerticalPlayer" + num);
        //Debug.Log("v=" + vertical);

        // 自身向前的方向 * 按键的 * 速度
        rigidbody.velocity = this.transform.forward * vertical * speed;

        float h = Input.GetAxis("HorizontalPlayer" + num);
        //Debug.Log("h=" + h);
        // 旋转速度为 围绕Y轴（up）旋转
        rigidbody.angularVelocity = this.transform.up * aApeed * h;
    }
}
