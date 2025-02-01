using System;
using System.Collections.Generic;

using UnityEngine;

[System.Serializable]
public class Player
	{
	#region Player Information

	// --- Basic Player Information --- //
	[Header("Basic Info")]
	[Tooltip("Player's full name.")]
	public string Name { get; set; }  // --- The player's name for identification --- //

	[Tooltip("Skill Level ranging from 1 (lowest) to 9 (highest).")]
	[Range(1, 9)]
	public int SkillLevel { get; set; }  // --- The player's skill level (1-9) --- //

	#endregion Player Information

	#region Season Stats

	// --- Current Season Stats --- //
	[Header("Season Stats")]
	[Tooltip("Total matches won this season.")]
	public int CurrentSeasonMatchesWon { get; set; }  // --- Matches won this season --- //

	[Tooltip("Total matches played this season.")]
	public int CurrentSeasonMatchesPlayed { get; set; }  // --- Matches played this season --- //

	[Tooltip("Average points scored per match this season.")]
	public int CurrentSeasonPointsPerMatch { get; set; }  // --- Average points per match for this season --- //

	[Tooltip("Total points awarded this season.")]
	public int CurrentSeasonPointsAwarded { get; set; }  // --- Total points awarded this season --- //

	[Tooltip("Defensive shot average this season.")]
	public float CurrentSeasonDefensiveShotAverage { get; set; }  // --- Defensive shot average for this season --- //

	[Tooltip("Number of 9-on-the-Snap shots this season.")]
	public int CurrentSeasonNineOnTheSnap { get; set; }  // --- Count of 9-on-the-Snap shots this season --- //

	[Tooltip("Number of Mini Slams achieved this season.")]
	public int CurrentSeasonMiniSlams { get; set; }  // --- Count of Mini Slams achieved this season --- //

	[Tooltip("Number of Shutouts achieved this season.")]
	public int CurrentSeasonShutouts { get; set; }  // --- Count of Shutouts this season --- //

	// --- Additional Current Season Stats --- //
	[Tooltip("Total points accumulated this season.")]
	public int CurrentSeasonTotalPoints { get; set; }  // --- Total points accumulated this season --- //

	[Tooltip("Points per match (PPM) this season.")]
	public float CurrentSeasonPpm { get; set; }  // --- Points per match for this season --- //

	[Tooltip("Percentage of points awarded (PA percentage) this season.")]
	public float CurrentSeasonPaPercentage { get; set; }  // --- Points awarded percentage or accuracy metric --- //

	[Tooltip("Break and run statistic for this season.")]
	public int CurrentSeasonBreakAndRun { get; set; }  // --- Break and run stat for this season --- //

	[Tooltip("Skill level for the current season (e.g., Beginner, Intermediate).")]
	public int CurrentSeasonSkillLevel { get; set; }  // --- Descriptive skill level for this season --- //

	#endregion Season Stats

	#region Lifetime Stats

	// --- Lifetime Player Stats --- //
	[Header("Lifetime Stats")]
	[Tooltip("Total matches won in lifetime.")]
	public int LifetimeMatchesWon { get; set; }  // --- Total lifetime matches won --- //

	[Tooltip("Total matches played in lifetime.")]
	public int LifetimeMatchesPlayed { get; set; }  // --- Total lifetime matches played --- //

	[Tooltip("Lifetime win percentage.")]
	public float LifetimeWinPercentage => LifetimeMatchesPlayed > 0
		? (float) LifetimeMatchesWon / LifetimeMatchesPlayed * 100f
		: 0f;  // --- Calculation of lifetime win percentage --- //

	[Tooltip("Defensive shot average in lifetime.")]
	public float LifetimeDefensiveShotAverage { get; set; }  // --- Lifetime defensive shot average --- //

	[Tooltip("Matches played in the last 2 years.")]
	public int MatchesPlayedInLast2Years { get; set; }  // --- Matches played in the last 2 years --- //

	[Tooltip("Number of 9-on-the-Snap shots in lifetime.")]
	public int LifetimeNineOnTheSnap { get; set; }  // --- Lifetime count of 9-on-the-Snap shots --- //

	[Tooltip("Number of Mini Slams achieved in lifetime.")]
	public int LifetimeMiniSlams { get; set; }  // --- Lifetime count of Mini Slams achieved --- //

	[Tooltip("Number of Shutouts achieved in lifetime.")]
	public int LifetimeShutouts { get; set; }  // --- Lifetime count of Shutouts achieved --- //

	// --- Additional Lifetime Stats --- //
	[Tooltip("Lifetime games won.")]
	public int LifetimeGamesWon { get; set; }  // --- Total lifetime games won --- //

	[Tooltip("Lifetime games played.")]
	public int LifetimeGamesPlayed { get; set; }  // --- Total lifetime games played --- //

	[Tooltip("Lifetime defensive shot average (alternative).")]
	public float LifetimeDefensiveShotAvg { get; set; }  // --- Alternative lifetime defensive shot average --- //

	[Tooltip("Lifetime break and run stats.")]
	public int LifetimeBreakAndRun { get; set; }  // --- Lifetime break and run statistic --- //

	#endregion Lifetime Stats

	#region Database Fields

	// --- PlayerPrefs Fields (used for PlayerPrefs storage) --- //
	public int TeamId { get; set; }  // --- ID of the team the player belongs to --- //

	public int Id { get; set; }  // --- Unique identifier for the player --- //

	#endregion Database Fields

	#region Constructor

	// --- Constructor to initialize a new player with default stats --- //
	public Player(string name, int skillLevel, int teamId)
		{
		Name = name.Trim();  // --- Remove extraneous whitespace from the name --- //
		SkillLevel = Mathf.Clamp(skillLevel, 1, 9);  // --- Clamp the skill level between 1 and 9 --- //
		TeamId = teamId;  // --- Set the player's team ID --- //
		ResetSeasonStats();  // --- Initialize current season stats to default values --- //
		ResetLifetimeStats();  // --- Initialize lifetime stats to default values --- //
		Debug.Log($"[Player] Created: {Name}, Skill: {SkillLevel}, Team ID: {TeamId}");  // --- Log player creation --- //
		}

	#endregion Constructor

	#region Methods

	// --- Method to update player stats for a match --- //
	public void UpdateStats(int pointsScored, bool wonMatch, float defensiveShotAvg = 0f)
		{
		// --- Validate points scored (must not be negative) --- //
		if (pointsScored < 0)
			{
			Debug.LogWarning($"Invalid points scored for {Name}: {pointsScored}");  // --- Log warning --- //
			return;  // --- Exit if invalid --- //
			}

		// --- Update current season stats --- //
		CurrentSeasonMatchesPlayed++;  // --- Increment matches played in current season --- //
		CurrentSeasonPointsAwarded += pointsScored;  // --- Add points scored to current season total --- //
		CurrentSeasonPointsPerMatch = CurrentSeasonMatchesPlayed > 0
			? CurrentSeasonPointsAwarded / CurrentSeasonMatchesPlayed
			: 0;  // --- Calculate current season average points per match --- //
		if (wonMatch)
			CurrentSeasonMatchesWon++;  // --- Increment matches won in current season if match won --- //

		// --- Update lifetime stats --- //
		LifetimeMatchesPlayed++;  // --- Increment lifetime matches played --- //
		if (wonMatch)
			LifetimeMatchesWon++;  // --- Increment lifetime matches won if match won --- //

		// --- Validate defensive shot average (must not be negative) --- //
		if (defensiveShotAvg < 0)
			{
			Debug.LogWarning($"Invalid defensive shot average for {Name}: {defensiveShotAvg}");
			return;
			}

		// --- Update lifetime defensive shot average (simple moving average) --- //
		LifetimeDefensiveShotAverage = (LifetimeDefensiveShotAverage * (LifetimeMatchesPlayed - 1) + defensiveShotAvg) / LifetimeMatchesPlayed;
		Debug.Log($"[Player] Updated Stats: {Name} | Season Matches: {CurrentSeasonMatchesPlayed} | Season Wins: {CurrentSeasonMatchesWon}");
		}

	// --- Method to reset current season stats to default values --- //
	public void ResetSeasonStats()
		{
		CurrentSeasonMatchesWon = 0;
		CurrentSeasonMatchesPlayed = 0;
		CurrentSeasonPointsPerMatch = 0;
		CurrentSeasonPointsAwarded = 0;
		CurrentSeasonDefensiveShotAverage = 0f;
		CurrentSeasonNineOnTheSnap = 0;
		CurrentSeasonMiniSlams = 0;
		CurrentSeasonShutouts = 0;
		CurrentSeasonTotalPoints = 0;
		CurrentSeasonPpm = 0f;
		CurrentSeasonPaPercentage = 0f;
		CurrentSeasonBreakAndRun = 0;
		CurrentSeasonSkillLevel = 1;
		Debug.Log($"[Player] Season stats reset for {Name}");
		}

	// --- Method to reset lifetime stats to default values --- //
	public void ResetLifetimeStats()
		{
		LifetimeMatchesWon = 0;
		LifetimeMatchesPlayed = 0;
		LifetimeDefensiveShotAverage = 0f;
		LifetimeNineOnTheSnap = 0;
		LifetimeMiniSlams = 0;
		LifetimeShutouts = 0;
		LifetimeGamesWon = 0;
		LifetimeGamesPlayed = 0;
		LifetimeDefensiveShotAvg = 0f;
		LifetimeBreakAndRun = 0;
		Debug.Log($"[Player] Lifetime stats reset for {Name}");
		}

	#endregion Methods

	#region Static Methods

	// --- Method to validate player name case-insensitively and avoid duplicates --- //
	public static bool IsValidPlayerName(string name, List<Player> players)
		{
		// Check if a player with the same name exists (case-insensitive)
		foreach (var player in players)
			{
			if (string.Equals(player.Name, name, StringComparison.OrdinalIgnoreCase))
				{
				Debug.LogWarning($"Player name '{name}' is already taken!");
				return false;  // --- Return false if the name already exists --- //
				}
			}
		return true;  // --- Return true if the name is valid --- //
		}

	#endregion Static Methods
	}
