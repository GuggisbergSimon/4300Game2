using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
	[SerializeField] private float timeSetupNoFall = 1.5f;
	[SerializeField] private int setupBlinkTimes = 5;

	private playerNumber lastPlayerHitting = playerNumber.Player1;
	private Rigidbody2D myRigidbody2D;
	private SpriteRenderer mySpriteRenderer;

	public void SetVelocity(Vector2 velocity)
	{
		myRigidbody2D.velocity = velocity;
	}

	public IEnumerator Setup(Vector2 position)
	{
		float gravityScale = myRigidbody2D.gravityScale;
		float timer = 0.0f;

		SetVelocity(Vector2.zero);
		myRigidbody2D.gravityScale = 0.0f;
		transform.position = position;

		while (timer < timeSetupNoFall)
		{
			yield return new WaitForSeconds(timeSetupNoFall / (setupBlinkTimes * 2));
			mySpriteRenderer.color = Color.clear;
			yield return new WaitForSeconds(timeSetupNoFall / (setupBlinkTimes * 2));
			mySpriteRenderer.color = Color.white;
			timer += timeSetupNoFall / setupBlinkTimes;
		}

		myRigidbody2D.gravityScale = gravityScale;
	}

	private void Start()
	{
		myRigidbody2D = GetComponent<Rigidbody2D>();
		mySpriteRenderer = GetComponentInChildren<SpriteRenderer>();
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		GameObject collisionObject = other.gameObject;

		switch (collisionObject.tag)
		{
			case "Border":
			{
				GameManager.Instance.MyMatchManager.AddPointTo(lastPlayerHitting.GetOpponent());
				break;
			}
			case "Ground":
			{
				if ((lastPlayerHitting == playerNumber.Player1 && transform.position.x > 0) ||
				    (lastPlayerHitting == playerNumber.Player2 && transform.position.x < 0))
				{
					GameManager.Instance.MyMatchManager.AddPointTo(lastPlayerHitting);
				}
				else
				{
					GameManager.Instance.MyMatchManager.AddPointTo(lastPlayerHitting.GetOpponent());
				}

				break;
			}
			case "Net":
			{
				GameManager.Instance.MyMatchManager.AddPointTo(lastPlayerHitting.GetOpponent());
				break;
			}
			case "Player":
			{
				PlayerMove player = collisionObject.GetComponent<PlayerMove>();
				GameManager.Instance.MyMatchManager.AddPointTo(player.PlayerNumber.GetOpponent());
				break;
			}
			case "Racket":
			{
				PlayerMove player = collisionObject.GetComponentInParent<PlayerMove>();
				if (player.PlayerNumber != lastPlayerHitting)
				{
					lastPlayerHitting = player.PlayerNumber;
				}
				else
				{
					GameManager.Instance.MyMatchManager.AddPointTo(player.PlayerNumber.GetOpponent());
				}

				break;
			}
		}
	}
}