using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public GameObject passDetector;
    public float startDelay;
    public float repeatRate;
    private float randDelay;
    public float minInterval;
    public float maxInterval;
    public bool isSpawning;
    private PlayerController playerControllerScript;

    void Start()
    { 
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        StartCoroutine(spawnTimer());
    }

     public IEnumerator spawnTimer()
    {
        //Checks if the game is still running
        while (playerControllerScript.gameOver == false)
        {
            isSpawning = true;

            //Set a random delay between the spawns and wait that duration
            randDelay = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(randDelay);
            //Call the spawn method
            Invoke("spawnObstacle", 0f);
        }
        isSpawning = false;
    }

    void spawnObstacle()
    {
        if (playerControllerScript.gameOver == false && playerControllerScript.gameStarted == true)
        {
            Instantiate(passDetector, transform.position, passDetector.transform.rotation);
            Instantiate(obstaclePrefab, transform.position, obstaclePrefab.transform.rotation);
        }
    }
}
