using System;

[Serializable]
public class MatchupResultData
	{
	public string teamA;
	public string teamB;
	public float teamAWinProbability;
	public float teamBWinProbability;

	// --- Region: Constructor --- //
	public MatchupResultData(string teamA, string teamB, float teamAWinProbability, float teamBWinProbability)
		{
		this.teamA = teamA;
		this.teamB = teamB;
		this.teamAWinProbability = teamAWinProbability;
		this.teamBWinProbability = teamBWinProbability;
		}

	// --- Region: Default Constructor --- //
	public MatchupResultData()
		{ }
	}