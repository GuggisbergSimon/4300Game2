using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI scorePlayer1;
	[SerializeField] private TextMeshProUGUI scorePlayer2;

	private void Update()
	{
		//TODO remove that test
		/*if (Input.GetButtonDown("Fire1"))
		{
			UpdateScore(playerNumber.Player2);
			UpdateScore(playerNumber.Player1);
		}*/
	}

	public void UpdateScore(playerNumber player)
	{
		switch (player)
		{
			case playerNumber.Player1:
			{
				ReplaceScore(scorePlayer1, player);
				break;
			}
			case playerNumber.Player2:
			{
				ReplaceScore(scorePlayer2, player);
				break;
			}
		}
	}

	private void ReplaceScore(TextMeshProUGUI textToReplace, playerNumber player)
	{
		textToReplace.text = textToReplace.text.Remove(textToReplace.text.IndexOf(":") + 2);
		textToReplace.text =
			textToReplace.text.Insert(textToReplace.text.Length, GameManager.Instance.Score[player].ToString());
	}

	public void UpdateGauge(playerNumber player)
	{
		//TODO update Object in Canvas based on GameManager.player.gauge
	}
}