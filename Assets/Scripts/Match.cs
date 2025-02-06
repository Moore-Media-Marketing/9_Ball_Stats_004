public class Match
	{
	public int MatchId { get; set; }
	public int Player1Id { get; set; }
	public int Player2Id { get; set; }
	public int Player1Score { get; set; }
	public int Player2Score { get; set; }
	public int WinnerId { get; set; }

	public Match(int matchId, int player1Id, int player2Id, int player1Score, int player2Score, int winnerId)
		{
		MatchId = matchId;
		Player1Id = player1Id;
		Player2Id = player2Id;
		Player1Score = player1Score;
		Player2Score = player2Score;
		WinnerId = winnerId;
		}
	}
