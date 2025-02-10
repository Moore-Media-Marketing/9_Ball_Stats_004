using System;

/// <summary>
/// Represents player statistics, including lifetime and current season data.
/// </summary>
public class PlayerStats
	{
	// --- Lifetime Stats --- //
	public int LifetimeGamesWon { get; set; }
	public int LifetimeGamesPlayed { get; set; }
	public int LifetimeMatchesWon { get; set; }
	public int LifetimeMatchesPlayed { get; set; }
	public int LifetimeBreakAndRun { get; set; }
	public int LifetimeNineOnTheSnap { get; set; }
	public int LifetimeMiniSlams { get; set; }
	public int LifetimeShutouts { get; set; }
	public float LifetimeDefensiveShotAverage { get; set; }

	// --- Current Season Stats --- //
	public int CurrentSeasonMatchesPlayed { get; set; }
	public int CurrentSeasonMatchesWon { get; set; }
	public int CurrentSeasonBreakAndRun { get; set; }
	public float CurrentSeasonDefensiveShotAverage { get; set; }
	public int CurrentSeasonMiniSlams { get; set; }
	public int CurrentSeasonNineOnTheSnap { get; set; }
	public int CurrentSeasonShutouts { get; set; }
	public float CurrentSeasonPaPercentage { get; set; }
	public int CurrentSeasonPointsAwarded { get; set; }
	public float CurrentSeasonPointsPerMatch { get; set; }
	public int CurrentSeasonPpm { get; set; }
	public int CurrentSeasonTotalPoints { get; set; }
	public int CurrentSeasonSkillLevel { get; set; }

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