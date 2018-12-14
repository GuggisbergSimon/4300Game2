using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
	[SerializeField] private float timeSetupNoFall = 1.5f;

	private playerNumber lastPlayerHitting=playerNumber.Player1;
	private Rigidbody2D myRigidbody2D;

	public void SetVelocity(Vector2 velocity)
	{
		myRigidbody2D.velocity = velocity;
	}

	public IEnumerator Setup(Vector2 position)
	{
		float gravityScale = myRigidbody2D.gravityScale;
		SetVelocity(Vector2.zero);
		myRigidbody2D.gravityScale = 0.0f;
		transform.position = position;
		yield return new WaitForSeconds(timeSetupNoFall);
		myRigidbody2D.gravityScale = gravityScale;
	}

	private void Start()
	{
		myRigidbody2D = GetComponent<Rigidbody2D>();
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
				lastPlayerHitting = player.PlayerNumber;
				break;
			}
		}
	}
}