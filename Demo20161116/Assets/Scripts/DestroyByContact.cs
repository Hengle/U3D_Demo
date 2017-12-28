using UnityEngine;
using System.Collections;
//障碍物爆炸
public class DestroyByContact : MonoBehaviour
{

    public GameObject explosion;
    public GameObject playerExplosion;

    public float speed;

    public int oneScore;
    private GameController gameController;


    // Use this for initialization
    void Start()
    {
        GameObject gameObject = GameObject.FindWithTag("GameController");
        if (gameObject != null)
        {
            this.gameController = gameObject.GetComponent<GameController>();
        }
        else
        {
            Debug.Log("什么鬼！！");
        }

        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Boundary")
        {
            return;
        }

        if (other.tag == "Player")
        {
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
            this.gameController.gameOver();
        }
        else
        {
            this.gameController.addScore(this.oneScore);
        }

        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(other.gameObject);
        Destroy(gameObject);
    }
}
