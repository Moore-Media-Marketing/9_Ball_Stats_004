using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;

/// <summary>
/// Manages the database of teams and players, including loading from and saving to CSV files.
/// </summary>
public class DatabaseManager:MonoBehaviour
	{
	// --- Singleton Instance ---
	public static DatabaseManager Instance { get; private set; }

	// --- File Paths ---
	private string playersDataFilePath = "Assets/PlayerData.csv";
	private string teamsDataFilePath = "Assets/TeamsData.csv";

	// --- Data Lists ---
	private List<Team> teams = new();
	private List<Player> players = new();

	// --- Initialization ---
	private void Awake()
		{
		if (Instance == null)
			{
			Instance = this;
			LoadTeamsAndPlayers();
			DontDestroyOnLoad(gameObject);
			}
		else
			{
			Destroy(gameObject);
			}
		}

	// --- Public Methods to Retrieve Data ---

	/// <summary>
	/// Returns a list of all teams.
	/// </summary>
	public List<Team> GetAllTeams()
		{
		return new List<Team>(teams);
		}

	/// <summary>
	/// Returns a list of all players.
	/// </summary>
	public List<Player> GetAllPlayers()
		{
		return new List<Player>(players);
		}

	// --- Load Methods ---
	private void LoadTeamsAndPlayers()
		{
		LoadTeams();
		LoadPlayers();
		Debug.Log($"Loaded {teams.Count} teams and {players.Count} players.");
		}

	private void LoadTeams()
		{
		teams.Clear();
		if (File.Exists(teamsDataFilePath))
			{
			string[] lines = File.ReadAllLines(teamsDataFilePath);
			foreach (string line in lines.Skip(1)) // Skip header
				{
				string[] values = line.Split(',');
				if (values.Length >= 2 && int.TryParse(values[0], out int teamId) && !string.IsNullOrEmpty(values[1]))
					{
					teams.Add(new Team(teamId, values[1]));
					}
				}
			}
		else
			{
			Debug.LogError("Teams data file not found: " + teamsDataFilePath);
			}
		}

	private void LoadPlayers()
		{
		players.Clear();
		if (File.Exists(playersDataFilePath))
			{
			string[] lines = File.ReadAllLines(playersDataFilePath);
			foreach (string line in lines.Skip(1)) // Skip header
				{
				string[] values = line.Split(',');
				if (values.Length >= 4 &&
					int.TryParse(values[0], out int playerId) &&
					!string.IsNullOrEmpty(values[1]) &&
					int.TryParse(values[2], out int teamId) &&
					int.TryParse(values[3], out int skillLevel))
					{
					PlayerStats stats = new() { CurrentSeasonSkillLevel = skillLevel };
					players.Add(new Player(playerId, values[1], teamId, stats));
					}
				}
			}
		else
			{
			Debug.LogError("Players data file not found: " + playersDataFilePath);
			}
		}

	// --- Team Management ---
	public void AddTeam(string teamName)
		{
		if (teams.Any(t => t.TeamName.Equals(teamName, StringComparison.OrdinalIgnoreCase)))
			{
			Debug.LogWarning($"Team '{teamName}' already exists.");
			return;
			}

		int newTeamId = teams.Count > 0 ? teams.Max(t => t.TeamId) + 1 : 1;
		teams.Add(new Team(newTeamId, teamName));
		SaveTeamsToCSV();
		Debug.Log($"Team '{teamName}' added with ID {newTeamId}.");
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
		SaveTeamsToCSV();
		Debug.Log($"Team ID {teamId} modified to '{newTeamName}'.");
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

		// Unassign players from the deleted team.
		foreach (var player in players.Where(p => p.TeamId == teamId))
			{
			player.TeamId = -1;  // -1 means "Unassigned"
			}

		SaveTeamsToCSV();
		SavePlayersToCSV();
		Debug.Log($"Team '{teamToDelete.TeamName}' deleted and players unassigned.");
		}

	// --- Player Management ---
	public void AddPlayer(int playerId, string playerName, int teamId, PlayerStats stats)
		{
		if (players.Any(p => p.PlayerId == playerId))
			{
			Debug.LogWarning($"Player with ID '{playerId}' already exists.");
			return;
			}

		Player newPlayer = new(playerId, playerName, teamId, stats);
		players.Add(newPlayer);
		SavePlayersToCSV();
		Debug.Log($"Player '{playerName}' added with ID {playerId}.");
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
		SavePlayersToCSV();
		Debug.Log($"Player ID {playerId} assigned to team ID {newTeamId}.");
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
		SavePlayersToCSV();
		Debug.Log($"Player '{player.PlayerName}' with ID {playerId} deleted.");
		}

	public void UpdatePlayerStats(int playerId, PlayerStats updatedStats)
		{
		Player player = players.FirstOrDefault(p => p.PlayerId == playerId);
		if (player == null)
			{
			Debug.LogWarning($"Player with ID '{playerId}' not found.");
			return;
			}

		if (updatedStats == null || !updatedStats.IsValid())
			{
			Debug.LogWarning("Invalid player stats.");
			return;
			}

		player.Stats = updatedStats;
		SavePlayersToCSV();
		Debug.Log($"Player stats updated for {player.PlayerName}.");
		}

	// --- CSV Saving Methods ---
	public void SaveTeamsToCSV()
		{
		using StreamWriter writer = new(teamsDataFilePath);
		writer.WriteLine("TeamId,TeamName");
		foreach (Team team in teams)
			{
			writer.WriteLine($"{team.TeamId},{team.TeamName}");
			}
		}

	public void SavePlayersToCSV()
		{
		using StreamWriter writer = new(playersDataFilePath);
		writer.WriteLine("PlayerId,PlayerName,TeamId,SkillLevel");
		foreach (Player player in players)
			{
			writer.WriteLine($"{player.PlayerId},{player.PlayerName},{player.TeamId},{player.Stats.CurrentSeasonSkillLevel}");
			}
		}
	}
