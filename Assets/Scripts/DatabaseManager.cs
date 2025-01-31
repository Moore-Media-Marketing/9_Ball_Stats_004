using System.Collections.Generic;
using System.IO;

using SQLite;

using UnityEngine;

public class DatabaseManager:MonoBehaviour
	{
	// Singleton instance
	public static DatabaseManager Instance { get; private set; }

	// Path to the SQLite database file
	private string dbPath;

	// SQLite connection
	private SQLiteConnection dbConnection;

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

	// Create tables for Teams, Players, and Matches
	private void CreateTables()
		{
		dbConnection.CreateTable<Team>();
		dbConnection.CreateTable<Player>();
		dbConnection.CreateTable<Match>();
		}

	// Add a new team to the database
	public void AddTeam(Team team)
		{
		dbConnection.Insert(team);
		}

	// Add a new player to the database
	public void AddPlayer(Player player)
		{
		dbConnection.Insert(player);
		}

	// Get all teams
	public List<Team> GetAllTeams()
		{
		return dbConnection.Table<Team>().ToList();
		}

	// Get players by team ID
	public List<Player> GetPlayersByTeam(int teamId)
		{
		return dbConnection.Table<Player>().Where(p => p.TeamId == teamId).ToList();
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
		}

	// Remove a team from the database
	public void RemoveTeam(Team team)
		{
		// Ensure there are no players associated with the team before deleting
		var playersInTeam = dbConnection.Table<Player>().Where(p => p.TeamId == team.Id).ToList();
		if (playersInTeam.Count > 0)
			{
			Debug.LogError("Cannot remove a team with players still assigned to it.");
			return;
			}

		dbConnection.Delete(team);  // Delete the team from the database
		}

	// Close the database connection when the application quits
	private void OnApplicationQuit()
		{
		dbConnection.Close();
		}
	}