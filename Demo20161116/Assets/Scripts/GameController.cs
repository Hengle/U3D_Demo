using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public GameObject hazard;
    public Vector3 spawnValue;
    public int amount;

    public float spawnWait;
    public float startWait;
    public float waveWait;

    private int score;
    public Text scoreText;

    private bool isOver;
    public Text overText;

    private bool restart;
    public Text restartText;

    // Use this for initialization
    void Start()
    {
        score = 0;
        UpdateScore();
        StartCoroutine(SpawnWaves());

        isOver = false;
        this.overText.text = "";

        this.restart = false;
        restartText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (this.restart && Input.GetKeyDown(KeyCode.Space))
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    public void addScore(int value)
    {
        this.score += value;
        UpdateScore();
    }

    public void gameOver()
    {
        this.isOver = true;
        this.overText.text = "Game Over";

        this.restart = true;
        this.restartText.text = "Press 'Space' to Restart";
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {

            for (int i = 0; i < amount; i++)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValue.x, spawnValue.x), spawnValue.y, spawnValue.z);
                Quaternion spawnRotation = Quaternion.identity;//初始的旋转值
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);
        }

    }
}
