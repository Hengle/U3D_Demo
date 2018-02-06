using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Shell : MonoBehaviour
{
    public LayerMask tankMask; // 发生爆炸效果的图层
    public ParticleSystem explosionParticy; // 爆炸效果的粒子系统
    public AudioSource explosionAudio; // 爆炸的声音效果
    public float maxDamage = 100f;// 最大伤害值
    public float explosionForce = 1000f; // 爆炸向外辐射的力的大小
    public float maxLifeTime = 2f;// 最大生存时间，如果没有碰到任何物体在这个时间后自动销毁
    public float explosionRadius = 5f; // 爆炸范围

    //public GameObject e;

    // Use this for initialization
    void Start()
    {
        // 在规定时间后自动销毁
        Destroy(this.gameObject, maxLifeTime);
    }

    // Update is called once per frame
    void FixedUpdate()
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
        // GameObject.Instantiate(e, this.transform.position + Vector3.up, this.transform.rotation);
        // GameObject.Destroy(this.gameObject);
        //if (other.tag == "Tank")
        //{
        //other.SendMessage("TakeDamage");
        //}
        // 将会返回以参数1为原点和参数2为半径的球体内“满足一定条件”的碰撞体集合
        Collider[] collider = Physics.OverlapSphere(this.transform.position, this.explosionRadius, this.tankMask);
        Debug.Log("长度："+ collider.Length);
        for (int i = 0; i < collider.Length; i++)
        {
            Debug.Log("1");
            Rigidbody rigidbody = collider[i].GetComponent<Rigidbody>();
            Debug.Log("2");
            // 如果没有刚体组件
            if (!rigidbody)
            {
                Debug.Log("没有刚体组件");
                continue;
            }
            // 施加一个向外推的爆炸力，explosionForce 爆炸的力大小，爆炸的位置，爆炸的半径（在这个半径外的物体不会受到作用，如果为0，则无视这个距离）
            rigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            
            TankHealth tankHealth = rigidbody.GetComponent<TankHealth>();
            // 如果tankHealth为null
            if (!tankHealth)
            {
                Debug.Log("没有找到TankHealth");
                continue;
            }
            // 计算伤害
            float damage = calDamage(rigidbody.position);
            Debug.Log("造成伤害：" + damage);
            tankHealth.TakeDamage(damage);
        }
        // 清除父层关系
        explosionParticy.transform.parent = null;
        // 播放爆炸效果
        explosionParticy.Play();
        // 播放音效
        explosionAudio.Play();
        // 效果播放完后，清除爆炸效果。duration粒子的持续时间
        Destroy(explosionParticy.gameObject, explosionParticy.main.duration);

        Destroy(this.gameObject);
    }

    // 计算伤害
    private float calDamage(Vector3 tarPos)
    {
        Vector3 juLiV3 = tarPos - transform.position;
        // Vector3.magnitude 向量的长度
        float juLi = juLiV3.magnitude;
        float damage = maxDamage * (explosionRadius - juLi) / explosionRadius;

        return Mathf.Max(0f, damage);
    }

}
