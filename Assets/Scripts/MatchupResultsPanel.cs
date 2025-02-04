public class MatchupResultData
	{
	// --- Properties --- //
	public float TeamAWins { get; set; }
	public float TeamBWins { get; set; }

	// --- Constructor --- //
	public MatchupResultData(float teamAWins, float teamBWins)
		{
		TeamAWins = teamAWins;
		TeamBWins = teamBWins;
		}
	}
