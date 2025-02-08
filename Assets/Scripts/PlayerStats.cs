using System;

// --- Player Stats Class --- //
public class PlayerStats
	{
	public int LifetimeGamesWon { get; set; }
	public int LifetimeGamesPlayed { get; set; }
	public int LifetimeBreakAndRun { get; set; }
	public float LifetimeDefensiveShotAverage { get; set; }
	public int LifetimeMatchesPlayed { get; set; }
	public int LifetimeMatchesWon { get; set; }
	public int LifetimeMiniSlams { get; set; }
	public int LifetimeNineOnTheSnap { get; set; }
	public int LifetimeShutouts { get; set; }

	public int CurrentSeasonBreakAndRun { get; set; }
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

	// Constructor to initialize all stats to zero
	public PlayerStats()
		{
		LifetimeGamesWon = 0;
		LifetimeGamesPlayed = 0;
		LifetimeBreakAndRun = 0;
		LifetimeDefensiveShotAverage = 0;
		LifetimeMatchesPlayed = 0;
		LifetimeMatchesWon = 0;
		LifetimeMiniSlams = 0;
		LifetimeNineOnTheSnap = 0;
		LifetimeShutouts = 0;

		CurrentSeasonBreakAndRun = 0;
		CurrentSeasonDefensiveShotAverage = 0;
		CurrentSeasonMatchesPlayed = 0;
		CurrentSeasonMatchesWon = 0;
		CurrentSeasonMiniSlams = 0;
		CurrentSeasonNineOnTheSnap = 0;
		CurrentSeasonPaPercentage = 0;
		CurrentSeasonPointsAwarded = 0;
		CurrentSeasonPointsPerMatch = 0;
		CurrentSeasonPpm = 0;
		CurrentSeasonShutouts = 0;
		CurrentSeasonSkillLevel = 0;
		CurrentSeasonTotalPoints = 0;
		}
	}
