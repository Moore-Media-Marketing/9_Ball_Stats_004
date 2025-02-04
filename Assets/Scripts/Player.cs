using System;
using UnityEngine;
using SQLite4Unity3d;

[Serializable]
public class Player
	{
	// --- Player Basic Info --- //
	public int playerId;
	public string playerName;
	public int teamId;

	// --- Current Season Stats --- //
	public int currentSeasonMatchesPlayed;
	public int currentSeasonMatchesWon;
	public int currentSeasonMiniSlams;
	public int currentSeasonNineOnTheSnap;
	public float currentSeasonDefensiveShotAverage;
	public int currentSeasonShutouts;
	public float currentSeasonPaPercentage;
	public int currentSeasonPointsAwarded;
	public float currentSeasonPointsPerMatch;
	public int currentSeasonTotalPoints;
	public int currentSeasonSkillLevel;
	public float currentSeasonPpm;
	public int currentSeasonBreakAndRun;
	public int currentSeasonGamesPlayed;

	// --- Lifetime Stats --- //
	public int lifetimeMatchesPlayed;
	public int lifetimeMatchesWon;
	public int lifetimeMiniSlams;
	public int lifetimeNineOnTheSnap;
	public int lifetimeShutouts;
	public int lifetimeGamesPlayed;
	public int lifetimeGamesWon;
	public float lifetimeDefensiveShotAverage;
	public int lifetimeBreakAndRun;

	// --- SQLite Database Methods --- //
	private static string dbPath = "PlayerDatabase.db";  // Path to the SQLite database file
	private static SQLiteConnection db;

	// --- Constructor --- //
	public Player() // Parameterless constructor for SQLite
		{
		// Initialize stats to zero or default values
		InitializeStats();
		}

	public Player(string playerName, int teamId) // Constructor with player details
		{
		this.playerName = playerName;
		this.teamId = teamId;

		// Initialize stats to zero or default values
		InitializeStats();
		}

	// --- Initialize Stats --- //
	private void InitializeStats()
		{
		// Initialize current season stats
		currentSeasonMatchesPlayed = 0;
		currentSeasonMatchesWon = 0;
		currentSeasonMiniSlams = 0;
		currentSeasonNineOnTheSnap = 0;
		currentSeasonDefensiveShotAverage = 0f;
		currentSeasonShutouts = 0;
		currentSeasonPaPercentage = 0f;
		currentSeasonPointsAwarded = 0;
		currentSeasonPointsPerMatch = 0f;
		currentSeasonTotalPoints = 0;
		currentSeasonSkillLevel = 1;
		currentSeasonPpm = 0f;
		currentSeasonBreakAndRun = 0;
		currentSeasonGamesPlayed = 0;

		// Initialize lifetime stats
		lifetimeMatchesPlayed = 0;
		lifetimeMatchesWon = 0;
		lifetimeMiniSlams = 0;
		lifetimeNineOnTheSnap = 0;
		lifetimeShutouts = 0;
		lifetimeGamesPlayed = 0;
		lifetimeGamesWon = 0;
		lifetimeDefensiveShotAverage = 0f;
		lifetimeBreakAndRun = 0;
		}

	// --- Initialize Database --- //
	public static void InitializeDatabase()
		{
		if (db == null)
			{
			db = new SQLiteConnection(dbPath);
			db.CreateTable<Player>();  // Create Player table if it doesn't exist
			}
		}

	// --- Save Player Data --- //
	public void SavePlayerData()
		{
		try
			{
			// If player already exists, update, else insert new record
			var existingPlayer = db.Table<Player>().Where(p => p.playerName == this.playerName && p.teamId == this.teamId).FirstOrDefault();
			if (existingPlayer != null)
				{
				db.Update(this);
				Debug.Log($"Player data for {playerName} updated successfully.");
				}
			else
				{
				db.Insert(this);
				Debug.Log($"Player data for {playerName} saved successfully.");
				}
			}
		catch (Exception ex)
			{
			Debug.LogError("Error saving player data: " + ex.Message);
			}
		}

	// --- Load Player Data --- //
	public static Player LoadPlayerData(int playerId)
		{
		try
			{
			return db.Table<Player>().Where(p => p.playerId == playerId).FirstOrDefault();
			}
		catch (Exception ex)
			{
			Debug.LogError("Error loading player data: " + ex.Message);
			return null;
			}
		}

	// --- Update Current Season Stats --- //
	public void UpdateCurrentSeasonStats(int matchesPlayed, int matchesWon, int miniSlams, int nineOnTheSnap,
										 float defensiveShotAverage, int shutouts, float paPercentage,
										 int pointsAwarded, float pointsPerMatch, int totalPoints,
										 int skillLevel, float ppm, int breakAndRun, int gamesPlayed)
		{
		currentSeasonMatchesPlayed = matchesPlayed;
		currentSeasonMatchesWon = matchesWon;
		currentSeasonMiniSlams = miniSlams;
		currentSeasonNineOnTheSnap = nineOnTheSnap;
		currentSeasonDefensiveShotAverage = defensiveShotAverage;
		currentSeasonShutouts = shutouts;
		currentSeasonPaPercentage = paPercentage;
		currentSeasonPointsAwarded = pointsAwarded;
		currentSeasonPointsPerMatch = pointsPerMatch;
		currentSeasonTotalPoints = totalPoints;
		currentSeasonSkillLevel = skillLevel;
		currentSeasonPpm = ppm;
		currentSeasonBreakAndRun = breakAndRun;
		currentSeasonGamesPlayed = gamesPlayed;

		SavePlayerData();
		}

	// --- Update Lifetime Stats --- //
	public void UpdateLifetimeStats(int matchesPlayed, int matchesWon, int miniSlams, int nineOnTheSnap,
									int shutouts, int gamesPlayed, int gamesWon, float defensiveShotAverage,
									int breakAndRun)
		{
		lifetimeMatchesPlayed = matchesPlayed;
		lifetimeMatchesWon = matchesWon;
		lifetimeMiniSlams = miniSlams;
		lifetimeNineOnTheSnap = nineOnTheSnap;
		lifetimeShutouts = shutouts;
		lifetimeGamesPlayed = gamesPlayed;
		lifetimeGamesWon = gamesWon;
		lifetimeDefensiveShotAverage = defensiveShotAverage;
		lifetimeBreakAndRun = breakAndRun;

		SavePlayerData();
		}

	// --- Get Player by Team ID --- //
	public static Player GetPlayerByTeamId(int teamId)
		{
		return db.Table<Player>().Where(p => p.teamId == teamId).FirstOrDefault();
		}
	}
