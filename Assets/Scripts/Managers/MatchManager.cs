using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
	//[SerializeField] private playerNumber playerStarting = playerNumber.Player1;

	private Dictionary<playerNumber, int> score = new Dictionary<playerNumber, int>()
	{
		{playerNumber.Player1, 0},
		{playerNumber.Player2, 0}
	};

	private void Engage(playerNumber player)
	{
		//moves the ball to somewhere and reset velocity/whatever
		//TODO code that part, actually no idea how we want to do it
	}

	public void AddPointTo(playerNumber player)
	{
		score[player]++;
		Engage(player.GetOpponent());
	}
}