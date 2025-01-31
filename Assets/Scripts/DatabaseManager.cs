using System;
using System.Collections.Generic;
using System.IO;

using SQLite;

using UnityEngine;

public class DatabaseManager:MonoBehaviour
	{
	#region Singleton

	// Singleton instance
	public static DatabaseManager Instance { get; private set; }

	#endregion Singleton

	#region Fields

	// Path to the SQLite database file
	private string dbPath;

	// SQLite connection
	private SQLiteConnection dbConnection;

	#endregion Fields

	#region Unity Methods

	private void Awake()
		{
		// Singleton pattern to ensure only one instance of DatabaseManager exists
		if (Instance == null)
			{
			Instance = this;
			DontDestroyOnLoad(gameObject); // Optionally keep this object across scenes
			}
		else
			{
			Destroy(gameObject);
			}

		// Set the database file path
		dbPath = Path.Combine(Application.persistentDataPath, "game_data.db");

		// Open/create the SQLite database connection
		dbConnection = new SQLiteConnection(dbPath);

		// Create tables if they don't exist
		CreateTables();
		}

	private void OnApplicationQuit()
		{
		// Close the database connection when the application quits
		dbConnection.Close();
		}

	#endregion Unity Methods

	#region Database Methods

	// Create tables for Teams, Players, and Matches
	private void CreateTables()
		{
		dbConnection.CreateTable<Team>();
		dbConnection.CreateTable<Player>();
		dbConnection.CreateTable<Match>();
		Debug.Log("Database tables created or already exist.");
		}

	// Add a new team to the database
	public void AddTeam(Team team)
		{
		dbConnection.Insert(team);
		Debug.Log($"Team '{team.Name}' added to the database.");
		}

	// Add a new player to the database
	public void AddPlayer(Player player)
		{
		dbConnection.Insert(player);
		Debug.Log($"Player '{player.Name}' added to the database.");
		}

	// Save a player to the database (or update if already exists)
	public void SavePlayer(Player player)
		{
		var existingPlayer = dbConnection.Table<Player>().FirstOrDefault(p => p.Id == player.Id);
		if (existingPlayer != null)
			{
			dbConnection.Update(player); // Update player in the database
			Debug.Log($"Player '{player.Name}' updated.");
			}
		else
			{
			dbConnection.Insert(player); // Insert new player if doesn't exist
			Debug.Log($"Player '{player.Name}' added to the database.");
			}
		}

	// Get all teams
	public List<Team> GetAllTeams()
		{
		return dbConnection.Table<Team>().ToList();
		}

	// Get a team by its ID
	public Team GetTeamById(int teamId)
		{
		var team = dbConnection.Table<Team>().FirstOrDefault(t => t.Id == teamId);
		if (team == null)
			{
			Debug.LogError($"Team with ID {teamId} not found.");
			}
		return team;
		}

	// Get a team by its name
	public Team GetTeamByName(string teamName)
		{
		// Perform a case-insensitive search for the team
		var team = dbConnection.Table<Team>().FirstOrDefault(t => t.Name.Equals(teamName, StringComparison.OrdinalIgnoreCase));

		if (team == null)
			{
			Debug.LogError($"Team with name {teamName} not found.");
			}

		return team;
		}


	// Get players by team ID
	public List<Player> GetPlayersByTeam(int teamId)
		{
		return dbConnection.Table<Player>().Where(p => p.TeamId == teamId).ToList();
		}


	// In DatabaseManager.cs
	public List<Player> GetAllPlayers()
		{
		// Assuming you're using SQLite to fetch players from a database
		var players = new List<Player>();

		// Example: Fetching all players from a database
		using (var connection = new SQLiteConnection(dbPath))
			{
			players = connection.Table<Player>().ToList();
			}

		return players;
		}


	// Get all matches
	public List<Match> GetAllMatches()
		{
		return dbConnection.Table<Match>().ToList();
		}

	// Insert a match result into the database
	public void AddMatchResult(Match match)
		{
		dbConnection.Insert(match);
		Debug.Log("Match result added to the database.");
		}

	// Update an existing team
	public void UpdateTeam(Team team)
		{
		var existingTeam = dbConnection.Table<Team>().FirstOrDefault(t => t.Id == team.Id);
		if (existingTeam != null)
			{
			existingTeam.Name = team.Name;  // Update other fields as necessary
			dbConnection.Update(existingTeam);  // Update the team in the database
			Debug.Log($"Team '{team.Name}' updated.");
			}
		else
			{
			Debug.LogError("Team not found for update.");
			}
		}

	// Remove a team from the database
	public void RemoveTeam(Team team)
		{
		var playersInTeam = dbConnection.Table<Player>().Where(p => p.TeamId == team.Id).ToList();
		if (playersInTeam.Count > 0)
			{
			Debug.LogError("Cannot remove a team with players still assigned to it.");
			return;
			}

		dbConnection.Delete(team);  // Delete the team from the database
		Debug.Log($"Team '{team.Name}' removed from the database.");
		}

	#endregion Database Methods
	}
