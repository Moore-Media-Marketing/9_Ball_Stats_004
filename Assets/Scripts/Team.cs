using System;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
using System.Linq;

[Serializable]
public class Team
	{
	// --- Team Basic Info --- //
	public int teamId;
	public string teamName;
	public List<int> playerIds; // List of player IDs associated with this team

	// --- SQLite Database Methods --- //
	private static string dbPath = "TeamDatabase.db";  // Path to the SQLite database file
	private static SQLiteConnection db;

	// --- Parameterless Constructor --- //
	public Team()
		{
		playerIds = new List<int>();  // Initialize player IDs list as empty
		}

	// --- Constructor with Team Name --- //
	public Team(string teamName)
		{
		this.teamName = teamName;
		this.playerIds = new List<int>();  // Initialize player IDs list as empty
		}

	// --- Initialize Database --- //
	public static void InitializeDatabase()
		{
		if (db == null)
			{
			db = new SQLiteConnection(dbPath);
			db.CreateTable<Team>();  // Create Team table if it doesn't exist
			}
		}

	// --- Save Team Data --- //
	public void SaveTeamData()
		{
		try
			{
			// If team already exists, update, else insert new record
			var existingTeam = db.Table<Team>().Where(t => t.teamName == this.teamName).FirstOrDefault();
			if (existingTeam != null)
				{
				db.Update(this);
				Debug.Log($"Team data for {teamName} updated successfully.");
				}
			else
				{
				db.Insert(this);
				Debug.Log($"Team data for {teamName} saved successfully.");
				}
			}
		catch (Exception ex)
			{
			Debug.LogError("Error saving team data: " + ex.Message);
			}
		}

	// --- Load Team Data --- //
	public static Team LoadTeamData(int teamId)
		{
		try
			{
			return db.Table<Team>().Where(t => t.teamId == teamId).FirstOrDefault();
			}
		catch (Exception ex)
			{
			Debug.LogError("Error loading team data: " + ex.Message);
			return null;
			}
		}

	// --- Update Team Name --- //
	public void UpdateTeamName(string newTeamName)
		{
		this.teamName = newTeamName;
		SaveTeamData();
		}

	// --- Add Player to Team --- //
	public void AddPlayerToTeam(int playerId)
		{
		if (!playerIds.Contains(playerId))
			{
			playerIds.Add(playerId);
			SaveTeamData();
			Debug.Log($"Player with ID {playerId} added to {teamName}.");
			}
		else
			{
			Debug.LogWarning($"Player with ID {playerId} is already on {teamName}.");
			}
		}

	// --- Remove Player from Team --- //
	public void RemovePlayerFromTeam(int playerId)
		{
		if (playerIds.Contains(playerId))
			{
			playerIds.Remove(playerId);
			SaveTeamData();
			Debug.Log($"Player with ID {playerId} removed from {teamName}.");
			}
		else
			{
			Debug.LogWarning($"Player with ID {playerId} is not on {teamName}.");
			}
		}

	// --- Get Team by ID --- //
	public static Team GetTeamById(int teamId)
		{
		return db.Table<Team>().Where(t => t.teamId == teamId).FirstOrDefault();
		}

	// --- Get All Teams --- //
	public static List<Team> GetAllTeams()
		{
		return db.Table<Team>().ToList();
		}
	}
