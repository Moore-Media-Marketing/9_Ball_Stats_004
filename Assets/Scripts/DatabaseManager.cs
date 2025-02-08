using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;

public class DatabaseManager:MonoBehaviour
	{
	public static DatabaseManager Instance { get; private set; }

	private string playersDataFilePath = "Assets/PlayerData.csv";
	private string teamsDataFilePath = "Assets/TeamsData.csv";

	private List<Team> teams = new();
	private List<Player> players = new();

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
			}
		}

	// --- Generate Sample Data --- //
	public void GenerateSampleData()
		{
		Debug.Log("Generating sample data...");

		// Create sample teams
		teams = new List<Team>
		{
			new(1, "Team Alpha"),
			new(2, "Team Bravo"),
			new(3, "Team Charlie")
		};

		// Create sample players
		players = new List<Player>
		{
			new("Alice", "Team Alpha", 5, new PlayerStats(), 1),
			new("Bob", "Team Bravo", 6, new PlayerStats(), 2),
			new("Charlie", "Team Charlie", 4, new PlayerStats(), 3),
			new("David", "Team Alpha", 7, new PlayerStats(), 1),
			new("Eve", "Team Bravo", 5, new PlayerStats(), 2)
		};

		// Save to CSV
		SaveTeams(teams);
		SavePlayersToCsv(players);

		Debug.Log("Sample data generated successfully.");
		}

	// --- Clear Sample Data --- //
	public void ClearSampleData()
		{
		Debug.Log("Clearing sample data...");

		teams.Clear();
		players.Clear();

		// Save empty lists to CSV to clear them
		SaveTeams(teams);
		SavePlayersToCsv(players);

		Debug.Log("Sample data cleared.");
		}

	// --- Delete Team by TeamId --- //
	public void DeleteTeam(int teamId)
		{
		// Ensure teams are loaded before deleting
		if (teams == null || teams.Count == 0)
			{
			teams = LoadTeams(); // Load teams if they haven't been loaded already
			}

		// Find the team to delete
		Team teamToDelete = teams.FirstOrDefault(t => t.TeamId == teamId);
		if (teamToDelete != null)
			{
			// Remove the team from the list
			teams.Remove(teamToDelete);

			// Also need to remove players in the deleted team from the players list
			players.RemoveAll(player => player.TeamId == teamId);

			// Save updated teams and players to CSV
			SaveTeams(teams);
			SavePlayersToCsv(players);

			Debug.Log($"Team '{teamToDelete.TeamName}' deleted.");
			}
		else
			{
			Debug.LogWarning($"No team found with TeamId: {teamId}");
			}
		}

	// --- Save Teams to CSV --- //
	public void SaveTeams(List<Team> teams)
		{
		List<string> lines = new()
			{
			"TeamId,TeamName" // CSV Header
        };

		foreach (var team in teams)
			{
			lines.Add($"{team.TeamId},{team.TeamName}");
			}

		File.WriteAllLines(teamsDataFilePath, lines);
		Debug.Log("Teams saved to CSV.");
		}

	// --- Save Players to CSV --- //
	public void SavePlayersToCsv(List<Player> players)
		{
		List<string> lines = new()
			{
			"PlayerName,TeamName,SkillLevel,LifetimeGamesWon,LifetimeGamesPlayed" // CSV Header
        };

		foreach (var player in players)
			{
			lines.Add($"{player.PlayerName},{player.TeamName},{player.SkillLevel}," +
					  $"{player.Stats.LifetimeGamesWon},{player.Stats.LifetimeGamesPlayed}");
			}

		File.WriteAllLines(playersDataFilePath, lines);
		Debug.Log("Players saved to CSV.");
		}

	// --- Load All Teams from CSV --- //
	public List<Team> LoadTeams()
		{
		if (!File.Exists(teamsDataFilePath))
			{
			Debug.LogWarning("Teams CSV not found, returning empty list.");
			return new List<Team>();
			}

		var lines = File.ReadAllLines(teamsDataFilePath).Skip(1); // Skip header
		List<Team> loadedTeams = new();

		foreach (var line in lines)
			{
			var data = line.Split(',');
			int teamId = int.Parse(data[0]);
			string teamName = data[1];

			loadedTeams.Add(new Team(teamId, teamName));
			}

		Debug.Log("Teams loaded from CSV.");
		return loadedTeams;
		}

	// --- Load All Players from CSV --- //
	public List<Player> LoadPlayersFromCsv()
		{
		if (!File.Exists(playersDataFilePath))
			{
			Debug.LogWarning("Players CSV not found, returning empty list.");
			return new List<Player>();
			}

		var lines = File.ReadAllLines(playersDataFilePath).Skip(1); // Skip header
		List<Player> loadedPlayers = new();

		foreach (var line in lines)
			{
			var data = line.Split(',');
			string playerName = data[0];
			string teamName = data[1];
			int skillLevel = int.Parse(data[2]);
			int lifetimeGamesWon = int.Parse(data[3]);
			int lifetimeGamesPlayed = int.Parse(data[4]);

			PlayerStats stats = new()
				{
				LifetimeGamesWon = lifetimeGamesWon,
				LifetimeGamesPlayed = lifetimeGamesPlayed
				};

			int teamId = teams.FirstOrDefault(t => t.TeamName == teamName)?.TeamId ?? -1;

			loadedPlayers.Add(new Player(playerName, teamName, skillLevel, stats, teamId));
			}

		Debug.Log("Players loaded from CSV.");
		return loadedPlayers;
		}

	// --- Load Players by Team from CSV (New method) --- //
	public List<Player> LoadPlayersByTeam(int teamId)
		{
		// Ensure players are loaded before filtering
		if (players == null || players.Count == 0)
			{
			players = LoadPlayersFromCsv(); // Load players if they haven't been loaded already
			}

		// Filter players by teamId
		List<Player> filteredPlayers = players.Where(p => p.TeamId == teamId).ToList();

		Debug.Log($"Loaded {filteredPlayers.Count} players for TeamId: {teamId}");
		return filteredPlayers;
		}
	}
