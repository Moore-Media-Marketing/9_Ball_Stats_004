using System;
using UnityEngine;
using SQLite;

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

	[Tooltip("Defensive shot average in lifetime.")]
	public float LifetimeDefensiveShotAverage;

	[Tooltip("Matches played in the last 2 years.")]
	public int MatchesPlayedInLast2Years;

	[Tooltip("Number of 9-on-the-Snap shots in lifetime.")]
	public int LifetimeNineOnTheSnap;

	[Tooltip("Number of Mini Slams achieved in lifetime.")]
	public int LifetimeMiniSlams;

	[Tooltip("Number of Shutouts achieved in lifetime.")]
	public int LifetimeShutouts;

	[Tooltip("Lifetime games won.")]
	public int LifetimeGamesWon;

	[Tooltip("Lifetime games played.")]
	public int LifetimeGamesPlayed;

	[Tooltip("Lifetime defensive shot average.")]
	public float LifetimeDefensiveShotAvg;

	[Tooltip("Lifetime break-and-run shots.")]
	public int LifetimeBreakAndRun;

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

	[Header("Editable Current Season Stats")]
	[Tooltip("Total points in the current season.")]
	public int CurrentSeasonTotalPoints;

	[Tooltip("Points per match for the current season.")]
	public float CurrentSeasonPpm;

	[Tooltip("Points awarded percentage for the current season.")]
	public float CurrentSeasonPaPercentage;

	[Tooltip("Current season break-and-run shots.")]
	public int CurrentSeasonBreakAndRun;

	[Tooltip("Current skill level for the player in the current season.")]
	public int CurrentSeasonSkillLevel;

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

		// Update additional lifetime stats
		LifetimeGamesPlayed++;
		LifetimeGamesWon += wonMatch ? 1 : 0;

		Debug.Log($"[Player] Updated Stats: {Name} | Matches: {MatchesPlayed} | Wins: {MatchesWon}");
		}

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

	public void ResetSeasonStats()
		{
		MatchesWon = 0;
		MatchesPlayed = 0;
		PointsPerMatch = 0;
		PointsAwarded = 0;
		}

	public void ResetLifetimeStats()
		{
		LifetimeMatchesWon = 0;
		LifetimeMatchesPlayed = 0;
		LifetimeDefensiveShotAverage = 0;
		MatchesPlayedInLast2Years = 0;
		LifetimeNineOnTheSnap = 0;
		LifetimeMiniSlams = 0;
		LifetimeShutouts = 0;
		LifetimeGamesWon = 0;
		LifetimeGamesPlayed = 0;
		LifetimeDefensiveShotAvg = 0;
		LifetimeBreakAndRun = 0;
		}

	#endregion
	}
