using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;

public class DatabaseManager:MonoBehaviour
	{
	// --- Singleton Instance ---
	public static DatabaseManager Instance { get; private set; }

	// --- File Paths ---
	private string playersDataFilePath = "Assets/PlayerData.csv";
	private string teamsDataFilePath = "Assets/TeamsData.csv";

	// --- Data Lists ---
	public List<Team> teams = new();
	public List<Player> players = new();

	// --- Initialization ---
	private void Awake()
		{
		if (Instance == null)
			{
			Instance = this;
			DontDestroyOnLoad(gameObject);
			}
		else
			{
			Destroy(gameObject);
			return;
			}

		LoadTeamsAndPlayers();
		}

	/// <summary>
	/// Loads teams and players from CSV files.
	/// </summary>
	public void LoadTeamsAndPlayers()
		{
		// Load teams
		teams = CSVManager.LoadCSV(teamsDataFilePath)
			.Select(line =>
			{
				string[] values = line.Split(',');
				if (values.Length >= 2 && int.TryParse(values[0], out int teamId))
					{
					return new Team(teamId, values[1]);
					}
				else
					{
					Debug.LogWarning($"Invalid team data: {line}");
					return null;
					}
			})
			.Where(team => team != null)
			.ToList();

		// Load players
		players = CSVManager.LoadCSV(playersDataFilePath)
			.Select(line =>
			{
				string[] values = line.Split(',');
				if (values.Length >= 4 && int.TryParse(values[0], out int playerId) && int.TryParse(values[2], out int teamId) && int.TryParse(values[3], out int skillLevel))
					{
					return new Player(playerId, values[1], teamId, new PlayerStats());
					}
				else
					{
					Debug.LogWarning($"Invalid player data: {line}");
					return null;
					}
			})
			.Where(player => player != null)
			.ToList();

		Debug.Log($"Loaded {teams.Count} teams and {players.Count} players.");
		}

	/// <summary>
	/// Adds a new team.
	/// </summary>
	public void AddTeam(string teamName)
		{
		if (teams.Any(t => t.TeamName.Equals(teamName, StringComparison.OrdinalIgnoreCase)))
			{
			Debug.LogWarning($"Team '{teamName}' already exists.");
			return;
			}

		int newTeamId = teams.Count > 0 ? teams.Max(t => t.TeamId) + 1 : 1;
		teams.Add(new Team(newTeamId, teamName));
		CSVManager.SaveTeamsToCSV(teamsDataFilePath, teams);
		}

	/// <summary>
	/// Modifies an existing team's name.
	/// </summary>
	public void ModifyTeam(int teamId, string newTeamName)
		{
		Team team = teams.FirstOrDefault(t => t.TeamId == teamId);
		if (team == null)
			{
			Debug.LogWarning("Team not found.");
			return;
			}

		team.TeamName = newTeamName;
		CSVManager.SaveTeamsToCSV(teamsDataFilePath, teams);
		}

	/// <summary>
	/// Deletes a team and assigns its players to 'Unassigned'.
	/// </summary>
	public void DeleteTeam(int teamId)
		{
		Team teamToDelete = teams.FirstOrDefault(t => t.TeamId == teamId);
		if (teamToDelete == null)
			{
			Debug.LogWarning("Team not found.");
			return;
			}

		teams.Remove(teamToDelete);

		foreach (var player in players.Where(p => p.TeamId == teamId))
			{
			player.TeamId = -1;
			}

		CSVManager.SaveTeamsToCSV(teamsDataFilePath, teams);
		CSVManager.SavePlayersToCSV(playersDataFilePath, players);
		}

	/// <summary>
	/// Adds a new player.
	/// </summary>
	public void AddPlayer(string playerName, int teamId, int skillLevel)
		{
		if (players.Any(p => p.PlayerName.Equals(playerName, StringComparison.OrdinalIgnoreCase)))
			{
			Debug.LogWarning($"Player '{playerName}' already exists.");
			return;
			}

		int newPlayerId = players.Count > 0 ? players.Max(p => p.PlayerId) + 1 : 1;
		players.Add(new Player(newPlayerId, playerName, teamId, new PlayerStats()));
		CSVManager.SavePlayersToCSV(playersDataFilePath, players);
		}

	/// <summary>
	/// Assigns a player to a new team.
	/// </summary>
	public void AssignPlayerToTeam(int playerId, int newTeamId)
		{
		Player player = players.FirstOrDefault(p => p.PlayerId == playerId);
		if (player == null)
			{
			Debug.LogWarning($"Player with ID '{playerId}' not found.");
			return;
			}

		if (!teams.Any(t => t.TeamId == newTeamId))
			{
			Debug.LogWarning($"Team with ID '{newTeamId}' does not exist.");
			return;
			}

		player.TeamId = newTeamId;
		CSVManager.SavePlayersToCSV(playersDataFilePath, players);
		}

	/// <summary>
	/// Deletes a player from the database.
	/// </summary>
	public void DeletePlayer(int playerId)
		{
		Player player = players.FirstOrDefault(p => p.PlayerId == playerId);
		if (player == null)
			{
			Debug.LogWarning($"Player with ID '{playerId}' not found.");
			return;
			}

		players.Remove(player);
		CSVManager.SavePlayersToCSV(playersDataFilePath, players);
		}

	/// <summary>
	/// Retrieves all players.
	/// </summary>
	public List<Player> GetAllPlayers() => new(players);

	/// <summary>
	/// Retrieves all teams.
	/// </summary>
	public List<Team> GetAllTeams() => new(teams);

	/// <summary>
	/// Updates the stats of a player.
	/// </summary>
	public void UpdatePlayerStats(int playerId, PlayerStats newStats)
		{
		Player player = players.FirstOrDefault(p => p.PlayerId == playerId);
		if (player == null)
			{
			Debug.LogWarning($"Player with ID '{playerId}' not found.");
			return;
			}

		player.Stats = newStats;
		CSVManager.SavePlayersToCSV(playersDataFilePath, players);
		Debug.Log($"Player stats updated for {player.PlayerName}");
		}
	}
