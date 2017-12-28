using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public float speed;
    private int count;//分数
    public Text countText;
    // Use this for initialization
    void Start () {
        count = 0;
    }
	
	// Update is called once per frame
	void Update () {
        Console.WriteLine("moveHorizontal");
    }

    //触发器进入时触发
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "PickUp") {
            other.gameObject.SetActive(false);
            Debug.Log("OnTriggerEnter");
        }
    }
    //碰撞器进入时触发
    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "PickUp")
        {
            collision.gameObject.SetActive(false);
            count++;
            updateCountText();
            Debug.Log("OnCollisionEnter");
        }
    }

    private void updateCountText() {
        countText.text = "Count:" + count;
    }

    void FixedUpdate() {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
       // Debug.Log("moveHorizontal: " + moveHorizontal);
       // Debug.Log("moveVertical:" + moveVertical);
        Vector3 movement = new Vector3(moveHorizontal,0.0f, moveVertical);
        GetComponent<Rigidbody>().AddForce(movement * speed * Time.deltaTime);
    }
}
