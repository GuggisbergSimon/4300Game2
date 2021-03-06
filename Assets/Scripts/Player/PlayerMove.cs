﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using InControl;

public class PlayerMove : MonoBehaviour
{
	[SerializeField] private float playerMaxSpeed = 14.0f;
	[SerializeField] private float playerAcceleration = 70.0f;
	[SerializeField] private float jumpThreshold = 0.1f;
	[SerializeField] private float playerJumpSpeed = 20.0f;
	[SerializeField] private bool playerIsJumping = false;
	[SerializeField] private AudioClip jumpSound;

	private Rigidbody2D playerRigidbody2D;
	private AudioSource myAudioSource;
	public bool EnableMove { get; set; } = true;
	public InputDevice MyController { get; private set; }
	public playerNumber PlayerNumber { get; private set; }
	public Racket MyRacket { get; private set; }

	public void AssignController(InputDevice controller)
	{
		MyController = controller;
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
		MyRacket = GetComponentInChildren<Racket>();
		myAudioSource = GetComponent<AudioSource>();
		playerRigidbody2D = GetComponent<Rigidbody2D>();
		//TODO temporarily code, change it later perhaps
		if (transform.position.x > 0)
		{
			PlayerNumber = playerNumber.Player2;
		}
		else
		{
			PlayerNumber = playerNumber.Player1;
		}
	}

	private void Update()
	{
		if (MyController != null && EnableMove)
		{
			float horizontal = MyController.LeftStickX;

			if (Math.Abs(playerRigidbody2D.velocity.x) <= playerMaxSpeed)
			{
				Vector2 playerNewVelocity = playerRigidbody2D.velocity;

				playerNewVelocity += new Vector2(playerAcceleration, 0.0f) * horizontal * Time.deltaTime;

				playerRigidbody2D.velocity = playerNewVelocity;
			}

			if (MyController.LeftStickY > jumpThreshold && !playerIsJumping)
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