using System;
using System.Collections.Generic;
using System.Linq;

using SQLite4Unity3d;

using UnityEngine;

public class DatabaseManager:MonoBehaviour
	{
	// --- Database Path --- //
	[SerializeField]
	private string dbPath = "Database.db";  // Path for SQLite database (visible in Inspector)

	private SQLiteConnection db;

	// --- Singleton Pattern --- //
	public static DatabaseManager Instance { get; private set; }

	private void Awake()
		{
		// Ensure there's only one instance of the DatabaseManager
		if (Instance == null)
			{
			Instance = this;
			DontDestroyOnLoad(gameObject);  // Keep the instance between scene loads
			}
		else
			{
			Destroy(gameObject);  // Destroy duplicates
			}

		InitializeDatabase();
		}

	// --- Initialize Database --- //
	public void InitializeDatabase()
		{
		if (db == null)
			{
			db = new SQLiteConnection(dbPath);
			db.CreateTable<Player>();  // Create Player table if it doesn't exist
			db.CreateTable<Team>();    // Create Team table if it doesn't exist
			Debug.Log("Database initialized and tables created.");
			}
		}

	// --- Save Player Data --- //
	public void SavePlayerData(Player player)
		{
		try
			{
			// Ensure that playerId and playerName are valid in the Player class
			var existingPlayer = db.Table<Player>().Where(p => p.PlayerName == player.PlayerName && p.TeamId == player.TeamId).FirstOrDefault();
			if (existingPlayer != null)
				{
				db.Update(player);  // Update existing player data
				Debug.Log($"Player data for {player.PlayerName} updated successfully.");
				}
			else
				{
				db.Insert(player);  // Insert new player data
				Debug.Log($"Player data for {player.PlayerName} saved successfully.");
				}
			}
		catch (Exception ex)
			{
			Debug.LogError($"Error saving player data: {ex.Message}");
			}
		}

	// --- Load Player Data by ID --- //
	public Player LoadPlayerData(int playerId)
		{
		try
			{
			// Load player data based on playerId
			return db.Table<Player>().Where(p => p.PlayerId == playerId).FirstOrDefault();
			}
		catch (Exception ex)
			{
			Debug.LogError($"Error loading player data: {ex.Message}");
			return null;
			}
		}

	// --- Save Team Data --- //
	public void SaveTeamData(Team team)
		{
		try
			{
			var existingTeam = db.Table<Team>().Where(t => t.TeamName == team.TeamName).FirstOrDefault();
			if (existingTeam != null)
				{
				db.Update(team);  // Update existing team data
				Debug.Log($"Team data for {team.TeamName} updated successfully.");
				}
			else
				{
				db.Insert(team);  // Insert new team data
				Debug.Log($"Team data for {team.TeamName} saved successfully.");
				}
			}
		catch (Exception ex)
			{
			Debug.LogError($"Error saving team data: {ex.Message}");
			}
		}

	// --- Load Team Data by ID --- //
	public Team LoadTeamData(int teamId)
		{
		try
			{
			// Load team data based on teamId
			return db.Table<Team>().Where(t => t.TeamId == teamId).FirstOrDefault();
			}
		catch (Exception ex)
			{
			Debug.LogError($"Error loading team data: {ex.Message}");
			return null;
			}
		}

	// --- Get All Teams --- //
	public List<Team> GetAllTeams()
		{
		try
			{
			return db.Table<Team>().ToList();  // Retrieve all teams
			}
		catch (Exception ex)
			{
			Debug.LogError($"Error retrieving all teams: {ex.Message}");
			return new List<Team>();
			}
		}

	// --- Get All Players --- //
	public List<Player> GetAllPlayers()
		{
		try
			{
			return db.Table<Player>().ToList();  // Retrieve all players
			}
		catch (Exception ex)
			{
			Debug.LogError($"Error retrieving all players: {ex.Message}");
			return new List<Player>();
			}
		}

	// --- Delete Player by ID --- //
	public void DeletePlayer(int playerId)
		{
		try
			{
			var player = db.Table<Player>().Where(p => p.PlayerId == playerId).FirstOrDefault();
			if (player != null)
				{
				db.Delete(player);  // Delete player from the database
				Debug.Log($"Player with ID {playerId} deleted successfully.");
				}
			else
				{
				Debug.LogWarning($"Player with ID {playerId} not found.");
				}
			}
		catch (Exception ex)
			{
			Debug.LogError($"Error deleting player: {ex.Message}");
			}
		}

	// --- Delete Team by ID --- //
	public void DeleteTeam(int teamId)
		{
		try
			{
			var team = db.Table<Team>().Where(t => t.TeamId == teamId).FirstOrDefault();
			if (team != null)
				{
				db.Delete(team);  // Delete team from the database
				Debug.Log($"Team with ID {teamId} deleted successfully.");
				}
			else
				{
				Debug.LogWarning($"Team with ID {teamId} not found.");
				}
			}
		catch (Exception ex)
			{
			Debug.LogError($"Error deleting team: {ex.Message}");
			}
		}

	// --- Additional Functions --- //
	// You can add any extra utility functions here if needed in the future
	}
