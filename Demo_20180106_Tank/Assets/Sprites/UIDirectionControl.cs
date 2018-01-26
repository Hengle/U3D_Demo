using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
     坦克血量ui的脚本
*/
public class UIDirectionControl : MonoBehaviour
{

    // 是否实时旋转
    public bool useRelativeRotation = true;

    // 最初的物体旋转角度
    private Quaternion relativeRotation;

    // Use this for initialization
    void Start()
    {
        // 初始角度等于父物体的旋转角度
        relativeRotation = transform.parent.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        // 如果不实时旋转，则物品旋转角度始终等于最初的旋转角度
        if (!this.useRelativeRotation)
        {
            transform.rotation = relativeRotation;
        }
    }
}
