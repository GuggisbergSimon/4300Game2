﻿using System.Collections;
using UnityEngine;

public class Racket : MonoBehaviour
{
	[SerializeField] private float timeChargingHit = 0.0f;
	[SerializeField] private float hitBasePower = 5.0f;
	[SerializeField] private float hitPowerPerSecCharging = 5.0f;
	[SerializeField] private float maximumChargingTime = 5.0f;
	[SerializeField] private float hitDuration = 0.2f;
	[SerializeField] private Vector2 aimPosition;
	[SerializeField] private float hitSpeed = 500.0f;

	//[SerializeField] private float racketCounterRotation = -0.5f;
	//[SerializeField] private float racketRotationSpeed = 50.0f;
	[SerializeField] private float smashMaxCharge = 100.0f;
	[SerializeField] private float amountOfChargeToSmash = 25.0f;
	[SerializeField] private float smashTimeScale = 0.5f;
	[SerializeField] private float smashPower = 50.0f;
	[SerializeField] private float smashSpeed = 1000.0f;
	[SerializeField] private float smashDuration = 0.1f;
	[SerializeField] private float prepareToSmashMaximumDuration = 1.5f;
	[SerializeField] private float timeCameraShaking = 0.2f;
	[SerializeField] private float amplitudeCameraShaking = 2.0f;
	[SerializeField] private float frequencyCameraShaking = 2.0f;

	private Ball ball;
	private PlayerMove player;
	private Vector2 racketDirection;
	private bool isFacingRightAtStartup;
	private bool hitBall = false;
	private RacketStates myState = RacketStates.Idle;
	private float timeHitting = 0.0f;
	public Quaternion RotationBeforeHitting { get; private set; }
	public float SmashMaxCharge => smashMaxCharge;
	public float SmashCurrentCharge { get; private set; } = 0.0f;
	public float AmountOfChargeToSmash => amountOfChargeToSmash;
	private float prepareToSmashDuration;

	private enum RacketStates
	{
		Idle,
		PrepareToHit,
		PrepareToSmash,
		Hitting,
		Smashing,
	}

	void Start()
	{
		// TODO add gameManager method to change racketRotationSpeed

		ball = GameManager.Instance.Ball.GetComponent<Ball>();
		player = GetComponentInParent<PlayerMove>();
		if (transform.right.x > 0)
		{
			isFacingRightAtStartup = true;
		}
		else
		{
			isFacingRightAtStartup = false;
		}
	}

	// Controller sets direction to the joystick's direction

	// Keyboard rotates direction clockwise or counter clockwise

