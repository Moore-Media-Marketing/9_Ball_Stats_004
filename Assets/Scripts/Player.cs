using System;

public class Player
	{
	public int PlayerId { get; set; }
	public string PlayerName { get; set; }
	public int SkillLevel { get; set; }
	public int TeamId { get; set; }
	public string TeamName { get; set; }
	public int LifetimeMatchesPlayedInLast2Years { get; set; }

	// Current Season Statistics
	public float CurrentSeasonDefensiveShotAverage { get; set; }
	public int CurrentSeasonMatchesPlayed { get; set; }
	public int CurrentSeasonMatchesWon { get; set; }
	public int CurrentSeasonMiniSlams { get; set; }
	public int CurrentSeasonNineOnTheSnap { get; set; }
	public float CurrentSeasonPaPercentage { get; set; }
	public int CurrentSeasonPointsAwarded { get; set; }
	public float CurrentSeasonPointsPerMatch { get; set; }
	public float CurrentSeasonPpm { get; set; }
	public int CurrentSeasonShutouts { get; set; }
	public int CurrentSeasonSkillLevel { get; set; }
	public int CurrentSeasonTotalPoints { get; set; }

	// Lifetime Statistics
	public int LifetimeBreakAndRun { get; set; }
	public float LifetimeDefensiveShotAverage { get; set; }
	public int LifetimeGamesPlayed { get; set; }
	public int LifetimeGamesWon { get; set; }
	public int LifetimeMatchesPlayed { get; set; }
	public int LifetimeMatchesWon { get; set; }
	public int LifetimeMiniSlams { get; set; }
	public int LifetimeNineOnTheSnap { get; set; }
	public int LifetimeShutouts { get; set; }

	// Constructor
	public Player(int playerId, string playerName, int skillLevel, int teamId, string teamName, int lifetimeMatchesPlayedInLast2Years,
				  float currentSeasonDefensiveShotAverage, int currentSeasonMatchesPlayed, int currentSeasonMatchesWon, int currentSeasonMiniSlams,
				  int currentSeasonNineOnTheSnap, float currentSeasonPaPercentage, int currentSeasonPointsAwarded, float currentSeasonPointsPerMatch,
				  float currentSeasonPpm, int currentSeasonShutouts, int currentSeasonSkillLevel, int currentSeasonTotalPoints,
				  int lifetimeBreakAndRun, float lifetimeDefensiveShotAverage, int lifetimeGamesPlayed, int lifetimeGamesWon,
				  int lifetimeMatchesPlayed, int lifetimeMatchesWon, int lifetimeMiniSlams, int lifetimeNineOnTheSnap, int lifetimeShutouts)
		{
		PlayerId = playerId;
		PlayerName = playerName;
		SkillLevel = skillLevel;
		TeamId = teamId;
		TeamName = teamName;
		LifetimeMatchesPlayedInLast2Years = lifetimeMatchesPlayedInLast2Years;
		CurrentSeasonDefensiveShotAverage = currentSeasonDefensiveShotAverage;
		CurrentSeasonMatchesPlayed = currentSeasonMatchesPlayed;
		CurrentSeasonMatchesWon = currentSeasonMatchesWon;
		CurrentSeasonMiniSlams = currentSeasonMiniSlams;
		CurrentSeasonNineOnTheSnap = currentSeasonNineOnTheSnap;
		CurrentSeasonPaPercentage = currentSeasonPaPercentage;
		CurrentSeasonPointsAwarded = currentSeasonPointsAwarded;
		CurrentSeasonPointsPerMatch = currentSeasonPointsPerMatch;
		CurrentSeasonPpm = currentSeasonPpm;
		CurrentSeasonShutouts = currentSeasonShutouts;
		CurrentSeasonSkillLevel = currentSeasonSkillLevel;
		CurrentSeasonTotalPoints = currentSeasonTotalPoints;
		LifetimeBreakAndRun = lifetimeBreakAndRun;
		LifetimeDefensiveShotAverage = lifetimeDefensiveShotAverage;
		LifetimeGamesPlayed = lifetimeGamesPlayed;
		LifetimeGamesWon = lifetimeGamesWon;
		LifetimeMatchesPlayed = lifetimeMatchesPlayed;
		LifetimeMatchesWon = lifetimeMatchesWon;
		LifetimeMiniSlams = lifetimeMiniSlams;
		LifetimeNineOnTheSnap = lifetimeNineOnTheSnap;
		LifetimeShutouts = lifetimeShutouts;
		}

	// Method to export to CSV (You may need to update based on your specific needs)
	public string ToCsv()
		{
		return $"{PlayerId},{PlayerName},{SkillLevel},{TeamId},{TeamName},{LifetimeMatchesPlayedInLast2Years}," +
			   $"{CurrentSeasonDefensiveShotAverage},{CurrentSeasonMatchesPlayed},{CurrentSeasonMatchesWon},{CurrentSeasonMiniSlams}," +
			   $"{CurrentSeasonNineOnTheSnap},{CurrentSeasonPaPercentage},{CurrentSeasonPointsAwarded},{CurrentSeasonPointsPerMatch}," +
			   $"{CurrentSeasonPpm},{CurrentSeasonShutouts},{CurrentSeasonSkillLevel},{CurrentSeasonTotalPoints}," +
			   $"{LifetimeBreakAndRun},{LifetimeDefensiveShotAverage},{LifetimeGamesPlayed},{LifetimeGamesWon},{LifetimeMatchesPlayed}," +
			   $"{LifetimeMatchesWon},{LifetimeMiniSlams},{LifetimeNineOnTheSnap},{LifetimeShutouts}";
		}

	// Method to import from CSV (You may need to update based on your specific needs)
	public static Player FromCsv(string csv)
		{
		string[] values = csv.Split(',');
		return new Player(
			int.Parse(values[0]),
			values[1],
			int.Parse(values[2]),
			int.Parse(values[3]),
			values[4],
			int.Parse(values[5]),
			float.Parse(values[6]),
			int.Parse(values[7]),
			int.Parse(values[8]),
			int.Parse(values[9]),
			int.Parse(values[10]),
			float.Parse(values[11]),
			int.Parse(values[12]),
			float.Parse(values[13]),
			float.Parse(values[14]),
			int.Parse(values[15]),
			int.Parse(values[16]),
			int.Parse(values[17]),
			int.Parse(values[18]),
			int.Parse(values[19]),
			float.Parse(values[20]),
			int.Parse(values[21]),
			int.Parse(values[22]),
			int.Parse(values[23]),
			int.Parse(values[24]),
			int.Parse(values[25]),
			int.Parse(values[26]),
			int.Parse(values[27])
		);
		}
	}
