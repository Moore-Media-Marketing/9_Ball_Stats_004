using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

[System.Serializable]
public class DataManager:MonoBehaviour
	{
	// Singleton for DataManager
	public static DataManager Instance { get; private set; }

	// List of teams and players
	public List<Team> teams = new();
	public List<Player> players = new();

	// Event to notify when the team list changes
	public event Action OnTeamListChanged;

	// Event to notify when the player data is updated (e.g., added or removed)
	public event Action OnPlayerDataUpdated;

	// Event to notify when the team data is updated (e.g., added or removed)
	public event Action OnTeamDataUpdated;

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

		LoadData();
		}

	// --- Load Data from PlayerPrefs --- //
	private void LoadData()
		{
		// Load teams from PlayerPrefs
		int teamCount = PlayerPrefs.GetInt("TeamCount", 0);
		Debug.Log($"Loaded {teamCount} teams."); // Debugging line to check loaded teams

		for (int i = 0; i < teamCount; i++)
			{
			string teamName = PlayerPrefs.GetString($"Team_{i}_Name", string.Empty);
			if (!string.IsNullOrEmpty(teamName))
				{
				Team team = new(teamName); // Assuming Team has a constructor that accepts a name
				teams.Add(team);
				Debug.Log($"Loaded team: {teamName}"); // Debugging line to check loaded team
				}
			}

		// Load players from PlayerPrefs
		int playerCount = PlayerPrefs.GetInt("PlayerCount", 0);
		for (int i = 0; i < playerCount; i++)
			{
			string playerName = PlayerPrefs.GetString($"Player_{i}_Name", string.Empty);
			int skillLevel = PlayerPrefs.GetInt($"Player_{i}_SkillLevel", 1); // Default to skill level 1 if not found
			int gamesPlayed = PlayerPrefs.GetInt($"Player_{i}_GamesPlayed", 0); // Default to 0 if not found
			int gamesWon = PlayerPrefs.GetInt($"Player_{i}_GamesWon", 0); // Default to 0 if not found
			string teamName = PlayerPrefs.GetString($"Player_{i}_TeamName", string.Empty); // Team name (empty if no team assigned)

			// Validate loaded player data
			if (!string.IsNullOrEmpty(playerName))
				{
				Player player = new(playerName, skillLevel, gamesPlayed, gamesWon, teamName);
				players.Add(player);
				}
			}
		}

	// --- Save Teams and Player Data to PlayerPrefs --- //
	public void SaveData()
		{
		SaveDataToPlayerPrefs();
		}

	public void SaveDataToPlayerPrefs()
		{
		// Save teams to PlayerPrefs
		PlayerPrefs.SetInt("TeamCount", teams.Count);
		for (int i = 0; i < teams.Count; i++)
			{
			PlayerPrefs.SetString($"Team_{i}_Name", teams[i].Name);
			}

		// Save players to PlayerPrefs
		PlayerPrefs.SetInt("PlayerCount", players.Count);
		for (int i = 0; i < players.Count; i++)
			{
			PlayerPrefs.SetString($"Player_{i}_Name", players[i].name);
			PlayerPrefs.SetInt($"Player_{i}_SkillLevel", players[i].skillLevel);
			PlayerPrefs.SetInt($"Player_{i}_GamesPlayed", players[i].gamesPlayed);
			PlayerPrefs.SetInt($"Player_{i}_GamesWon", players[i].gamesWon);
			PlayerPrefs.SetString($"Player_{i}_TeamName", players[i].TeamName);
			}

		// Save player data to PlayerPrefs
		PlayerPrefs.Save();
		}

	// --- Get players for a specific team --- //
	public List<Player> GetPlayersForTeam(string teamName)
		{
		return players.Where(p => p.TeamName == teamName).ToList();
		}

	// --- Check if a player exists by name --- //
	public bool PlayerExists(string playerName)
		{
		return players.Any(p => p.name == playerName);
		}

	// --- Get team names --- //
	public List<string> GetTeamNames()
		{
		return teams.Select(t => t.Name).ToList();
		}

	// --- Get player names --- //
	public List<string> GetPlayerNames()
		{
		return players.Select(p => p.name).ToList();
		}

	// --- Get skill levels --- //
	public List<int> GetSkillLevelOptions()
		{
		return new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
		}

	// --- Get team by name --- //
	public Team GetTeamByName(string name)
		{
		return teams.FirstOrDefault(t => t.Name == name);
		}

	// --- Add team --- //
	public void AddTeam(Team team)
		{
		teams.Add(team);
		OnTeamListChanged?.Invoke();  // Trigger the event for team list change
		OnTeamDataUpdated?.Invoke();  // Trigger the event for team data update
		SaveData();  // Save teams and players to PlayerPrefs after adding
		}

	// --- Remove team --- //
	public void RemoveTeam(Team team)
		{
		teams.Remove(team);
		OnTeamListChanged?.Invoke();  // Trigger the event for team list change
		OnTeamDataUpdated?.Invoke();  // Trigger the event for team data update
		SaveData();  // Save teams and players to PlayerPrefs after removing
		}

	// --- Add player --- //
	public void AddPlayer(Player player)
		{
		players.Add(player);
		OnPlayerDataUpdated?.Invoke();  // Trigger the event for player data update
		}

	// --- Remove player --- //
	public void RemovePlayer(Player player)
		{
		players.Remove(player);
		OnPlayerDataUpdated?.Invoke();  // Trigger the event for player data update
		}
	}
