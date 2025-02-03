using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class DatabaseManager:MonoBehaviour
	{
	public static DatabaseManager Instance { get; private set; }

	private void Awake()
		{
		if (Instance == null)
			{
			Instance = this;
			DontDestroyOnLoad(gameObject); // Keep this instance across scenes
			}
		else
			{
			Debug.LogError("Duplicate DatabaseManager instance detected. Destroying duplicate.");
			Destroy(gameObject);
			}
		}

	private const string TeamsKey = "teams_key";
	private const string PlayersKey = "players_key";

	private List<Team> teams = new();
	private List<Player> players = new();

	// --- Region: Team Methods --- //
	public List<Team> GetAllTeams()
		{
		if (teams == null || teams.Count == 0)
			{
			LoadTeams();
			}
		return teams;
		}

	public List<string> GetAllTeamNames()
		{
		return teams.Select(t => t.name).Distinct().ToList();
		}

	public Team GetTeamByName(string teamName)
		{
		return teams.FirstOrDefault(t => t.name.Equals(teamName, StringComparison.OrdinalIgnoreCase));
		}

	public bool TeamExists(string teamName)
		{
		return teams.Any(t => t.name.Equals(teamName, StringComparison.OrdinalIgnoreCase));
		}

	public void AddTeam(Team team)
		{
		if (team == null)
			{
			Debug.LogError("AddTeam failed: team is null.");
			return;
			}

		team.id = teams.Count > 0 ? teams.Max(t => t.id) + 1 : 1;
		teams.Add(team);
		SaveData();
		Debug.Log("Team added: " + team.name);
		}

	public void RemoveTeam(string teamName)
		{
		Team team = teams.FirstOrDefault(t => t.name.Equals(teamName, StringComparison.OrdinalIgnoreCase));
		if (team != null)
			{
			teams.Remove(team);
			players.RemoveAll(p => p.teamId == team.id);
			SaveData();
			Debug.Log("Team removed: " + team.name);
			}
		else
			{
			Debug.LogError("RemoveTeam failed: team not found - " + teamName);
			}
		}

	public void UpdateTeamName(int teamId, string newTeamName)
		{
		Team teamToUpdate = teams.FirstOrDefault(t => t.id == teamId);
		if (teamToUpdate != null)
			{
			teamToUpdate.name = newTeamName;
			SaveData(); // Save after updating the team name
			Debug.Log("Updated team name to: " + newTeamName);
			}
		else
			{
			Debug.LogError("UpdateTeamName failed: team with ID " + teamId + " not found.");
			}
		}
	// --- End Region: Team Methods --- //

	// --- Region: Player Methods --- //
	public List<string> GetAllPlayerNames()
		{
		if (players == null || players.Count == 0)
			{
			LoadPlayers();
			}
		return players.Select(p => p.name).Distinct().ToList();
		}

	// --- Add GetAllPlayers Method --- //
	public List<Player> GetAllPlayers()
		{
		if (players == null || players.Count == 0)
			{
			LoadPlayers();  // Ensure players are loaded before returning
			}
		return players;
		}

	public List<Player> GetPlayersByTeam(int teamId)
		{
		return players.FindAll(p => p.teamId == teamId);
		}

	public void AddPlayer(Player player)
		{
		if (player == null)
			{
			Debug.LogError("AddPlayer failed: player is null.");
			return;
			}

		player.id = players.Count > 0 ? players.Max(p => p.id) + 1 : 1;
		players.Add(player);
		SaveData();
		Debug.Log("Player added: " + player.name);
		}

	public void SavePlayer(Player player)
		{
		if (player == null)
			{
			Debug.LogError("SavePlayer failed: player is null.");
			return;
			}

		int index = players.FindIndex(p => p.id == player.id);
		if (index >= 0)
			{
			players[index] = player;
			}
		else
			{
			players.Add(player);
			}
		SaveData();
		Debug.Log("Player data saved: " + player.name);
		}

	public Player GetPlayerByName(string playerName)
		{
		if (string.IsNullOrEmpty(playerName))
			{
			Debug.LogError("GetPlayerByName failed: playerName is null or empty.");
			return null;
			}

		if (players == null || players.Count == 0)
			{
			LoadPlayers(); // Ensure players are loaded before searching
			}

		Player foundPlayer = players.FirstOrDefault(p => p.name.Equals(playerName, StringComparison.OrdinalIgnoreCase));

		if (foundPlayer != null)
			{
			Debug.Log("Player found: " + foundPlayer.name);
			}
		else
			{
			Debug.LogWarning("Player not found: " + playerName);
			}

		return foundPlayer;
		}

	// --- End Region: Player Methods --- //

	// --- Region: Data Loading and Saving --- //
	private void LoadTeams()
		{
		teams.Clear();
		for (int i = 1; i <= 100; i++) // Assuming max 100 teams for simplicity
			{
			if (PlayerPrefs.HasKey($"Team_{i}"))
				{
				string json = PlayerPrefs.GetString($"Team_{i}");
				Team team = JsonUtility.FromJson<Team>(json);
				teams.Add(team);
				}
			}
		Debug.Log($"Loaded {teams.Count} teams from PlayerPrefs.");
		}

	private void LoadPlayers()
		{
		players.Clear();
		for (int i = 1; i <= 500; i++) // Assuming max 500 players for simplicity
			{
			if (PlayerPrefs.HasKey($"Player_{i}"))
				{
				string json = PlayerPrefs.GetString($"Player_{i}");
				Player player = JsonUtility.FromJson<Player>(json);
				players.Add(player);
				}
			}
		Debug.Log($"Loaded {players.Count} players from PlayerPrefs.");
		}

	public void SaveData()
		{
		try
			{
			// Save Teams Individually
			foreach (Team team in teams)
				{
				string teamJson = JsonUtility.ToJson(team);
				PlayerPrefs.SetString($"Team_{team.id}", teamJson);
				}

			// Save Players Individually
			foreach (Player player in players)
				{
				string playerJson = JsonUtility.ToJson(player);
				PlayerPrefs.SetString($"Player_{player.id}", playerJson);
				}

			PlayerPrefs.Save();
			Debug.Log("All data saved to PlayerPrefs in individual keys.");
			}
		catch (Exception ex)
			{
			Debug.LogError("Error saving data: " + ex.Message);
			}
		}
	// --- End Region: Data Loading and Saving --- //

	// --- Region: Additional Functions --- //

	// --- End Region: Additional Functions --- //
	}
