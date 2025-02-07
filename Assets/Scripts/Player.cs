public class Player
	{
	public int TeamId { get; set; }
	public string TeamName { get; set; }
	public string PlayerName { get; set; }
	public int SkillLevel { get; set; }
	public int LifetimeMatchesPlayedInLast2Years { get; set; }
	public int LifetimeGamesPlayed { get; set; }
	public int LifetimeGamesWon { get; set; }
	public int LifetimeMiniSlams { get; set; }
	public int LifetimeNineOnTheSnap { get; set; }
	public int LifetimeShutouts { get; set; }
	public int LifetimeMatchesPlayed { get; set; }
	public int LifetimeMatchesWon { get; set; }
	public int CurrentSeasonMatchesPlayed { get; set; }
	public int CurrentSeasonMatchesWon { get; set; }
	public int CurrentSeasonPointsAwarded { get; set; }
	public float CurrentSeasonPointsPerMatch { get; set; }
	public int CurrentSeasonBreakAndRun { get; set; }
	public float CurrentSeasonDefensiveShotAverage { get; set; }
	public float CurrentSeasonPaPercentage { get; set; }
	public int CurrentSeasonShutouts { get; set; }
	public int CurrentSeasonTotalPoints { get; set; }

	public int LifetimeBreakAndRun { get; set; }
	public float LifetimeDefensiveShotAverage { get; set; }

	// Constructor for initializing all properties (21 parameters)
	public Player(
		int teamId,
		string teamName,
		string playerName,
		int skillLevel,
		int currentSeasonMatchesPlayed,
		int currentSeasonMatchesWon,
		int currentSeasonPointsAwarded,
		float currentSeasonPointsPerMatch,
		int currentSeasonBreakAndRun,
		float currentSeasonDefensiveShotAverage,
		int currentSeasonShutouts,
		int lifetimeGamesPlayed,
		int lifetimeGamesWon,
		int lifetimeMiniSlams,
		int lifetimeNineOnTheSnap,
		int lifetimeShutouts,
		int lifetimeMatchesPlayedInLast2Years,
		int lifetimeMatchesPlayed,
		int lifetimeMatchesWon,
		int lifetimeBreakAndRun,
		float lifetimeDefensiveShotAverage)
		{
		TeamId = teamId;
		TeamName = teamName;
		PlayerName = playerName;
		SkillLevel = skillLevel;
		CurrentSeasonMatchesPlayed = currentSeasonMatchesPlayed;
		CurrentSeasonMatchesWon = currentSeasonMatchesWon;
		CurrentSeasonPointsAwarded = currentSeasonPointsAwarded;
		CurrentSeasonPointsPerMatch = currentSeasonPointsPerMatch;
		CurrentSeasonBreakAndRun = currentSeasonBreakAndRun;
		CurrentSeasonDefensiveShotAverage = currentSeasonDefensiveShotAverage;
		CurrentSeasonShutouts = currentSeasonShutouts;
		LifetimeGamesPlayed = lifetimeGamesPlayed;
		LifetimeGamesWon = lifetimeGamesWon;
		LifetimeMiniSlams = lifetimeMiniSlams;
		LifetimeNineOnTheSnap = lifetimeNineOnTheSnap;
		LifetimeShutouts = lifetimeShutouts;
		LifetimeMatchesPlayedInLast2Years = lifetimeMatchesPlayedInLast2Years;
		LifetimeMatchesPlayed = lifetimeMatchesPlayed;
		LifetimeMatchesWon = lifetimeMatchesWon;
		LifetimeBreakAndRun = lifetimeBreakAndRun;
		LifetimeDefensiveShotAverage = lifetimeDefensiveShotAverage;
		}

	// --- New Method: Calculate Overall Score ---
	public float CalculateOverallScore(PlayerWeightSettings weightSettings)
		{
		// Weighted score calculation example.
		// (You can adjust the weights or formula as needed.)
		float score = (CurrentSeasonMatchesWon * 2) +
					  (CurrentSeasonBreakAndRun * 5) +
					  (LifetimeMiniSlams * 3) +
					  (LifetimeNineOnTheSnap * 4) +
					  (CurrentSeasonPaPercentage * 10) +
					  (LifetimeDefensiveShotAverage * 8);
		return score;
		}

	// Converts the player's statistics to CSV format for easy saving
	public string ToCsv()
		{
		return $"{TeamName},{TeamId},{PlayerName},{LifetimeGamesPlayed},{LifetimeGamesWon},{CurrentSeasonPointsAwarded},{LifetimeGamesPlayed}," +
			   $"{CurrentSeasonBreakAndRun},{CurrentSeasonDefensiveShotAverage},{CurrentSeasonMatchesPlayed},{CurrentSeasonMatchesWon},{LifetimeMiniSlams}," +
			   $"{LifetimeNineOnTheSnap},{CurrentSeasonPaPercentage},{CurrentSeasonPointsAwarded},{CurrentSeasonPointsPerMatch},{CurrentSeasonShutouts}," +
			   $"{SkillLevel},{CurrentSeasonTotalPoints},{LifetimeGamesPlayed},{LifetimeGamesWon},{LifetimeMatchesPlayed},{LifetimeMatchesWon}," +
			   $"{LifetimeMiniSlams},{LifetimeNineOnTheSnap},{LifetimeShutouts},{LifetimeMatchesPlayedInLast2Years},{LifetimeBreakAndRun},{LifetimeDefensiveShotAverage}";
		}
	}
