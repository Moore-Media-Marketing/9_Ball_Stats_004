using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class DatabaseManager:MonoBehaviour
	{
	// --- Singleton Pattern --- //
	public static DatabaseManager Instance { get; private set; }

	private string playersCsvPath = "Assets/Resources/players.csv";
	private string teamsCsvPath = "Assets/Resources/teams.csv";

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
		}

	// --- Save Player Data --- //
	public void SavePlayerData(Player player)
		{
		try
			{
			List<Player> players = LoadPlayersFromCsv();
			var existingPlayer = players.Find(p => p.PlayerName == player.PlayerName);

			if (existingPlayer != null)
				{
				existingPlayer = player;  // Update existing player data
				Debug.Log($"Player data for {player.PlayerName} updated successfully.");
				}
			else
				{
				players.Add(player);  // Insert new player data
				Debug.Log($"Player data for {player.PlayerName} saved successfully.");
				}

			SavePlayersToCsv(players);  // Save back to CSV
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
			List<Player> players = LoadPlayersFromCsv();
			return players.Find(p => p.PlayerId == playerId);
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
			List<Team> teams = LoadTeamsFromCsv();
			var existingTeam = teams.Find(t => t.TeamName == team.TeamName);

			if (existingTeam != null)
				{
				existingTeam = team;  // Update existing team data
				Debug.Log($"Team data for {team.TeamName} updated successfully.");
				}
			else
				{
				teams.Add(team);  // Insert new team data
				Debug.Log($"Team data for {team.TeamName} saved successfully.");
				}

			SaveTeamsToCsv(teams);  // Save back to CSV
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
			List<Team> teams = LoadTeamsFromCsv();
			return teams.Find(t => t.TeamId == teamId);
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
			List<Player> players = LoadPlayersFromCsv();
			var player = players.Find(p => p.PlayerId == playerId);
			if (player != null)
				{
				players.Remove(player);  // Delete player from the list
				Debug.Log($"Player with ID {playerId} deleted successfully.");
				SavePlayersToCsv(players);  // Save back to CSV
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
			List<Team> teams = LoadTeamsFromCsv();
			var team = teams.Find(t => t.TeamId == teamId);
			if (team != null)
				{
				teams.Remove(team);  // Delete team from the list
				Debug.Log($"Team with ID {teamId} deleted successfully.");
				SaveTeamsToCsv(teams);  // Save back to CSV
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

	// --- Load Players from CSV --- //
	private List<Player> LoadPlayersFromCsv()
		{
		List<Player> players = new();
		try
			{
			if (File.Exists(playersCsvPath))
				{
				string[] lines = File.ReadAllLines(playersCsvPath);
				foreach (string line in lines)
					{
					string[] values = line.Split(',');
					if (values.Length == 5)  // Assuming 5 columns in the CSV (PlayerId, PlayerName, SkillLevel, TotalGames, TotalWins)
						{
						Player player = new()
							{
							PlayerId = int.Parse(values[0]),
							PlayerName = values[1],
							SkillLevel = int.Parse(values[2]),
							TotalGames = int.Parse(values[3]),
							TotalWins = int.Parse(values[4])
							};
						players.Add(player);
						}
					}
				}
			}
		catch (System.Exception ex)
			{
			Debug.LogError($"Error loading players from CSV: {ex.Message}");
			}
		return players;
		}

	// --- Save Players to CSV --- //
	private void SavePlayersToCsv(List<Player> players)
		{
		try
			{
			List<string> lines = new();
			foreach (Player player in players)
				{
				lines.Add($"{player.PlayerId},{player.PlayerName},{player.SkillLevel},{player.TotalGames},{player.TotalWins}");
				}
			File.WriteAllLines(playersCsvPath, lines);
			}
		catch (System.Exception ex)
			{
			Debug.LogError($"Error saving players to CSV: {ex.Message}");
			}
		}

	// --- Load Teams from CSV --- //
	private List<Team> LoadTeamsFromCsv()
		{
		List<Team> teams = new();
		try
			{
			if (File.Exists(teamsCsvPath))
				{
				string[] lines = File.ReadAllLines(teamsCsvPath);
				foreach (string line in lines)
					{
					string[] values = line.Split(',');
					if (values.Length == 2)  // Assuming 2 columns in the CSV (TeamId, TeamName)
						{
						// --- Fix: Ensure 'TeamId' and 'TeamName' are passed correctly --- //
						int teamId = int.Parse(values[0]);  // Extract TeamId
						string teamName = values[1];        // Extract TeamName
						Team team = new(teamId, teamName);  // Pass both parameters
						teams.Add(team);
						}
					}
				}
			}
		catch (System.Exception ex)
			{
			Debug.LogError($"Error loading teams from CSV: {ex.Message}");
			}
		return teams;
		}


	// --- Save Teams to CSV --- //
	private void SaveTeamsToCsv(List<Team> teams)
		{
		try
			{
			List<string> lines = new();
			foreach (Team team in teams)
				{
				lines.Add($"{team.TeamId},{team.TeamName}");
				}
			File.WriteAllLines(teamsCsvPath, lines);
			}
		catch (System.Exception ex)
			{
			Debug.LogError($"Error saving teams to CSV: {ex.Message}");
			}
		}
	}
