// --- Region: Using Directives --- //
using System;

using UnityEngine;

// --- End Region: Using Directives --- //

[Serializable]
public class Player
	{
	// --- Region: Player Information --- //

	public int id; // Unique identifier for the player (assigned by DatabaseManager)
	[Tooltip("Enter the player's name.")]
	public string name; // Name of the player
	public int teamId; // ID of the team to which the player belongs

	// --- End Region: Player Information --- //

	// --- Region: Current Season Stats --- //

	public int currentSeasonTotalPoints; // Total points scored by the player in the current season
	public float currentSeasonPpm; // Points per match in the current season
	public float currentSeasonPaPercentage; // Accuracy percentage in the current season
	public int currentSeasonBreakAndRun; // Break and run statistic for the current season
	public int currentSeasonSkillLevel; // Skill level of the player for the current season

	// --- New fields for current season stats --- //
	public int currentSeasonGamesPlayed; // Number of games played in the current season
	public int currentSeasonGamesWon; // Number of games won in the current season
	public int currentSeasonMiniSlams; // Mini slams for the current season
	public int currentSeasonNineOnTheSnap; // Nine on the snap for the current season
	public int currentSeasonShutouts; // Shutouts for the current season

	// --- End Region: Current Season Stats --- //

	// --- Region: Lifetime Stats --- //

	public int lifetimeGamesWon; // Lifetime games won
	public int lifetimeGamesPlayed; // Lifetime games played
	public float lifetimeDefensiveShotAvg; // Lifetime defensive shot average
	public int matchesPlayedInLast2Years; // Matches played in the last 2 years
	public int lifetimeBreakAndRun; // Lifetime break and run
	public int lifetimeNineOnTheSnap; // Lifetime nine on the snap
	public int lifetimeMiniSlams; // Lifetime mini slams
	public int lifetimeShutouts; // Lifetime shutouts

	// --- End Region: Lifetime Stats --- //

	// --- Region: Constructors --- //

	public Player(string name, int startingSkillLevel, int teamId)
		{
		this.name = name;
		currentSeasonSkillLevel = startingSkillLevel;
		this.teamId = teamId;
		}

	// --- End Region: Constructors --- //

	// --- Region: Update Methods --- //

	public void UpdateCurrentSeasonStats(int totalPoints, float ppm, float paPercentage, int breakAndRun,
										  int gamesPlayed, int gamesWon, int miniSlams, int nineOnTheSnap, int shutouts)
		{
		currentSeasonTotalPoints = totalPoints;
		currentSeasonPpm = ppm;
		currentSeasonPaPercentage = paPercentage;
		currentSeasonBreakAndRun = breakAndRun;
		currentSeasonGamesPlayed = gamesPlayed; // Update games played
		currentSeasonGamesWon = gamesWon; // Update games won
		currentSeasonMiniSlams = miniSlams; // Update mini slams
		currentSeasonNineOnTheSnap = nineOnTheSnap; // Update nine on the snap
		currentSeasonShutouts = shutouts; // Update shutouts
		}

	public void UpdateLifetimeStats(int gamesWon, int gamesPlayed, float defensiveShotAvg, int matchesLast2Years,
									int breakAndRun, int nineOnTheSnap, int miniSlams, int shutouts)
		{
		lifetimeGamesWon = gamesWon;
		lifetimeGamesPlayed = gamesPlayed;
		lifetimeDefensiveShotAvg = defensiveShotAvg;
		matchesPlayedInLast2Years = matchesLast2Years;
		lifetimeBreakAndRun = breakAndRun;
		lifetimeNineOnTheSnap = nineOnTheSnap;
		lifetimeMiniSlams = miniSlams;
		lifetimeShutouts = shutouts;
		}

	// --- End Region: Update Methods --- //

	// --- Region: Additional Functions --- //
	// --- Add any extra helper methods for the Player class here --- //
	// --- End Region: Additional Functions --- //
	}
