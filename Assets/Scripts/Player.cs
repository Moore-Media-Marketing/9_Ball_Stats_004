using System;
using UnityEngine;
using SQLite;

// --- Player Class --- //
[Serializable]
public class Player
	{
	#region Player Information
	[Header("Basic Info")]
	[Tooltip("Player's full name.")]
	public string Name;

	[Tooltip("Skill Level ranging from 1 (lowest) to 9 (highest).")]
	[Range(1, 9)]
	public int SkillLevel;

	#endregion

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

	#endregion

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

	// Additional lifetime stats
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

	#endregion

	#region Current Season Data
	[Header("Current Season Data")]
	[Tooltip("Matches won in the current season.")]
	public int CurrentSeasonMatchesWon;

	[Tooltip("Matches played in the current season.")]
	public int CurrentSeasonMatchesPlayed;

	[Tooltip("Points scored per match in the current season.")]
	public int CurrentSeasonPointsPerMatch;

	[Tooltip("Total points awarded in the current season.")]
	public int CurrentSeasonPointsAwarded;

	[Tooltip("Defensive shot average in the current season.")]
	public float CurrentSeasonDefensiveShotAverage;

	[Tooltip("Number of 9-on-the-Snap shots in the current season.")]
	public int CurrentSeasonNineOnTheSnap;

	[Tooltip("Number of Mini Slams achieved in the current season.")]
	public int CurrentSeasonMiniSlams;

	[Tooltip("Number of Shutouts in the current season.")]
	public int CurrentSeasonShutouts;

	#endregion

	#region Database Fields
	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }

	[Tooltip("Foreign key linking the player to a team.")]
	public int TeamId { get; set; }

	// Required for SQLite
	public Player() { }

	#endregion

	#region Constructor

	public Player(string name, int skillLevel, int teamId)
		{
		Name = name.Trim();
		SkillLevel = Mathf.Clamp(skillLevel, 1, 9);
		TeamId = teamId;

		ResetSeasonStats();
		ResetLifetimeStats();

		Debug.Log($"[Player] Created: {Name}, Skill: {SkillLevel}, Team ID: {TeamId}");
		}

	#endregion

	#region Methods

	// --- Updates the player's stats after a match --- //
	public void UpdateStats(int pointsScored, bool wonMatch)
		{
		MatchesPlayed++;
		PointsAwarded += pointsScored;
		PointsPerMatch = MatchesPlayed > 0 ? PointsAwarded / MatchesPlayed : 0;

		if (wonMatch)
			MatchesWon++;

		// Update lifetime stats
		LifetimeMatchesPlayed++;
		if (wonMatch)
			LifetimeMatchesWon++;

		// Update additional lifetime stats like points average
		LifetimeGamesPlayed++;
		LifetimeGamesWon += wonMatch ? 1 : 0;

		Debug.Log($"[Player] Updated Stats: {Name} | Matches: {MatchesPlayed} | Wins: {MatchesWon}");
		}

	// --- Updates the player's current season stats --- //
	public void UpdateCurrentSeasonStats(int matchesWon, int matchesPlayed, int pointsPerMatch, int pointsAwarded,
										  float defensiveShotAverage, int nineOnTheSnap, int miniSlams, int shutouts)
		{
		CurrentSeasonMatchesWon = matchesWon;
		CurrentSeasonMatchesPlayed = matchesPlayed;
		CurrentSeasonPointsPerMatch = pointsPerMatch;
		CurrentSeasonPointsAwarded = pointsAwarded;
		CurrentSeasonDefensiveShotAverage = defensiveShotAverage;
		CurrentSeasonNineOnTheSnap = nineOnTheSnap;
		CurrentSeasonMiniSlams = miniSlams;
		CurrentSeasonShutouts = shutouts;

		Debug.Log($"[Player] Current season data updated for {Name}");
		}

	// --- Transfers player to another team --- //
	public void TransferToNewTeam(int newTeamId)
		{
		Team newTeam = DatabaseManager.Instance.GetTeamById(newTeamId);

		if (newTeam == null)
			{
			Debug.LogError($"[Player] Error: Team with ID {newTeamId} does not exist!");
			return;
			}

		TeamId = newTeamId;
		Debug.Log($"[Player] {Name} transferred to {newTeam.Name}");
		}

	// --- Resets season stats --- //
	public void ResetSeasonStats()
		{
		MatchesWon = 0;
		MatchesPlayed = 0;
		PointsPerMatch = 0;
		PointsAwarded = 0;
		}

	// --- Resets lifetime stats --- //
	public void ResetLifetimeStats()
		{
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

	#endregion

	// Total points in the current season (editable)
	public int CurrentSeasonTotalPoints;

	// Points per match for the current season (editable)
	public float CurrentSeasonPpm;

	// Points awarded percentage for the current season (editable)
	public float CurrentSeasonPaPercentage;

	// Current season break-and-run shots (editable)
	public int CurrentSeasonBreakAndRun;

	// Current skill level for the player in the current season (editable)
	public int CurrentSeasonSkillLevel;
	}