	void Update()
	{
		if (player.MyController != null)
		{
			switch (myState)
			{
				case RacketStates.Idle:
				{
					RotationBeforeHitting = transform.rotation;
					RotateRacketThroughInput();
					if ((player.MyController.RightTrigger.WasPressed || player.MyController.LeftTrigger.WasPressed) &&
					    SmashCurrentCharge >= amountOfChargeToSmash && Mathf.Sign(ball.transform.position.x)
						    .CompareTo(Mathf.Sign(player.transform.position.x)) == 0)
					{
						SmashCurrentCharge -= amountOfChargeToSmash;
						GameManager.Instance.UpdateUI();
						GameManager.Instance.ChangeTimeScale(smashTimeScale);
						player.transform.position = (Vector2) ball.transform.position - aimPosition;
						ball.SetVelocity(Vector2.zero);
						ball.SetGravityScale(0.0f);
						player.SetVelocity(Vector2.zero);
						player.SetGravityScale(0.0f);
						player.EnableMove = false;
						myState = RacketStates.PrepareToSmash;
					}
					else if (player.MyController.RightBumper.WasPressed || player.MyController.LeftBumper.WasPressed)
					{
						myState = RacketStates.PrepareToHit;
					}

					break;
				}
				case RacketStates.PrepareToSmash:
				{
					RotateRacketThroughInput();
					GameManager.Instance.ChangeTimeScale(smashTimeScale);
					prepareToSmashDuration += Time.deltaTime;
					RotationBeforeHitting = transform.rotation;
					if (player.MyController.RightTrigger.WasReleased || player.MyController.LeftTrigger.WasReleased ||
					    (prepareToSmashDuration >= prepareToSmashMaximumDuration))
					{
						prepareToSmashDuration = 0.0f;
						GameManager.Instance.ChangeTimeScale(1.0f);
						myState = RacketStates.Smashing;
						player.SetGravityScale(4.0f);
						player.EnableMove = true;
						SendBall(smashPower);
						ball.SetTrailActive(true, true);
					}

					break;
				}
				case RacketStates.PrepareToHit:
				{
					RotateRacketThroughInput();
					timeChargingHit += Time.deltaTime;
					RotationBeforeHitting = transform.rotation;
					if (timeChargingHit > maximumChargingTime)
					{
						timeChargingHit = maximumChargingTime;
					}

					if (player.MyController.RightBumper.WasReleased || player.MyController.LeftBumper.WasReleased)
					{
						myState = RacketStates.Hitting;
						RotationBeforeHitting = transform.rotation;
					}

					break;
				}
				case RacketStates.Hitting:
				{
					timeHitting += Time.deltaTime;
					RotateHit(hitSpeed);
					if (timeHitting >= hitDuration)
					{
						timeHitting = 0.0f;
						timeChargingHit = 0.0f;
						hitBall = false;
						myState = RacketStates.Idle;
						transform.rotation = RotationBeforeHitting;
					}

					break;
				}
				case RacketStates.Smashing:
				{
					timeHitting += Time.deltaTime;
					RotateHit(smashSpeed);
					if (timeHitting >= smashDuration)
					{
						timeHitting = 0.0f;
						hitBall = false;
						myState = RacketStates.Idle;
						transform.rotation = RotationBeforeHitting;
					}

					break;
				}
			}
		}
	}

	//Rotate the racket depending on its orientation
	private void RotateHit(float speed)
	{
		//TODO, rewrite more properly the following 5 lines, it's super ugly rn
		Vector3 orientation = Vector3.forward;
		if (!isFacingRightAtStartup)
		{
			orientation *= -1;
		}

		gameObject.transform.Rotate(orientation * speed * Time.deltaTime);
	}

	private void RotateRacketThroughInput()
	{
		//update racket direction only if new input received, and normalized racketdirection so that it's always the same basic speed applied anywhere
		if (player.MyController.RightStick)
		{
			racketDirection = player.MyController.RightStick;
			racketDirection.Normalize();
			gameObject.transform.right = racketDirection;
		}
	}


	private void SendBall(float power)
	{
		ball.ResetPhysics();
		hitBall = true;
		if (player.PlayerNumber != ball.LastPlayerHitting)
		{
			StartCoroutine(GameManager.Instance.MyCameraManager.ShakeCameraFor(timeCameraShaking,
				power * amplitudeCameraShaking, frequencyCameraShaking));
			GameManager.Instance.MyCameraManager.ChangeColorBackground();
			ball.LastPlayerHitting = player.PlayerNumber;
			ball.SetVelocity(racketDirection * power);
		}
		else
		{
			ball.LastPlayerHitting = ball.LastPlayerHitting.GetOpponent();
			GameManager.Instance.MyMatchManager.AddPointTo(player.PlayerNumber.GetOpponent());
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		HandleCollision(collision);
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		HandleCollision(collision);
	}

	private void HandleCollision(Collider2D collision)
	{
		if (collision.gameObject.tag == ("Ball") && !hitBall)
		{
			if (myState == RacketStates.Hitting)
			{
				ball.SetTrailActive(true, false);
				SmashCurrentCharge += ball.GetSmashCharge();
				GameManager.Instance.UpdateUI();
				if (SmashCurrentCharge > smashMaxCharge)
				{
					SmashCurrentCharge = smashMaxCharge;
				}

				float hitPower = hitBasePower + hitPowerPerSecCharging * timeChargingHit;
				SendBall(hitPower);
			}
		}
	}
}