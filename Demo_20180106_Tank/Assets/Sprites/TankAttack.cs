using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankAttack : MonoBehaviour
{

    // 子弹的起始位置
    private Transform point;
    // 子弹
    public GameObject shell;

    // 发射的蓄力滑块
    public Slider aimSlider;
    // 最大力度
    public float maxPower = 30f;
    // 最小力度
    public float minPower = 15f;
    // 蓄力的最大时间 单位：秒
    public float maxPowerTime = 0.75f;
    // 发送时播放的声音
    public AudioSource m_ShootingAudio;
    // 蓄力时播放的声音片段
    public AudioClip m_ChargingClip;
    // 发射时播放的声音片段
    public AudioClip m_FireClip;

    public KeyCode keycode = KeyCode.Space;

    // 子弹的力的大小
    private float shellPower;
    // 发射蓄力的速度 value 每秒
    private float powerSpeed;
    // 防止连续发射 true 可以发射 false 不能发射
    private bool isFrie = false;

    private void OnEnable()
    {
        shellPower = minPower;
        aimSlider.value = minPower;
    }

    // Use this for initialization
    void Start()
    {
        point = this.transform.Find("point");

        powerSpeed = (maxPower - minPower) / maxPowerTime;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(keycode))
        {
            isFrie = true;
            // 按下
            this.m_ShootingAudio.clip = this.m_ChargingClip;
            this.m_ShootingAudio.Play();
        }
        else if (Input.GetKey(keycode) && isFrie)
        {
            // 字段威力的成长
            this.shellPower += powerSpeed * Time.deltaTime;
            // 按住了
            aimSlider.value = this.shellPower;

            // 如果已经达到最大值就直接开火
            if (this.shellPower >= this.maxPower)
            {
                this.shellPower = this.maxPower;
                this.Frie();
            }
        }
        else if (Input.GetKeyUp(keycode) && isFrie)
        {
            // 放开
            this.Frie();
        }

    }

    /// <summary>
    /// 开火
    /// </summary>
    private void Frie()
    {
        GameObject zidan = GameObject.Instantiate(shell, point.position, point.rotation);
        Rigidbody rig = zidan.GetComponent<Rigidbody>();
        rig.velocity = this.point.forward * this.shellPower;

        aimSlider.value = this.minPower;
        shellPower = this.minPower;

        this.m_ShootingAudio.clip = this.m_FireClip;
        this.m_ShootingAudio.Play();
        this.isFrie = false;
    }

}
