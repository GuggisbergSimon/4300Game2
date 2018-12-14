﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
	[SerializeField] private GameObject engagePositionObject;
	[SerializeField] private playerNumber playerStarting = playerNumber.Player1;

	private Vector2 engagePosition;
	private Ball ball;

	private Dictionary<playerNumber, int> score = new Dictionary<playerNumber, int>()
	{
		{playerNumber.Player1, 0},
		{playerNumber.Player2, 0}
	};

	private void Start()
	{
		if (GameManager.Instance.InLevel)
		{
			engagePosition = engagePositionObject.transform.position;
			ball = GameManager.Instance.Ball.GetComponent<Ball>();
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
		score[player]++;
		Engage(player.GetOpponent());
	}
}