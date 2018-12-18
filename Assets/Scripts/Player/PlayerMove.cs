﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using InControl;

public class PlayerMove : MonoBehaviour
{
	[SerializeField] private Rigidbody2D playerRigidbody2D;
	[SerializeField] private float playerMaxSpeed = 14.0f;
	[SerializeField] private float playerAcceleration = 70.0f;
	[SerializeField] private float jumpThreshold = 0.1f;
	[SerializeField] private float playerJumpSpeed = 20.0f;
	[SerializeField] private bool playerIsJumping = false;
    [SerializeField] private AudioClip jumpSound;

    private AudioSource myAudioSource;

    private bool enableMove = true;

	public bool EnableMove
	{
		get { return enableMove; }
		set { enableMove = value; }
	}

	private InputDevice myController;
	public InputDevice MyController => myController;

	private playerNumber playerNumber;
	public playerNumber PlayerNumber => playerNumber;

	private Racket myRacket;
	public Racket MyRacket => myRacket;

	public void AssignController(InputDevice controller)
	{
		myController = controller;
	}

	public void SetVelocity(Vector2 velocity)
	{
		playerRigidbody2D.velocity = velocity;
	}

	public void SetGravityScale(float gravityScale)
	{
		playerRigidbody2D.gravityScale = gravityScale;
	}

	private void Start()
	{
        myAudioSource = GetComponent<AudioSource>();
        myRacket = GetComponentInChildren<Racket>();

		//TODO temporarily code, change it later perhaps
		if (transform.position.x > 0)
		{
			playerNumber = playerNumber.Player2;
		}
		else
		{
			playerNumber = playerNumber.Player1;
		}
	}

	private void Update()
	{
		if (myController != null && EnableMove)
		{
			float horizontal = myController.LeftStickX;

			if (Math.Abs(playerRigidbody2D.velocity.x) <= playerMaxSpeed)
			{
				Vector2 playerNewVelocity = playerRigidbody2D.velocity;

				playerNewVelocity += new Vector2(playerAcceleration, 0.0f) * horizontal * Time.deltaTime;

				playerRigidbody2D.velocity = playerNewVelocity;
			}

			if (myController.LeftStickY > jumpThreshold && !playerIsJumping)
			{
                myAudioSource.PlayOneShot(jumpSound);
				playerIsJumping = true;
				playerRigidbody2D.velocity += new Vector2(0.0f, playerJumpSpeed);
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Ground")
		{
			playerIsJumping = false;
		}
	}
}