using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMove : MonoBehaviour {

    [SerializeField]
    private Rigidbody2D playerRigidbody2D;
    [SerializeField]
    private float playerMaxSpeed = 5.0f;
    [SerializeField]
    private float playerAcceleration = 50.0f;

    [SerializeField]
    private float playerJumpSpeed = 8.0f;
    [SerializeField]
    private bool playerIsJumping = false;

    void Start ()
    {
		

	}
	
	void Update ()
    {
        float horizontal = Input.GetAxis("Horizontal");

        if(Math.Abs(playerRigidbody2D.velocity.x) <= playerMaxSpeed)
        {
            Vector2 playerNewVelocity = playerRigidbody2D.velocity;

            playerNewVelocity += new Vector2(playerAcceleration, 0.0f) * horizontal * Time.deltaTime;

            playerRigidbody2D.velocity = playerNewVelocity;
        }

        if (Input.GetButton("Jump") && !playerIsJumping)
        {
            playerIsJumping = true;
            playerRigidbody2D.velocity += new Vector2 (0.0f, playerJumpSpeed);
        }
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            playerIsJumping = false;
        }
    }
}
