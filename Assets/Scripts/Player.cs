using System;

public class Player
	{
	// --- Player properties --- //
	public int PlayerId { get; set; }
	public int SkillLevel { get; set; }
	public string PlayerName { get; set; }
	public string TeamName { get; set; }
	public int LifetimeMatchesPlayed { get; set; }
	public int LifetimeMatchesWon { get; set; }
	public float CurrentSeasonPointsPerMatch { get; set; }
	public float LifetimeDefensiveShotAvg { get; set; }
	// Add any other properties you need for calculations

	// --- Constructor --- //
	public Player(int playerId, int skillLevel, string playerName, string teamName,
				  int lifetimeMatchesPlayed, int lifetimeMatchesWon,
				  float currentSeasonPointsPerMatch, float lifetimeDefensiveShotAvg)
		{
		PlayerId = playerId;
		SkillLevel = skillLevel;
		PlayerName = playerName;
		TeamName = teamName;
		LifetimeMatchesPlayed = lifetimeMatchesPlayed;
		LifetimeMatchesWon = lifetimeMatchesWon;
		CurrentSeasonPointsPerMatch = currentSeasonPointsPerMatch;
		LifetimeDefensiveShotAvg = lifetimeDefensiveShotAvg;
		// Initialize any other properties
		}

	// --- Calculate Overall Score --- //
	public float CalculateOverallScore(PlayerWeightSettings weightSettings)
		{
		// Example calculation using current season's points per match and lifetime stats
		float score = CurrentSeasonPointsPerMatch * weightSettings.PointsWeight;

		score += LifetimeDefensiveShotAvg * weightSettings.DefensiveShotWeight;
		// You can add more weighted stats here as needed

		return score;
		}

	// --- Other player-related methods --- //
	public string ToCsv()
		{
		return $"{PlayerId},{SkillLevel},{PlayerName},{TeamName},{LifetimeMatchesPlayed},{LifetimeMatchesWon},{CurrentSeasonPointsPerMatch},{LifetimeDefensiveShotAvg}";
		}

	public static Player FromCsv(string csvData)
		{
		var data = csvData.Split(',');
		return new Player(
			int.Parse(data[0]),
			int.Parse(data[1]),
			data[2],
			data[3],
			int.Parse(data[4]),
			int.Parse(data[5]),
			float.Parse(data[6]),
			float.Parse(data[7])
		);
		}
	}

public class PlayerWeightSettings
	{
	public float PointsWeight { get; set; }
	public float DefensiveShotWeight { get; set; }
	// Add other weights as needed
	}
