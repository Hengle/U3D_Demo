using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{


    public GameObject e;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

        GameObject.Instantiate(e, this.transform.position, this.transform.rotation);
        GameObject.Destroy(this.gameObject);
    }
}
