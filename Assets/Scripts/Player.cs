using System;
using System.Collections.Generic;
using System.Linq;  // --- Importing LINQ extension methods --- //

using UnityEngine;

[Serializable]
public class Player
	{
	// --- Region: Public Variables --- //
	public string id; // --- Unique ID for each player (changed to string for Guid compatibility) --- //

	public string name; // --- Player's name --- //
	public int skillLevel; // --- Skill level of the player --- //
	public int teamId; // --- ID of the team the player belongs to --- //
					   // --- End Region --- //

	// --- Region: Lifetime Stats --- //
	public int lifetimeMatchesPlayed; // --- Lifetime matches played --- //

	public int lifetimeMatchesWon; // --- Lifetime matches won --- //
	public float lifetimeDefensiveShotAvg; // --- Lifetime defensive shot average --- //
	public int lifetimeGamesPlayed; // --- Lifetime games played --- //
	public int lifetimeGamesWon; // --- Lifetime games won --- //
	public int lifetimeMiniSlams; // --- Lifetime mini slams --- //
	public int lifetimeNineOnTheSnap; // --- Lifetime nine on the snap --- //
	public int lifetimeShutouts; // --- Lifetime shutouts --- //
	public int lifetimeBreakAndRun; // --- Lifetime break and run --- //
	public int lifetimeTotalPoints; // --- Lifetime total points --- //
	public int matchesPlayedInLast2Years; // --- Matches played in the last 2 years --- //
										  // --- End Region --- //

	// --- Region: Current Season Stats --- //
	public int currentSeasonGamesWon; // --- Current season games won --- //

	public int currentSeasonGamesPlayed; // --- Current season games played --- //
	public float currentSeasonPpm; // --- Current season points per match --- //

	public int currentSeasonBreakAndRun; // --- Current season break and run --- //
	public float currentSeasonDefensiveShotAverage; // --- Current season defensive shot average --- //
	public int currentSeasonMatchesPlayed; // --- Current season matches played --- //
	public int currentSeasonMatchesWon; // --- Current season matches won --- //
	public int currentSeasonMiniSlams; // --- Current season mini slams --- //
	public int currentSeasonNineOnTheSnap; // --- Current season nine on the snap --- //
	public int currentSeasonShutouts; // --- Current season shutouts --- //
	public float currentSeasonPaPercentage; // --- Current season PA percentage --- //
	public int currentSeasonSkillLevel; // --- Current season skill level --- //
	public int currentSeasonTotalPoints; // --- Current season total points --- //
	public int currentSeasonPointsAwarded; // --- Current season points awarded --- //
	public float currentSeasonPointsPerMatch; // --- Current season points per match --- //
											  // --- End Region --- //

	// --- Region: Points Required to Win (New) --- //
	public int PointsRequiredToWin; // --- Points required for this player to win a match --- //
									// --- End Region --- //

	// --- Region: Static Player List --- //
	public static List<Player> playerList = new(); // --- Static list to hold all player objects --- //
												   // --- End Region --- //

	// --- Region: Constructor --- //
	public Player(string playerName, int skillLevel, int teamId)
		{
		this.id = Guid.NewGuid().ToString(); // --- Unique ID generation --- //
		this.name = playerName;
		this.skillLevel = skillLevel;
		this.teamId = teamId;

		playerList.Add(this); // --- Add the player to the static list upon creation --- //
		}
	// --- End Region --- //

	// --- Region: Utility Functions --- //
	// --- Function to get a player by ID --- //
	public static Player GetPlayerById(string playerId)
		{
		return playerList.FirstOrDefault(p => p.id == playerId); // --- Find a player by ID --- //
		}

	// --- Function to get all players by teamId --- //
	public static List<Player> GetPlayersByTeam(int teamId)
		{
		return playerList.Where(p => p.teamId == teamId).ToList(); // --- Get all players by team ID --- //
		}

	// --- Function to remove a player by ID --- //
	public static void RemovePlayerById(string playerId)
		{
		Player playerToRemove = playerList.FirstOrDefault(p => p.id == playerId);
		if (playerToRemove != null)
			{
			playerList.Remove(playerToRemove); // --- Remove player from the list --- //
			}
		}

	// --- Function to get all player names --- //
	public static List<string> GetAllPlayerNames()
		{
		return playerList.Select(p => p.name).ToList(); // --- Get all player names --- //
		}
	// --- End Region --- //

	// --- Region: JSON Save/Load Methods --- //
	// --- Save player data to JSON file --- //
	public void SavePlayerToJson(string filePath)
		{
		string json = JsonUtility.ToJson(this, true); // --- Serialize the player object to JSON --- //
		System.IO.File.WriteAllText(filePath, json); // --- Write the JSON data to a file --- //
		Debug.Log($"Player data saved to {filePath}");
		}

	// --- Load player data from JSON file --- //
	public static Player LoadPlayerFromJson(string filePath)
		{
		if (System.IO.File.Exists(filePath))
			{
			string json = System.IO.File.ReadAllText(filePath); // --- Read the JSON data from the file --- //
			Player player = JsonUtility.FromJson<Player>(json); // --- Deserialize the JSON to a Player object --- //
			Debug.Log($"Player data loaded from {filePath}");
			return player;
			}
		else
			{
			Debug.LogError("File not found.");
			return null;
			}
		}
	// --- End Region --- //
	}
