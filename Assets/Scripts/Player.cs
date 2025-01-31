using System;

using SQLite;

using UnityEngine;

[System.Serializable]
public class Player
	{
	#region Player Information

	[Header("Basic Info")]
	[Tooltip("Player's full name.")]
	public string Name;

	[Tooltip("Skill Level ranging from 1 (lowest) to 9 (highest).")]
	[Range(1, 9)]
	public int SkillLevel;

	#endregion Player Information

	#region Season Stats

	[Header("Season Stats")]
	[Tooltip("Total matches won this season.")]
	public int MatchesWon;

	[Tooltip("Total matches played this season.")]
	public int MatchesPlayed;

	[Tooltip("Average points scored per match.")]
	public int PointsPerMatch;

	[Tooltip("Total points awarded this season.")]
	public int PointsAwarded;

	#endregion Season Stats

	#region Lifetime Stats

	[Header("Lifetime Stats")]
	[Tooltip("Total matches won in lifetime.")]
	public int LifetimeMatchesWon;

	[Tooltip("Total matches played in lifetime.")]
	public int LifetimeMatchesPlayed;

	[Tooltip("Lifetime win percentage.")]
	public float LifetimeWinPercentage => LifetimeMatchesPlayed > 0
		? (float) LifetimeMatchesWon / LifetimeMatchesPlayed * 100f
		: 0f;

	[Tooltip("Defensive shot average.")]
	public float DefensiveShotAverage;

	[Tooltip("Matches played in the last 2 years.")]
	public int MatchesPlayedInLast2Years;

	[Tooltip("Number of 9-on-the-Snap shots.")]
	public int NineOnTheSnap;

	[Tooltip("Number of Mini Slams achieved.")]
	public int MiniSlams;

	[Tooltip("Number of Shutouts achieved.")]
	public int Shutouts;

	// Additional lifetime stats (if required)
	[Tooltip("Lifetime games won.")]
	public int LifetimeGamesWon;

	[Tooltip("Lifetime games played.")]
	public int LifetimeGamesPlayed;

	[Tooltip("Lifetime defensive shot average.")]
	public float LifetimeDefensiveShotAvg;

	[Tooltip("Lifetime break-and-run shots.")]
	public int LifetimeBreakAndRun;

	[Tooltip("Lifetime mini slams achieved.")]
	public int LifetimeMiniSlams;

	[Tooltip("Lifetime shutouts achieved.")]
	public int LifetimeShutouts;

	#endregion Lifetime Stats

	#region Database Fields

	// SQLite requires a parameterless constructor
	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }

	// Add TeamId to link Player to Team
	public int TeamId { get; set; }

	// Parameterless constructor for SQLite
	public Player() { }

	#endregion Database Fields

	#region Constructor

	// Constructor to initialize a new player
	public Player(string name, int skillLevel, int teamId)
		{
		Name = name;
		SkillLevel = Mathf.Clamp(skillLevel, 1, 9);
		TeamId = teamId; // Link player to team
		ResetSeasonStats();
		LifetimeMatchesWon = 0;
		LifetimeMatchesPlayed = 0;
		DefensiveShotAverage = 0;
		MatchesPlayedInLast2Years = 0;
		NineOnTheSnap = 0;
		MiniSlams = 0;
		Shutouts = 0;
		LifetimeGamesWon = 0;
		LifetimeGamesPlayed = 0;
		LifetimeDefensiveShotAvg = 0;
		LifetimeBreakAndRun = 0;
		LifetimeMiniSlams = 0;
		LifetimeShutouts = 0;
		}

	#endregion Constructor

	#region Methods

	// Updates the player's stats after a match
	public void UpdateStats(int pointsScored, bool wonMatch)
		{
		MatchesPlayed++;
		PointsAwarded += pointsScored;
		PointsPerMatch = PointsAwarded / MatchesPlayed;

		if (wonMatch)
			MatchesWon++;

		// Update lifetime stats
		LifetimeMatchesPlayed++;
		if (wonMatch)
			LifetimeMatchesWon++;
		}

	// Transfers player to another team
	public void TransferToNewTeam(int newTeamId)
		{
		// Validate if the team exists
		Team newTeam = DatabaseManager.Instance.GetTeamById(newTeamId);

		if (newTeam == null)
			{
			Debug.LogError($"Error: Team with ID {newTeamId} does not exist!");
			return;  // Exit if the team doesn't exist
			}

		// Proceed with transfer
		TeamId = newTeamId;
		Debug.Log($"{Name} has been moved to {newTeam.Name}.");
		}

	// Resets season stats at the start of a new season
	public void ResetSeasonStats()
		{
		MatchesWon = 0;
		MatchesPlayed = 0;
		PointsPerMatch = 0;
		PointsAwarded = 0;
		}

	#endregion Methods
	}
