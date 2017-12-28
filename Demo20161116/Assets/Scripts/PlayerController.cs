using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{

    public float speed;
    public float tilt;
    public Boundary boundary;

    private float nextFire;//下次开火时间
    public float fireRate;//开火间隔时间
    public GameObject shot;//子弹
    public Transform shotSqawn;//发射点

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        movement = movement * speed;
        GetComponent<Rigidbody>().velocity = movement;

        GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 0.0f, movement.x * -tilt);

        Vector3 position = GetComponent<Rigidbody>().position;

        GetComponent<Rigidbody>().position = new Vector3(
            Mathf.Clamp(position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(position.z, boundary.zMin, boundary.zMax)
            );

        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + this.fireRate;
            GetComponent<AudioSource>().Play();
            Instantiate(shot, shotSqawn.position, shotSqawn.rotation);

        }
    }
}
