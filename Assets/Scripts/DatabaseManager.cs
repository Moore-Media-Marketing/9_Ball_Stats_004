using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// --- Region: Using Directives --- //
using System.Collections;
using System.IO;
// --- End Region: Using Directives --- //

// --- Region: Class Definition --- //
public class DatabaseManager:MonoBehaviour
	{
	// --- Region: Singleton Setup --- //
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
	// --- End Region: Singleton Setup --- //

	// --- Region: Private Data Storage --- //
	private const string TeamsKey = "teams_key";    // Key for teams storage in PlayerPrefs
	private const string PlayersKey = "players_key"; // Key for players storage in PlayerPrefs

	private List<Team> teams = new();   // List of teams
	private List<Player> players = new(); // List of players
										  // --- End Region: Private Data Storage --- //

	// --- Region: Team Methods --- //

	// --- Comment: Fetch all teams from the list, loading if needed --- //
	public List<Team> GetAllTeams()
		{
		// Load teams from PlayerPrefs if not already loaded
		if (teams == null || teams.Count == 0)
			{
			LoadTeams(); // Ensure teams are loaded before use
			}
		return teams;
		}

	// --- Comment: Fetch all team names from the list --- //
	public List<string> GetAllTeamNames()
		{
		return teams.Select(t => t.name).Distinct().ToList();
		}

	// --- Comment: Get a team by its name (case-insensitive) --- //
	public Team GetTeamByName(string teamName)
		{
		return teams.FirstOrDefault(t => t.name.Equals(teamName, StringComparison.OrdinalIgnoreCase));
		}

	// --- Comment: Check if a team exists by its name --- //
	public bool TeamExists(string teamName)
		{
		return teams.Any(t => t.name.Equals(teamName, StringComparison.OrdinalIgnoreCase));
		}

	// --- Comment: Update the name of a team, saving changes --- //
	public void UpdateTeamName(string oldName, string newName)
		{
		Team team = teams.FirstOrDefault(t => t.name.Equals(oldName, StringComparison.OrdinalIgnoreCase));
		if (team != null)
			{
			team.name = newName;
			SaveData();
			Debug.Log("Team name updated to: " + newName);
			}
		else
			{
			Debug.LogWarning("Team not found for update: " + oldName);
			}
		}

	// --- Comment: Add a new team and save data --- //
	public void AddTeam(Team team)
		{
		if (team == null)
			{
			Debug.LogError("AddTeam failed: team is null.");
			return;
			}

		// Generate a unique ID
		team.id = teams.Count > 0 ? teams.Max(t => t.id) + 1 : 1;
		teams.Add(team);
		SaveData();
		Debug.Log("Team added: " + team.name);
		}

	// --- Comment: Remove a team and its associated players from the list --- //
	public void RemoveTeam(string teamName)
		{
		Team team = teams.FirstOrDefault(t => t.name.Equals(teamName, StringComparison.OrdinalIgnoreCase));
		if (team != null)
			{
			teams.Remove(team);
			// Remove associated players for this team
			players.RemoveAll(p => p.teamId == team.id);
			SaveData();
			Debug.Log("Team removed: " + team.name);
			}
		else
			{
			Debug.LogError("RemoveTeam failed: team not found - " + teamName);
			}
		}

	// --- End Region: Team Methods --- //

	// --- Region: Player Methods --- //

	// --- Comment: Get all player names from the list --- //
	public List<string> GetAllPlayerNames()
		{
		// Load players from PlayerPrefs if not already loaded
		if (players == null || players.Count == 0)
			{
			LoadPlayers(); // Ensure players are loaded before use
			}
		return players.Select(p => p.name).Distinct().ToList();
		}

	// --- Comment: Get players associated with a specific team --- //
	public List<Player> GetPlayersByTeam(int teamId)
		{
		return players.FindAll(p => p.teamId == teamId);
		}

	// --- Comment: Add a new player and save data --- //
	public void AddPlayer(Player player)
		{
		if (player == null)
			{
			Debug.LogError("AddPlayer failed: player is null.");
			return;
			}

		// Generate a unique ID
		player.id = players.Count > 0 ? players.Max(p => p.id) + 1 : 1;
		players.Add(player);
		SaveData();
		Debug.Log("Player added: " + player.name);
		}

	// --- Comment: Save player data, updating if already exists --- //
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

	// --- End Region: Player Methods --- //

	// --- Region: Data Loading and Saving --- //

	// --- Comment: Load teams from PlayerPrefs and handle errors --- //
	private void LoadTeams()
		{
		try
			{
			if (PlayerPrefs.HasKey(TeamsKey))
				{
				string json = PlayerPrefs.GetString(TeamsKey);
				SerializationWrapper<Team> wrapper = JsonUtility.FromJson<SerializationWrapper<Team>>(json);
				teams = wrapper.Items ?? new List<Team>();
				}
			else
				{
				teams = new List<Team>();
				}
			Debug.Log("Teams loaded from PlayerPrefs.");
			}
		catch (Exception ex)
			{
			Debug.LogError("Error loading teams: " + ex.Message);
			teams = new List<Team>();
			}
		}

	// --- Comment: Load players from PlayerPrefs and handle errors --- //
	private void LoadPlayers()
		{
		try
			{
			if (PlayerPrefs.HasKey(PlayersKey))
				{
				string json = PlayerPrefs.GetString(PlayersKey);
				SerializationWrapper<Player> wrapper = JsonUtility.FromJson<SerializationWrapper<Player>>(json);
				players = wrapper.Items ?? new List<Player>();
				}
			else
				{
				players = new List<Player>();
				}
			Debug.Log("Players loaded from PlayerPrefs.");
			}
		catch (Exception ex)
			{
			Debug.LogError("Error loading players: " + ex.Message);
			players = new List<Player>();
			}
		}

	// --- Comment: Save all teams and players to PlayerPrefs --- //
	public void SaveData()
		{
		try
			{
			// Save teams data
			string teamsJson = JsonUtility.ToJson(new SerializationWrapper<Team>(teams));
			PlayerPrefs.SetString(TeamsKey, teamsJson);

			// Save players data
			string playersJson = JsonUtility.ToJson(new SerializationWrapper<Player>(players));
			PlayerPrefs.SetString(PlayersKey, playersJson);

			PlayerPrefs.Save();
			Debug.Log("All data saved to PlayerPrefs.");
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

// --- End Region: Class Definition --- //

// --- Region: Serialization Wrapper Class --- //
[Serializable]
public class SerializationWrapper<T>
	{
	public List<T> Items;

	public SerializationWrapper(List<T> items)
		{
		Items = items;
		}
	}
// --- End Region: Serialization Wrapper Class --- //
