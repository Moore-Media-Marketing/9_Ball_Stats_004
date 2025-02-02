using System;                             // --- Import system namespaces ---

using UnityEngine;                        // --- Import Unity namespaces ---

// --- Region: Player Class Definition --- //
[Serializable]
public class Player
	{
	// --- Region: Public Variables --- //
	public int id;                         // --- Unique ID for each player ---
	public string name;                    // --- Player's name ---
	public int skillLevel;                 // --- Skill level of the player ---
	public int teamId;                     // --- ID of the team the player belongs to ---

	// --- Region: Lifetime Stats --- //
	public int lifetimeMatchesPlayed;      // --- Lifetime matches played ---
	public int lifetimeMatchesWon;         // --- Lifetime matches won ---
	public float lifetimeDefensiveShotAvg; // --- Lifetime defensive shot average ---
	public int lifetimeGamesPlayed;        // --- Lifetime games played ---
	public int lifetimeGamesWon;           // --- Lifetime games won ---
	public int lifetimeMiniSlams;          // --- Lifetime mini slams ---
	public int lifetimeNineOnTheSnap;      // --- Lifetime nine on the snap ---
	public int lifetimeShutouts;           // --- Lifetime shutouts ---
	public int lifetimeBreakAndRun;        // --- Lifetime break and run ---
	public int lifetimeTotalPoints;        // --- Lifetime total points ---
	public int matchesPlayedInLast2Years;  // --- Matches played in the last 2 years ---

	// --- Region: Current Season Stats --- //
	public int currentSeasonBreakAndRun;   // --- Current season break and run ---
	public float currentSeasonDefensiveShotAverage; // --- Current season defensive shot average ---
	public int currentSeasonMatchesPlayed; // --- Current season matches played ---
	public int currentSeasonMatchesWon;    // --- Current season matches won ---
	public int currentSeasonMiniSlams;     // --- Current season mini slams ---
	public int currentSeasonNineOnTheSnap; // --- Current season nine on the snap ---
	public float currentSeasonPaPercentage; // --- Current season PA percentage ---
	public int currentSeasonPointsAwarded; // --- Current season points awarded ---
	public float currentSeasonPointsPerMatch; // --- Current season points per match ---
	public int currentSeasonShutouts;     // --- Current season shutouts ---
	public int currentSeasonSkillLevel;   // --- Current season skill level ---
	public int currentSeasonTotalPoints;  // --- Current season total points ---
	public int currentSeasonGamesPlayed;  // --- Current season games played ---
	public int currentSeasonGamesWon;     // --- Current season games won ---
	public int currentSeasonPpm;          // --- Current season PPM ---

	// --- Region: Points Required to Win --- //
	public int PointsRequiredToWin;       // --- Points required to win based on skill level ---

	// --- Region: Constructor --- //
	public Player(string name, int skillLevel, int teamId)
		{
		this.name = name;                 // --- Set player name ---
		this.skillLevel = skillLevel;     // --- Set player skill level ---
		this.teamId = teamId;             // --- Set the player's team ID ---
		}

	// --- Region: Additional Functions --- //
	}
// --- End Region: Player Class Definition --- //
