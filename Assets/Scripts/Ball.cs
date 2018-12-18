using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
	[SerializeField] private float timeSetupNoFall = 1.5f;
	[SerializeField] private int setupBlinkTimes = 5;
	[SerializeField] private float smashChargePerSpeed = 2.0f;
    [SerializeField] private Color smashTrailColor = Color.red;
    [SerializeField] private Color hitTrailColor = Color.yellow;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip smashSound;
    [SerializeField] private AudioClip hitPlayerSound;

    private playerNumber lastPlayerHitting = playerNumber.Player1;

	public playerNumber LastPlayerHitting
	{
		get { return lastPlayerHitting; }
		set { lastPlayerHitting = value; }
	}

	private Rigidbody2D myRigidbody2D;
	private SpriteRenderer mySpriteRenderer;
	private Collider2D myCollider2D;
    private TrailRenderer myTrailRenderer;
    private AudioSource myAudioSource;

    public void SetVelocity(Vector2 velocity)
	{
		myRigidbody2D.velocity = velocity;
	}

	public void SetGravityScale(float gravityScale)
	{
		myRigidbody2D.gravityScale = gravityScale;
	}

	public void ResetPhysics()
	{
		myRigidbody2D.gravityScale = 1.0f;
		myCollider2D.enabled = true;
	}

	public IEnumerator Setup(Vector2 position)
	{
		float gravityScale = myRigidbody2D.gravityScale;
		float timer = 0.0f;

		SetVelocity(Vector2.zero);
		myRigidbody2D.gravityScale = 0.0f;
		transform.position = position;
		myCollider2D.enabled = false;

		while (timer < timeSetupNoFall)
		{
			yield return new WaitForSeconds(timeSetupNoFall / (setupBlinkTimes * 2));
			mySpriteRenderer.color = Color.clear;
			yield return new WaitForSeconds(timeSetupNoFall / (setupBlinkTimes * 2));
			mySpriteRenderer.color = Color.white;
			timer += timeSetupNoFall / setupBlinkTimes;
		}

		myCollider2D.enabled = true;
		myRigidbody2D.gravityScale = gravityScale;
	}

	private void Awake()
	{
		myRigidbody2D = GetComponent<Rigidbody2D>();
		mySpriteRenderer = GetComponentInChildren<SpriteRenderer>();
		myCollider2D = GetComponent<Collider2D>();
        myTrailRenderer = GetComponent<TrailRenderer>();
        myAudioSource = GetComponent<AudioSource>();
    }

	private void OnCollisionEnter2D(Collision2D other)
	{
		GameObject collisionObject = other.gameObject;
        if (myTrailRenderer.enabled)
        {
            myTrailRenderer.enabled = false;
        }
        switch (collisionObject.tag)
		{
			case "Border":
			{
				GameManager.Instance.MyMatchManager.AddPointTo(lastPlayerHitting.GetOpponent());
				lastPlayerHitting = lastPlayerHitting.GetOpponent();
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
					lastPlayerHitting = lastPlayerHitting.GetOpponent();
				}

				break;
			}
			case "Net":
			{
				GameManager.Instance.MyMatchManager.AddPointTo(lastPlayerHitting.GetOpponent());
				lastPlayerHitting = lastPlayerHitting.GetOpponent();
				break;
			}
			case "Player":
			{
                myAudioSource.PlayOneShot(hitPlayerSound);
				PlayerMove player = collisionObject.GetComponent<PlayerMove>();
				GameManager.Instance.MyMatchManager.AddPointTo(player.PlayerNumber.GetOpponent());
				lastPlayerHitting = player.PlayerNumber.GetOpponent();
				break;
			}
		}
	}

	public void SetVelocity(Vector3 newVelocity)
	{
		myRigidbody2D.velocity = newVelocity;
	}

	public float GetSmashCharge()
	{
		return smashChargePerSpeed * myRigidbody2D.velocity.magnitude;
	}

    public void SetTrailActive(bool trailActive, bool smashed)
    {
        if(smashed)
        {
            myAudioSource.PlayOneShot(smashSound);
            myTrailRenderer.startColor = smashTrailColor;
        }
        else
        {
            myAudioSource.PlayOneShot(hitSound);
            myTrailRenderer.startColor = hitTrailColor;
        }
        myTrailRenderer.enabled = trailActive;
    }
}