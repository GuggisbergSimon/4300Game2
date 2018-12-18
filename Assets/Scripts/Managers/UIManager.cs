using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI scorePlayer1;
	[SerializeField] private TextMeshProUGUI scorePlayer2;
	[SerializeField] private Image gaugeSmashPlayer1;
	[SerializeField] private Image gaugeSmashPlayer2;
	[SerializeField] private GameObject panelControllerMenu;

	public void UpdateUI()
	{
		foreach (var player in GameManager.Instance.Players)
		{
			UpdateScore(player.PlayerNumber);
			UpdateGauge(player.PlayerNumber);
		}
	}

	public void ToggleControllerMenu(bool value)
	{
		panelControllerMenu.SetActive(value);
	}

	private void UpdateScore(playerNumber player)
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

	private void UpdateGauge(playerNumber player)
	{
		Racket racket = GameManager.Instance.GetPlayer(player).MyRacket;
		float percent = racket.SmashCurrentCharge / racket.SmashMaxCharge;
		switch (player)
		{
			case playerNumber.Player1:
			{
				FillGauge(gaugeSmashPlayer1, percent);
				break;
			}
			case playerNumber.Player2:
			{
				FillGauge(gaugeSmashPlayer2, percent);
				break;
			}
		}
	}

	private void FillGauge(Image image, float percent)
	{
		image.fillAmount = percent;
	}
}