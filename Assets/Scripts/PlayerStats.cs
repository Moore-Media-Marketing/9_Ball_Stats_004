public class PlayerStats
	{
	// --- Lifetime Stats --- //
	public int LifetimeGamesWon { get; set; }  // Lifetime Games Won
	public int LifetimeGamesPlayed { get; set; } // Lifetime Games Played
	public float LifetimeDefensiveShotAverage { get; set; } // Lifetime Defensive Shot Average
	public int LifetimeMatchesPlayed { get; set; } // Lifetime Matches Played
	public int LifetimeBreakAndRun { get; set; } // Lifetime Break and Run occurrences
	public int LifetimeNineOnTheSnap { get; set; } // Lifetime Nine on the Snap occurrences
	public int LifetimeMiniSlams { get; set; } // Lifetime Mini Slams occurrences
	public int LifetimeShutouts { get; set; } // Lifetime Shutouts

	// --- Current Season Stats --- //
	public int CurrentSeasonMatchesPlayed { get; set; } // Matches played in current season
	public int CurrentSeasonMatchesWon { get; set; } // Matches won in current season
	public int CurrentSeasonBreakAndRun { get; set; } // Current season Break and Run
	public float CurrentSeasonDefensiveShotAverage { get; set; } // Current season Defensive Shot Average
	public int CurrentSeasonMiniSlams { get; set; } // Current season Mini Slams
	public int CurrentSeasonNineOnTheSnap { get; set; } // Current season Nine on the Snap
	public int CurrentSeasonShutouts { get; set; } // Current season Shutouts
	public float CurrentSeasonPaPercentage { get; set; } // Current season PA Percentage
	public int CurrentSeasonPointsAwarded { get; set; } // Points awarded in current season
	public float CurrentSeasonPointsPerMatch { get; set; } // Points per match in current season
	public int CurrentSeasonPpm { get; set; } // Points per match in current season
	public int CurrentSeasonTotalPoints { get; set; } // Total points in current season
	public int CurrentSeasonSkillLevel { get; set; } // Skill level in current season
	public int LifetimeMatchesWon { get; internal set; }

	// --- Method to Check Validity --- //
	public bool IsValid()
		{
		// Basic validation for stats
		return LifetimeGamesPlayed >= 0 && LifetimeGamesWon >= 0 &&
			   CurrentSeasonMatchesPlayed >= 0 && CurrentSeasonMatchesWon >= 0;
		}
	}
