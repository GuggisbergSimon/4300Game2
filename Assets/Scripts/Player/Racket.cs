using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine;
using InControl;

public class Racket : MonoBehaviour
{
	[SerializeField] private bool isHitting = false;
	[SerializeField] private bool isChargingHit = false;
	[SerializeField] private bool hitBall = false;
	[SerializeField] private float timeChargingHit = 0.0f;
	[SerializeField] private float hitBasePower = 50.0f;
	[SerializeField] private float hitPowerPerSecCharging = 5.0f;
	[SerializeField] private float maximumChargingTime = 5.0f;
	[SerializeField] private float hitDuration = 0.2f;
	[SerializeField] private float timeHitting = 0.0f;
	[SerializeField] private float hitSpeed = 500.0f;
	[SerializeField] private float racketCounterRotation = -0.5f;

	[SerializeField] private Ball ball;
	[SerializeField] private PlayerMove player;

	[SerializeField] private float racketRotationSpeed = 50.0f;

	[SerializeField] private float smashMaxCharge = 100.0f;
	[SerializeField] private float smashCurrentCharge = 0.0f;
	[SerializeField] private float amountOfChargeToSmash = 25.0f;

	private Vector2 racketDirection;
	private bool isFacingRightAtStartup;

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
			//check if smash is possible and deplete gauge, TODO make the ball smash actually
			if (player.MyController.RightTrigger.WasPressed && smashCurrentCharge >= amountOfChargeToSmash)
			{
				smashCurrentCharge -= amountOfChargeToSmash;
			}

			//start of a hit
			if (player.MyController.RightBumper.WasPressed && !isHitting)
			{
				isChargingHit = true;
				//TODO keep that rotation while charging
				//gameObject.transform.Rotate(new Vector3(0.0f, 0.0f, 1.0f) * racketCounterRotation * hitSpeed *
				//                          hitDuration);
			}

			//charging a hit
			if (isChargingHit)
			{
				timeChargingHit += Time.deltaTime;
			}

			if (player.MyController.RightBumper.WasReleased)
			{
				Debug.Log("Hitting");
				isChargingHit = false;
				isHitting = true;
			}

			if (isHitting)
			{
				timeHitting += Time.deltaTime;

				//we rotate the racket depending on its orientation
				//NB : that's super ugly
				Vector3 orientation=Vector3.forward;
				if (!isFacingRightAtStartup)
				{
					orientation *= -1;
				}

				gameObject.transform.Rotate(orientation* hitSpeed * Time.deltaTime);

				if (timeHitting >= hitDuration)
				{
					Debug.Log("Hitting end");
					isHitting = false;
					hitBall = false;
					timeHitting = 0.0f;
					timeChargingHit = 0.0f;
				}
			}

			if (!isHitting)
			{
				racketDirection = player.MyController.RightStick;
				Debug.DrawLine(transform.position, transform.position + (Vector3) racketDirection * 4.0f);
				//Vector2 racketDirection = player.MyController.RightStick;

				if (racketDirection != Vector2.zero)
				{
					gameObject.transform.right = racketDirection.normalized;
				}
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == ("Ball") && !hitBall && isHitting)
		{
			Debug.Log("Ball hit");
			smashCurrentCharge += ball.getSmashCharge();
			hitBall = true;
			float hitPower = hitBasePower + hitPowerPerSecCharging * timeChargingHit;
			ball.SetVelocity(racketDirection * hitPower);
		}
	}
}