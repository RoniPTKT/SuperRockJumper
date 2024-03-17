using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private ParticleSystem dirtParticle;
    [SerializeField] private ParticleSystem landingParticles;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip crashSound;
    [SerializeField] private AudioClip confirmSound;
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject scoreBoard;
    public GameObject scoreString;
    [SerializeField] private GameObject spawner;
    public float score;
    private SpawnManager spawnManagerScript;
    private GameObject[] obstacles;
    private GameObject[] triggers;
    private AudioSource playerAudio;
    public float jumpForce;
    public float gravityMultiplier;
    public bool grounded;
    public bool gameStarted;
    public bool gameOver;
    private Animator playerAnim;

    void Start()
    {
        score = 0;
        playerAnim = GetComponent<Animator>();
        Physics.gravity *= gravityMultiplier;
        playerRb = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();
        spawnManagerScript = spawner.GetComponent<SpawnManager>();
    }

    void Update()
    {
        //Input for starting the game
        if (!gameStarted && Input.GetKeyDown(KeyCode.Space))
        {
            Invoke("startGame", 0.1f);
            if (startScreen.activeSelf)
            {
                playerAudio.PlayOneShot(confirmSound);
            }
            startScreen.SetActive(false);
            scoreBoard.SetActive(true);
        }

        //Input for restarting the game after dying
        if (gameOver && Input.GetKeyDown(KeyCode.Space))
        {
            gameOverScreen.SetActive(false);



            //Call the reset method after a small delay
            Invoke("restartGame", 0.5f);
        }

        //Make the player characer jump with an animation when space is pressed
        if (Input.GetKeyDown(KeyCode.Space) && grounded && !gameOver && gameStarted) 
        {
            playerAudio.PlayOneShot(jumpSound, 1.0f);
            dirtParticle.Stop();
            playerRb.AddForce(this.transform.up * jumpForce, ForceMode.Impulse);
            playerAnim.SetTrigger("Jump_trig");
            grounded = false;
        }



    }

    private void FixedUpdate()
    {
        //Check if the game is running and the player is jumping
        if (!grounded && !gameOver)
        {
            //Make the player character do a flip when jumping
            transform.Rotate(Vector3.forward, -7.8f, Space.World);
        }
        if (grounded)
        {
            //Set the player character upright when it comes into contact with the ground
            resetRot();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Flag for touching the ground
        if (collision.gameObject.CompareTag("Ground") && !gameOver)
        {
            playerAnim.Play("Run_Static");
            grounded = true;
            dirtParticle.Play();
            if (landingParticles.isStopped)
            {
                landingParticles.Play();
            }
        }

        //Check if player passes an obstacle with a collider
        if (collision.gameObject.CompareTag("Score"))
        {
            //Increase score count and update the score UI
            score += 10;
            scoreString.GetComponent<TextMeshProUGUI>().text = "Score: " + score;

            //Destroy the collider to prevent colliding with it multiple times
            Destroy(collision.gameObject);
        }

        //Check if player hits an obstacle
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            //Play sounds and particles
            playerAudio.PlayOneShot(crashSound, 1.0f);
            explosionParticle.Play();
            dirtParticle.Stop();

            //Reset the character rotation
            resetRot();

            //Play death animation and end the game
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
            gameOverScreen.SetActive(true);
            gameOver = true;
        }

    }

    public void resetRot()
    {
        transform.rotation = Quaternion.Euler(0, 90, 0);
    }

    public void startGame()
    {
        gameStarted = true;
    }

    public void restartGame()
    {
        //Find all obstacles and score triggers and delete them
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obstacle in obstacles)
        {
            Destroy(obstacle);
        }
        triggers = GameObject.FindGameObjectsWithTag("Score");
        foreach (GameObject trigger in triggers)
        {
            Destroy(trigger);
        }

        //Reset score (never would've guessed)
        score = 0;

        //Reset player animations
        playerAnim.SetBool("Death_b", false);
        playerAnim.SetInteger("DeathType_int", 0);
        playerAnim.Rebind();

        //Reset gameover state
        gameOver = false;

        //Check if the obstacle spawner had finished its loop (to prevent it running twice at the same time)
        if (spawnManagerScript.isSpawning == false)
        {
            //Restart the obstacle spawner method
            StartCoroutine(spawnManagerScript.spawnTimer());
        }
    }


}
