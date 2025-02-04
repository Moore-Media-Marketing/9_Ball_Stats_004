using SQLite;
using UnityEngine;
using System.Collections.Generic;

public static class DatabaseHelper
	{
	// --- Database Connection --- //
	private static SQLiteConnection db;

	// Initialize the database connection
	static DatabaseHelper()
		{
		db = new SQLiteConnection(System.IO.Path.Combine(Application.persistentDataPath, "Database.db"));
		db.CreateTable<Player>();  // Create the 'Players' table
		db.CreateTable<Team>();    // Create the 'Teams' table
		}

	// --- Insert Team --- //
	public static void InsertTeam(Team team)
		{
		try
			{
			db.Insert(team);
			Debug.Log($"Team {team.TeamName} inserted successfully.");
			}
		catch (System.Exception ex)
			{
			Debug.LogError($"Error inserting team: {ex.Message}");
			}
		}

	// --- Update Team --- //
	public static void UpdateTeam(Team team)
		{
		try
			{
			db.Update(team);
			Debug.Log($"Team {team.TeamName} updated successfully.");
			}
		catch (System.Exception ex)
			{
			Debug.LogError($"Error updating team: {ex.Message}");
			}
		}

	// --- Get Team by ID --- //
	public static Team GetTeamById(int teamId)
		{
		try
			{
			return db.Table<Team>().Where(t => t.TeamId == teamId).FirstOrDefault();
			}
		catch (System.Exception ex)
			{
			Debug.LogError($"Error retrieving team by ID {teamId}: {ex.Message}");
			return null;
			}
		}

	// --- Get All Teams --- //
	public static List<Team> GetAllTeams()
		{
		try
			{
			return db.Table<Team>().ToList();
			}
		catch (System.Exception ex)
			{
			Debug.LogError($"Error retrieving teams: {ex.Message}");
			return new List<Team>();
			}
		}

	// --- Get Players for a Team --- //
	public static List<Player> GetPlayersForTeam(int teamId)
		{
		try
			{
			// Get the team by its ID
			var team = db.Table<Team>().Where(t => t.TeamId == teamId).FirstOrDefault();
			if (team != null)
				{
				// Retrieve players who belong to this team based on PlayerIds
				List<Player> players = new List<Player>();
				foreach (var playerId in team.PlayerIds)
					{
					var player = db.Table<Player>().Where(p => p.PlayerId == playerId).FirstOrDefault();
					if (player != null)
						{
						players.Add(player);
						}
					}
				return players;
				}
			return new List<Player>();  // Return empty list if team not found
			}
		catch (System.Exception ex)
			{
			Debug.LogError($"Error retrieving players for team {teamId}: {ex.Message}");
			return new List<Player>();
			}
		}

	// --- Delete Team --- //
	public static void DeleteTeam(Team team)
		{
		try
			{
			db.Delete(team);
			Debug.Log($"Team {team.TeamName} deleted successfully.");
			}
		catch (System.Exception ex)
			{
			Debug.LogError($"Error deleting team: {ex.Message}");
			}
		}

	// --- Initialize Database --- //
	public static void InitializeDatabase()
		{
		try
			{
			// Create tables if they don't exist
			db.CreateTable<Player>();
			db.CreateTable<Team>();
			Debug.Log("Database initialized successfully.");
			}
		catch (System.Exception ex)
			{
			Debug.LogError($"Error initializing database: {ex.Message}");
			}
		}
	}
