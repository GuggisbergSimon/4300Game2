using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine;
using InControl;

public class Racket : MonoBehaviour
{
	[SerializeField] private bool isChargingHit = false;
	[SerializeField] private float timeChargingHit = 0.0f;
	[SerializeField] private float hitBasePower = 5.0f;
	[SerializeField] private float hitPowerPerSecCharging = 5.0f;
	[SerializeField] private float maximumChargingTime = 5.0f;
	[SerializeField] private float hitDuration = 0.2f;
	[SerializeField] private float timeHitting = 0.0f;
	[SerializeField] private float hitSpeed = 500.0f;
	[SerializeField] private float racketCounterRotation = -0.5f;
	[SerializeField] private float racketRotationSpeed = 50.0f;
	[SerializeField] private float smashMaxCharge = 100.0f;
	[SerializeField] private float smashCurrentCharge = 0.0f;
	[SerializeField] private float amountOfChargeToSmash = 25.0f;
	[SerializeField] private float smashTimeScale = 0.5f;
	[SerializeField] private float smashPower = 50.0f;
	[SerializeField] private float smashSpeed = 1000.0f;
	[SerializeField] private float smashDuration = 0.1f;

	private Ball ball;
	private PlayerMove player;
	private Vector2 racketDirection;
	private bool isFacingRightAtStartup;

	private bool hitBall = false;

	//private bool isHitting = false;
	//private bool isSmashing = false;
	private RacketStates myState = RacketStates.Idle;

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
					RotateRacketThroughInput();
					if (player.MyController.RightTrigger.WasPressed && smashCurrentCharge >= amountOfChargeToSmash)
					{
						smashCurrentCharge -= amountOfChargeToSmash;
						myState = RacketStates.PrepareToSmash;
					}
					else if (player.MyController.RightBumper.WasPressed)
					{
						myState = RacketStates.PrepareToHit;
					}

					break;
				}
				case RacketStates.PrepareToSmash:
				{
					RotateRacketThroughInput();
					GameManager.Instance.ChangeTimeScale(smashTimeScale);
					if (player.MyController.RightTrigger.WasReleased)
					{
						GameManager.Instance.ChangeTimeScale(1.0f);
						myState = RacketStates.Smashing;
					}

					break;
				}
				case RacketStates.PrepareToHit:
				{
					RotateRacketThroughInput();
					timeChargingHit += Time.deltaTime;
					if (timeChargingHit > maximumChargingTime)
					{
						timeChargingHit = maximumChargingTime;
					}

					if (player.MyController.RightBumper.WasReleased)
					{
						myState = RacketStates.Hitting;
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
					}

					break;
				}
			}

			/*//check if smash is possible and deplete gauge
			if (player.MyController.RightTrigger.WasPressed && smashCurrentCharge >= amountOfChargeToSmash)
			{
				smashCurrentCharge -= amountOfChargeToSmash;
				StartCoroutine(Smash());
			}*/

			//start of a hit
			/*if (player.MyController.RightBumper.WasPressed && !isHitting)
			{
				isChargingHit = true;
			}*/

			//charging a hit
			/*if (isChargingHit)
			{
				timeChargingHit += Time.deltaTime;
			}

			if (player.MyController.RightBumper.WasReleased)
			{
				isChargingHit = false;
				isHitting = true;
			}*/

			/*if (isHitting)
			{
				timeHitting += Time.deltaTime;

				//we rotate the racket depending on its orientation
				//NB : that's super ugly
				Vector3 orientation = Vector3.forward;
				if (!isFacingRightAtStartup)
				{
					orientation *= -1;
				}

				gameObject.transform.Rotate(orientation * hitSpeed * Time.deltaTime);

				if (timeHitting >= hitDuration)
				{
					isHitting = false;
					hitBall = false;
					timeHitting = 0.0f;
					timeChargingHit = 0.0f;
				}
			}*/

			/*if (!isHitting)
			{
				racketDirection = player.MyController.RightStick;
				Debug.DrawLine(transform.position, transform.position + (Vector3) racketDirection * 4.0f);
				if (racketDirection != Vector2.zero)
				{
					gameObject.transform.right = racketDirection.normalized;
				}
			}*/
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
		racketDirection = player.MyController.RightStick;
		Debug.DrawLine(transform.position, transform.position + (Vector3) racketDirection * 4.0f);
		if (racketDirection != Vector2.zero)
		{
			gameObject.transform.right = racketDirection.normalized;
		}
	}

	private void SendBall(float power)
	{
		hitBall = true;
		ball.SetVelocity(racketDirection * power);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == ("Ball") && !hitBall)
		{
			switch (myState)
			{
				case RacketStates.Smashing:
				{
					SendBall(smashPower);
					break;
				}
				case RacketStates.Hitting:
				{
					smashCurrentCharge += ball.GetSmashCharge();
					if (smashCurrentCharge > smashMaxCharge)
					{
						smashCurrentCharge = smashMaxCharge;
					}

					Debug.Log("ball hit, SmashCharge is : " + smashCurrentCharge);
					float hitPower = hitBasePower + hitPowerPerSecCharging * timeChargingHit;
					SendBall(hitPower);
					break;
				}
			}
		}
	}
}