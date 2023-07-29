using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private Animator playerAnim;

    private AudioSource playerAudio;
    public AudioClip jumpSound;
    public AudioClip crashSound;

    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;

    private float jumpForce = 23.0f;
    private float gravityModifier = 6.0f;
    private bool isOnGround = true;
    private bool canDoubleJump = false;
    public bool dash = false;
    public bool gameOver = false;
    private int score;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        Physics.gravity *= gravityModifier; 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !gameOver)
        {
            Jump();
            isOnGround = false;
            canDoubleJump = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !isOnGround && !gameOver && canDoubleJump)
        {
            Jump();
            canDoubleJump = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !gameOver)
        {
            dash = true;

        }

        if (Input.GetKeyUp(KeyCode.LeftShift) && !gameOver)
        {
            dash = false;
        }

        ScoreTracker();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            dirtParticle.Play();
        } else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Game over!");
            gameOver = true;
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
            explosionParticle.Play();
            dirtParticle.Stop();
            playerAudio.PlayOneShot(crashSound, 1.0f);
        }
        
    }

    void Jump()
    {
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        playerAnim.SetTrigger("Jump_trig");
        playerAudio.PlayOneShot(jumpSound, 1.0f);
        dirtParticle.Stop();
    }

    void ScoreTracker()
    {
        if (!dash)
        {
            score++;
        }
        else if (dash)
        {
            score += 2;
        }
        Debug.Log(score);
    }
}
