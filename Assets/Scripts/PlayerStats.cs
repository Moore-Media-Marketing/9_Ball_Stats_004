public class PlayerStats
	{
	// Current Season Stats
	public int CurrentSeasonMatchesPlayed { get; set; }
	public int CurrentSeasonMatchesWon { get; set; }
	public int CurrentSeasonBreakAndRun { get; set; }
	public float CurrentSeasonDefensiveShotAverage { get; set; }
	public int CurrentSeasonMiniSlams { get; set; }
	public int CurrentSeasonNineOnTheSnap { get; set; }
	public float CurrentSeasonPaPercentage { get; set; }
	public int CurrentSeasonPointsAwarded { get; set; }
	public float CurrentSeasonPointsPerMatch { get; set; }
	public float CurrentSeasonPpm { get; set; }
	public int CurrentSeasonShutouts { get; set; }
	public int CurrentSeasonSkillLevel { get; set; }
	public int CurrentSeasonTotalPoints { get; set; }

	// Lifetime Stats
	public int LifetimeGamesPlayed { get; set; }
	public int LifetimeGamesWon { get; set; }
	public int LifetimeMatchesPlayed { get; set; }
	public int LifetimeMatchesWon { get; set; }
	public int LifetimeMiniSlams { get; set; }
	public int LifetimeNineOnTheSnap { get; set; }
	public int LifetimeShutouts { get; set; }
	public float LifetimeDefensiveShotAverage { get; set; }
	public int LifetimeBreakAndRun { get; set; }
	public int LifetimePointsAwarded { get; set; }
	public float LifetimePointsPerMatch { get; set; }
	public float LifetimePpm { get; set; }
	public int LifetimeTotalPoints { get; set; }
	public int LifetimeMatchesPlayedInLast2Years { get; set; }

	// Constructor to initialize default values
	public PlayerStats()
		{
		// Initialize all stats to default values
		CurrentSeasonMatchesPlayed = 0;
		CurrentSeasonMatchesWon = 0;
		CurrentSeasonBreakAndRun = 0;
		CurrentSeasonDefensiveShotAverage = 0;
		CurrentSeasonMiniSlams = 0;
		CurrentSeasonNineOnTheSnap = 0;
		CurrentSeasonPaPercentage = 0;
		CurrentSeasonPointsAwarded = 0;
		CurrentSeasonPointsPerMatch = 0;
		CurrentSeasonPpm = 0;
		CurrentSeasonShutouts = 0;
		CurrentSeasonSkillLevel = 0;
		CurrentSeasonTotalPoints = 0;

		LifetimeGamesPlayed = 0;
		LifetimeGamesWon = 0;
		LifetimeMatchesPlayed = 0;
		LifetimeMatchesWon = 0;
		LifetimeMiniSlams = 0;
		LifetimeNineOnTheSnap = 0;
		LifetimeShutouts = 0;
		LifetimeDefensiveShotAverage = 0;
		LifetimeBreakAndRun = 0;
		LifetimeMatchesPlayedInLast2Years = 0;
		}

	// Method to update current season stats after a match
	public void UpdateSeasonStats(bool wonMatch)
		{
		CurrentSeasonMatchesPlayed++;
		if (wonMatch)
			{
			CurrentSeasonMatchesWon++;
			}
		}

	// Method to update lifetime stats after a match
	public void UpdateLifetimeStats(bool wonMatch)
		{
		LifetimeGamesPlayed++;
		if (wonMatch)
			{
			LifetimeGamesWon++;
			}
		}
	}
