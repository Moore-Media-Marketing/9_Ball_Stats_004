using MyGame.Database;  // Reference to your custom SQLite helper namespace
using UnityEngine;
using System.Collections.Generic;

public class DatabaseManager:MonoBehaviour
	{
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

		// Initialize database connection through DatabaseHelper
		DatabaseHelper.InitializeDatabase();
		}

	// --- Save Player Data --- //
	public void SavePlayerData(Player player)
		{
		try
			{
			// Ensure that playerId and playerName are valid in the Player class
			var existingPlayer = DatabaseHelper.GetPlayersForTeam(player.TeamId).Find(p => p.PlayerName == player.PlayerName);
			if (existingPlayer != null)
				{
				DatabaseHelper.UpdatePlayer(player);  // Update existing player data
				Debug.Log($"Player data for {player.PlayerName} updated successfully.");
				}
			else
				{
				DatabaseHelper.InsertPlayer(player);  // Insert new player data
				Debug.Log($"Player data for {player.PlayerName} saved successfully.");
				}
			}
		catch (System.Exception ex)
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
			return DatabaseHelper.GetAllPlayers().Find(p => p.PlayerId == playerId);
			}
		catch (System.Exception ex)
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
			var existingTeam = DatabaseHelper.GetAllTeams().Find(t => t.TeamName == team.TeamName);
			if (existingTeam != null)
				{
				DatabaseHelper.UpdateTeam(team);  // Update existing team data
				Debug.Log($"Team data for {team.TeamName} updated successfully.");
				}
			else
				{
				DatabaseHelper.InsertTeam(team);  // Insert new team data
				Debug.Log($"Team data for {team.TeamName} saved successfully.");
				}
			}
		catch (System.Exception ex)
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
			return DatabaseHelper.GetTeamById(teamId);
			}
		catch (System.Exception ex)
			{
			Debug.LogError($"Error loading team data: {ex.Message}");
			return null;
			}
		}

	// --- Delete Player by ID --- //
	public void DeletePlayer(int playerId)
		{
		try
			{
			var player = DatabaseHelper.GetPlayersForTeam(playerId).Find(p => p.PlayerId == playerId);
			if (player != null)
				{
				DatabaseHelper.DeletePlayer(player);  // Delete player from the database
				Debug.Log($"Player with ID {playerId} deleted successfully.");
				}
			else
				{
				Debug.LogWarning($"Player with ID {playerId} not found.");
				}
			}
		catch (System.Exception ex)
			{
			Debug.LogError($"Error deleting player: {ex.Message}");
			}
		}

	// --- Delete Team by ID --- //
	public void DeleteTeam(int teamId)
		{
		try
			{
			var team = DatabaseHelper.GetTeamById(teamId);
			if (team != null)
				{
				DatabaseHelper.DeleteTeam(team);  // Delete team from the database
				Debug.Log($"Team with ID {teamId} deleted successfully.");
				}
			else
				{
				Debug.LogWarning($"Team with ID {teamId} not found.");
				}
			}
		catch (System.Exception ex)
			{
			Debug.LogError($"Error deleting team: {ex.Message}");
			}
		}
	}
