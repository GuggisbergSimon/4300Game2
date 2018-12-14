using System.Diagnostics;

public enum playerNumber
{
	Player1,
	Player2
}

static class playerNumberMethods
{
	public static playerNumber GetOpponent(this playerNumber player)
	{
		switch (player)
		{
			case playerNumber.Player1:
				return playerNumber.Player2;
			case playerNumber.Player2:
				return playerNumber.Player1;
			default:
				return playerNumber.Player1;
		}
	}
}