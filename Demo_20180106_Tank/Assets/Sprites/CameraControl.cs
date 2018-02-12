using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float cacheSize = 2;// 摄像机缓冲区
    public float minSize = 10f;// 摄像机最小大小
    public float moveTime = 0.2f;// 摄像机移动时间
    public float zoomTime = 0.2f;// 摄像机缩放的时间
    // [HideInInspector] public Transform[] m_Targets; 
    [System.NonSerialized]// 不保存序列化文件，HideInInspector只是在面板隐藏
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
        tanks = GameObject.FindGameObjectsWithTag("Tank");
    }

    private void FixedUpdate()
    {
        // Debug.Log("x="+this.mainCamera.transform.position.x);
        // Debug.Log("y=" + this.mainCamera.transform.position.y);
        //Debug.Log("z=" + this.mainCamera.transform.position.z);
        Vector3 pos = this.getCameraPos();// 摄像机位置
        // SmoothDamp 阻尼滑动
        this.transform.position = Vector3.SmoothDamp(this.transform.position, pos, ref moveSpeed, moveTime);
        // SmoothDamp 渐变缩放
        //  this.mainCamera.orthographicSize = Mathf.SmoothDamp(this.mainCamera.orthographicSize, size, ref zoomSpeed, zoomTime);
        Zoom(pos);
    }

    void Update()
    {
    }

    private void Zoom(Vector3 m_DesiredPosition)
    {
        float requiredSize = FindRequiredSize(m_DesiredPosition);
        mainCamera.orthographicSize = Mathf.SmoothDamp(mainCamera.orthographicSize, requiredSize, ref zoomSpeed, zoomTime);
    }

    /**
     * 在计算相机大小时以相机的最终目标位置为依据计算。
     * 这里在计算所有用到的东西的时候都以现在CameraRig为原点建立的坐标系，摄像机位于（0，0，-65）
     * 所以Z轴上的坐标对于计算距离有没意义（这里忽略坦克本身的高度）
     * 这里摄像机是照到一个倾斜的场景上，坦克在这个场景上活动，摄像机的Size是改变垂直的大小，即改变XY轴上的大小值。
     * 所以忽略Z轴
     */
    private float FindRequiredSize(Vector3 m_DesiredPosition)
    {
        // 计算相对位置，目标位置和当前位置有一个差值
        // 计算目标点和当前相机的相对位置坐标，受相机旋转影响
          Vector3 desiredLocalPos = transform.InverseTransformPoint(m_DesiredPosition);
      //  Vector3 desiredLocalPos = m_DesiredPosition;

        float size = 0f;

        for (int i = 0; i < tanks.Length; i++)
        {
            if (!tanks[i].activeSelf)
                continue;
            // 算计tank和相机的相对位置
            Vector3 targetLocalPos = transform.InverseTransformPoint(tanks[i].transform.position);
            // 位置的差值
            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;
            // 因为摄像机在Z轴上，Z轴为光轴，所以Z轴上的坐标没有意义。
            // 如果是X轴旋转90度，就可以直接用下面的算法
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));
            // orthographicSize的大小以宽为标准，orthographicSize为9，长宽比为16:9时，长为16，宽9，所以计算宽时除以长宽比
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / mainCamera.aspect);// aspect 长宽比
        }

        size += cacheSize;

        size = Mathf.Max(size, minSize);

        return size;
    }

    /**
     *   计算摄像机当前应处的位置
     */
    private Vector3 getCameraPos()
    {
        Vector3 pos = this.transform.position;//摄像机的位置       
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
            //Debug.Log("x=" + pos.x);
            //Debug.Log("y=" + pos.y);
            // Debug.Log("z=" + pos.z);
            return pos;
        }
        pos.x = (maxX + minX) / 2;
        pos.z = (maxZ + minZ) / 2;

        //Debug.Log("x=" + pos.x);
        //Debug.Log("y=" + pos.y);
        //Debug.Log("z=" + pos.z);
        return pos;
    }

}
