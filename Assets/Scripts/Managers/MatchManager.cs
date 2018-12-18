using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
	[SerializeField] private GameObject engagePositionObject;
	[SerializeField] private playerNumber playerStarting = playerNumber.Player2;
    [SerializeField] private AudioClip pointSound;

    private Vector2 engagePosition;
	private Ball ball;
    private AudioSource myAudioSource;

    private void Start()
	{
		if (GameManager.Instance.InLevel)
		{
			engagePosition = engagePositionObject.transform.position;
			ball = GameManager.Instance.Ball.GetComponent<Ball>();
            myAudioSource = GetComponent<AudioSource>();
            Engage(playerStarting);
		}
	}

	public void Engage(playerNumber player)
	{
		if (player == playerNumber.Player2)
		{
			StartCoroutine(ball.Setup(engagePosition));
		}
		else
		{
			StartCoroutine(ball.Setup(engagePosition * (Vector2.left + Vector2.up)));
		}
	}

	public void AddPointTo(playerNumber player)
	{
        myAudioSource.PlayOneShot(pointSound);
		GameManager.Instance.Score[player]++;
		GameManager.Instance.UpdateUI();
		Engage(player.GetOpponent());
	}
}