using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class DatabaseManager:MonoBehaviour
	{
	// --- Region: Singleton Instance ---
	public static DatabaseManager Instance { get; private set; }

	// --- Region: File Paths ---
	private string playersDataFilePath = "Assets/PlayerData.csv";

	private string teamsDataFilePath = "Assets/TeamsData.csv";

	// --- Region: Data Lists ---
	public List<Team> teams = new();

	public List<Player> players = new();

	// --- Region: Initialization ---
	private void Awake()
		{
		// Ensure that only one instance of DatabaseManager exists
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

		LoadTeamsAndPlayers();  // Load the teams and players from CSV on start
		}

	public void LoadTeamsAndPlayers()
		{
		// --- Load Teams ---
		teams = CSVManager.LoadCSV(teamsDataFilePath)
			.Select(line =>
			{
				string[] values = line.Split(',');
				if (IsValidData(values, 2) && int.TryParse(values[0], out int teamId) && !string.IsNullOrEmpty(values[1]))
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

		// --- Load Players ---
		players = CSVManager.LoadCSV(playersDataFilePath)
			.Select(line =>
			{
				string[] values = line.Split(',');
				if (IsValidData(values, 17) && int.TryParse(values[0], out int playerId) && int.TryParse(values[2], out int teamId)
					&& int.TryParse(values[3], out int skillLevel) && !string.IsNullOrEmpty(values[1]))
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

	private bool IsValidData(string[] values, int requiredLength)
		{
		return values.Length >= requiredLength && values.All(v => !string.IsNullOrWhiteSpace(v));
		}

	// --- Region: Team Management Methods ---
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
			player.TeamId = -1;  // Assign players to 'Unassigned'
			}

		CSVManager.SaveTeamsToCSV(teamsDataFilePath, teams);
		CSVManager.SavePlayersToCSV(playersDataFilePath, players);
		}

	// --- Region: Player Management Methods ---
	public void AddPlayer(string playerName, int teamId, int skillLevel, SampleDataGenerator sampleDataGenerator)
		{
		if (players.Any(p => p.PlayerName.Equals(playerName, StringComparison.OrdinalIgnoreCase)))
			{
			Debug.LogWarning($"Player '{playerName}' already exists.");
			return;
			}

		int newPlayerId = players.Count > 0 ? players.Max(p => p.PlayerId) + 1 : 1;
		Player newPlayer = new(newPlayerId, playerName, teamId, new PlayerStats());
		players.Add(newPlayer);
		sampleDataGenerator.GeneratePlayerStats(newPlayerId, skillLevel); // Generate stats for new player

		CSVManager.SavePlayersToCSV(playersDataFilePath, players);
		}

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

	// --- Region: Additional Functions ---
	public List<Player> GetAllPlayers() => new(players);

	public List<Team> GetAllTeams() => new(teams);

	public void UpdatePlayerStats(int playerId, PlayerStats newStats)
		{
		Player player = players.FirstOrDefault(p => p.PlayerId == playerId);
		if (player == null)
			{
			Debug.LogWarning($"Player with ID '{playerId}' not found.");
			return;
			}

		if (newStats == null || !newStats.IsValid())  // Assuming IsValid() exists in PlayerStats
			{
			Debug.LogWarning("Invalid player stats.");
			return;
			}

		player.Stats = newStats;
		CSVManager.SavePlayersToCSV(playersDataFilePath, players);
		Debug.Log($"Player stats updated for {player.PlayerName}");
		}
	}