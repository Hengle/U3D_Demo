using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 坦克健康控制类
/// </summary>
public class TankHealth : MonoBehaviour
{

    public float maxHP = 100.0f; // 最大生命值
    public Slider sliderHP; // 血条滑块
    public Image sliderHPfile; // 血条填充图片
    public Color healthColor = Color.green; // 健康的血条颜色
    public Color noHealthColor = Color.red; // 健康的血条颜色
    public GameObject explosionPrefab;// 爆炸特效对象

    // 当前血量 
    private float nowHp = 100;

    public float NowHp
    {
        set
        {
            if (value < 0)
            {
                value = 0;
            }
            this.nowHp = value;
        }
        get
        {
            return this.nowHp;
        }
    }
    private ParticleSystem explosionParticle; // 爆炸的粒子效果
    private AudioSource explosionAudio; // 爆炸声音
    private bool isDead; // 是否死亡

    // 对象被调用是运行一次
    private void Awake()
    {
        // 克隆对象explosionPrefab，并且获得他的粒子效果
        explosionParticle = Instantiate(explosionPrefab).GetComponent<ParticleSystem>();
        // 爆炸声音
        explosionAudio = explosionParticle.GetComponent<AudioSource>();
        // 将这个爆炸效果设置为 未激活
        explosionParticle.gameObject.SetActive(false);
    }

    // 脚本可用时被调用，如果不可用就不会被调用，调用在awake之后，Start之前
    private void onEable()
    {
        this.nowHp = this.maxHP;
        this.isDead = false;

        // 设置血量条
        this.setSliderHP();
    }

    public void TakeDamage(float hurt)
    {
       this.NowHp -= hurt;
        this.setSliderHP();

        Debug.Log("当前生命值：" + this.nowHp);
        if (this.nowHp <= 0 && !this.isDead)
        {
            // 坦克被毁坏
            onDeadth();
        }
    }
    
    // 坦克毁坏
    private void onDeadth()
    {
        this.isDead = true;
        explosionParticle.transform.position = this.transform.position;
        explosionParticle.gameObject.SetActive(true);

        explosionParticle.Play();
        explosionAudio.Play();

        this.gameObject.SetActive(false);
    }

    // 对象第一次刷帧是被调用一次
    void Start()
    {
        // 设置血量条
        this.setSliderHP();
    }

    // 每帧都执行
    void FixedUpdate()
    {

    }

    // 并不是每帧都刷新，在机器性能不足时，可能不刷新
    void Update()
    {

    }

    // 设置血量条，颜色会根据血量渐变
    private void setSliderHP()
    {
        sliderHP.value = this.nowHp; // 设置血量滑轮的值
        // 在起始颜色和结束颜色上取一个颜色
        sliderHPfile.color = Color.Lerp(this.noHealthColor, this.healthColor, this.nowHp / this.maxHP);
    }

}
