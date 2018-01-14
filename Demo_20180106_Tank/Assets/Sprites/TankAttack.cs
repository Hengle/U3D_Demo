using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAttack : MonoBehaviour
{

    // 子弹的起始位置
    private Transform point;
    // 子弹
    public GameObject shell;

    public float shellSpeed = 10;


    public KeyCode keycode = KeyCode.Space;

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

        }


    }
}
