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

    public float shellSpeed = 10;
    // 发射的蓄力滑块
    public Slider aimSlider;
    // 最大力度
    public float maxPower = 30f;
    // 最小力度
    public float minPower = 15f;

    // 子弹的力的大小
    private float shellPower;

    public KeyCode keycode = KeyCode.Space;


    private void OnEnable()
    {
        shellPower = minPower;
        aimSlider.value = minPower;
    }

    // Use this for initialization
    void Start()
    {
        point = this.transform.Find("point");


    }

    // Update is called once per frame
    void Update()
    {
        // 开火
        if (Input.GetKeyDown(keycode))
        {
            GameObject zidan = GameObject.Instantiate(shell, point.position, point.rotation);
            Rigidbody rig = zidan.GetComponent<Rigidbody>();
            rig.velocity = this.point.forward * shellSpeed;


            aimSlider.value += 1;

        }


    }
}
