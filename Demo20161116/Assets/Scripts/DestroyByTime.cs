using UnityEngine;
using System.Collections;
//爆炸效果回收
public class DestroyByTime : MonoBehaviour
{

    public float lifeTime;

    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
