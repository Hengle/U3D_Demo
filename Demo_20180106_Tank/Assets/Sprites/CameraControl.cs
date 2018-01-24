using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float cacheSize = 6;// 摄像机缓冲区
    public float minSize = 10f;// 摄像机最小大小
    public float moveTime = 0.2f;// 摄像机移动时间
    public float zoomTime = 0.2f;// 摄像机缩放的时间
    // [HideInInspector] public Transform[] m_Targets; 
    
    public GameObject[] tanks; // 坦克列表，HideInInspector 表示将原本显示在面板inspector上的序列化值隐藏起来。

    private Vector3 moveSpeed;// 摄像机移动速度
    private float zoomSpeed;// 摄像机缩放的速度
    private Camera mainCamera = null;

    void Awake()
    {
        mainCamera = GetComponentInChildren<Camera>();
    }

    void Start()
    {
        // tanks = GameObject.FindGameObjectsWithTag("Tank");
    }

    private void FixedUpdate()
    {
       // Debug.Log("x="+this.mainCamera.transform.position.x);
       // Debug.Log("y=" + this.mainCamera.transform.position.y);
       //Debug.Log("z=" + this.mainCamera.transform.position.z);
        float size;// 摄像机大小
        Vector3 pos = this.getCameraPos(out size);// 摄像机位置
        // SmoothDamp 阻尼滑动
        this.mainCamera.transform.position = Vector3.SmoothDamp(this.transform.position, pos, ref moveSpeed, moveTime);
        // SmoothDamp 渐变缩放
        this.mainCamera.orthographicSize = Mathf.SmoothDamp(this.mainCamera.orthographicSize, size, ref zoomSpeed, zoomTime);
    }

    void Update()
    {
    }

    /**
     *   计算摄像机当前应处的位置
     *   size 摄像机范围大小
     */
    private Vector3 getCameraPos(out float size)
    {
        Vector3 pos = this.mainCamera.transform.position;//摄像机的位置
        size = this.minSize;// 默认摄像机范围最小
        GameObject tank = null;
        float minX = 0;
        float maxX = 0;
        float minZ = 0;
        float maxZ = 0;
        bool isInit = true;
        Vector3 tankPos;
        for (int i = 0; i < this.tanks.Length; i++)
        {
            tank = this.tanks[i];
            // 如果这个坦克已经被销毁，就跳过,activeSelf标记这个tank是否是活动的
            if (tank == null || !tank.activeSelf || tank.transform == null)
            {
                continue;
            }
            tankPos = tank.transform.position;
            if (isInit || minX > tankPos.x)
            {
                minX = tankPos.x;
            }
            if (isInit || maxX < tankPos.x)
            {
                maxX = tankPos.x;
            }
            if (isInit || minZ > tankPos.z)
            {
                minZ = tankPos.z;
            }
            if (isInit || maxZ < tankPos.z)
            {
                maxZ = tankPos.z;
            }
            isInit = false;
        }
        if (isInit)
        {
            Debug.Log("x=" + pos.x);
            Debug.Log("y=" + pos.y);
            Debug.Log("z=" + pos.z);
            return pos;
        }
        pos.x = (maxX + minX) / 2;
        pos.y = (maxZ + minZ) / 2;
        // 计算大小
        float sizeX = maxX - minX;
        float sizeZ = maxZ - minZ;
        if (sizeX > sizeZ)
        {
            size = sizeX + cacheSize;
        }
        else
        {
            size = sizeZ + cacheSize;
        }
        if (size < minSize)
        {
            size = minSize;
        }
        Debug.Log("x=" + pos.x);
        Debug.Log("y=" + pos.y);
        Debug.Log("z=" + pos.z);
        return pos;
    }

}
