using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{

    public float speed;
    private float leftBound = -15;
    private PlayerController playerControllerScript;

    void Start()
    {
        //Find the player object at start
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        //Keep moving to the left as long as the game is running
        if (playerControllerScript.gameOver == false)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }

        //Destroy the object when it reaches the boundary
        if (transform.position.x < leftBound)
        {
            Destroy(gameObject);
        }
    }
}
