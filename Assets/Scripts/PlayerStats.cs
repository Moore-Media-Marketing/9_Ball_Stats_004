/// <summary>
/// Represents player statistics, including lifetime and current season data.
/// </summary>
public class PlayerStats
	{
	// --- Lifetime Stats --- //
	public int LifetimeGamesWon { get; set; } // Total games won in lifetime
	public int LifetimeGamesPlayed { get; set; } // Total games played in lifetime
	public int LifetimeMatchesWon { get; set; } // Total matches won in lifetime
	public int LifetimeMatchesPlayed { get; set; } // Total matches played in lifetime
	public int LifetimeBreakAndRun { get; set; } // Lifetime Break and Run occurrences
	public int LifetimeNineOnTheSnap { get; set; } // Lifetime Nine on the Snap occurrences
	public int LifetimeMiniSlams { get; set; } // Lifetime Mini Slams occurrences
	public int LifetimeShutouts { get; set; } // Lifetime Shutouts
	public float LifetimeDefensiveShotAverage { get; set; } // Defensive Shot Average in lifetime

	// --- Current Season Stats --- //
	public int CurrentSeasonMatchesPlayed { get; set; } // Matches played in current season
	public int CurrentSeasonMatchesWon { get; set; } // Matches won in current season
	public int CurrentSeasonBreakAndRun { get; set; } // Current season Break and Run
	public float CurrentSeasonDefensiveShotAverage { get; set; } // Current season Defensive Shot Average
	public int CurrentSeasonMiniSlams { get; set; } // Current season Mini Slams
	public int CurrentSeasonNineOnTheSnap { get; set; } // Current season Nine on the Snap
	public int CurrentSeasonShutouts { get; set; } // Current season Shutouts
	public float CurrentSeasonPaPercentage { get; set; } // PA Percentage (0.0 - 1.0)
	public int CurrentSeasonPointsAwarded { get; set; } // Points awarded in current season
	public float CurrentSeasonPointsPerMatch { get; set; } // Points per match in current season
	public int CurrentSeasonPpm { get; set; } // Points per match in current season
	public int CurrentSeasonTotalPoints { get; set; } // Total points in current season
	public int CurrentSeasonSkillLevel { get; set; } // Skill level in current season

	/// <summary>
	/// Initializes a new instance of PlayerStats with default values.
	/// </summary>
	public PlayerStats()
		{
		LifetimeGamesWon = 0;
		LifetimeGamesPlayed = 0;
		LifetimeMatchesWon = 0;
		LifetimeMatchesPlayed = 0;
		LifetimeBreakAndRun = 0;
		LifetimeNineOnTheSnap = 0;
		LifetimeMiniSlams = 0;
		LifetimeShutouts = 0;
		LifetimeDefensiveShotAverage = 0f;

		CurrentSeasonMatchesPlayed = 0;
		CurrentSeasonMatchesWon = 0;
		CurrentSeasonBreakAndRun = 0;
		CurrentSeasonDefensiveShotAverage = 0f;
		CurrentSeasonMiniSlams = 0;
		CurrentSeasonNineOnTheSnap = 0;
		CurrentSeasonShutouts = 0;
		CurrentSeasonPaPercentage = 0f;
		CurrentSeasonPointsAwarded = 0;
		CurrentSeasonPointsPerMatch = 0f;
		CurrentSeasonPpm = 0;
		CurrentSeasonTotalPoints = 0;
		CurrentSeasonSkillLevel = 1;
		}

	/// <summary>
	/// Validates that all stats are within logical limits.
	/// </summary>
	public bool IsValid()
		{
		return LifetimeGamesPlayed >= 0 && LifetimeGamesWon >= 0 &&
			   LifetimeMatchesPlayed >= 0 && LifetimeMatchesWon >= 0 &&
			   LifetimeGamesWon <= LifetimeGamesPlayed &&
			   LifetimeMatchesWon <= LifetimeMatchesPlayed &&
			   CurrentSeasonMatchesPlayed >= 0 && CurrentSeasonMatchesWon >= 0 &&
			   CurrentSeasonMatchesWon <= CurrentSeasonMatchesPlayed &&
			   CurrentSeasonDefensiveShotAverage >= 0f && CurrentSeasonDefensiveShotAverage <= 1f &&
			   CurrentSeasonPaPercentage >= 0f && CurrentSeasonPaPercentage <= 1f;
		}
	}
