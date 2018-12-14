using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
	private playerNumber lastPlayerHitting;
	private Rigidbody2D myRigidbody2D;

	public void SetVelocity(Vector2 velocity)
	{
		myRigidbody2D.velocity = velocity;
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

   public void SetVelocity(Vector3 newVelocity)
    {
        myRigidbody2D.velocity = newVelocity;
    }
}