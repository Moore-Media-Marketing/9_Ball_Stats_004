// --- Region: Using Directives --- //
using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
// --- End Region: Using Directives --- //


// --- Region: Class Definition --- //
public class DatabaseManager:MonoBehaviour
	{
	// --- Region: Singleton Setup --- //

	// --- Comment: Singleton instance of the DatabaseManager --- //
	public static DatabaseManager Instance { get; private set; }

	// --- Comment: Awake method to enforce the singleton pattern --- //
	private void Awake()
		{
		if (Instance == null)
			{
			Instance = this;
			// --- Comment: Persist this object between scenes --- //
			DontDestroyOnLoad(gameObject);
			}
		else
			{
			Debug.LogError("Duplicate DatabaseManager instance detected. Destroying duplicate.");
			Destroy(gameObject);
			}
		}

	// --- End Region: Singleton Setup --- //


	// --- Region: Private Data Storage --- //

	// --- Comment: Placeholder keys for PlayerPrefs storage --- //
	private const string TeamsKey = "teams_key";      // Placeholder key for teams
	private const string PlayersKey = "players_key";    // Placeholder key for players

	// --- Comment: In-memory lists for current session use (loaded from PlayerPrefs) --- //
	private List<Team> teams = new List<Team>();  // List of teams
	private List<Player> players = new List<Player>();  // List of players

	// --- End Region: Private Data Storage --- //


	// --- Region: Team Methods --- //

	// --- Comment: Returns the list of all teams --- //
	public List<Team> GetAllTeams()
		{
		// --- Comment: Load teams from PlayerPrefs if not already loaded --- //
		if (teams == null || teams.Count == 0)
			{
			LoadTeams();
			}
		return teams;
		}

	// --- Comment: Returns the list of all team names --- //
	public List<string> GetAllTeamNames()
		{
		return teams.Select(t => t.name).Distinct().ToList();
		}

	// --- Comment: Checks if a team with the given name exists (case-insensitive) --- //
	public bool TeamExists(string teamName)
		{
		return teams.Any(t => t.name.Equals(teamName, StringComparison.OrdinalIgnoreCase));
		}

	// --- Comment: Updates the team name given the old name and the new name --- //
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

	// --- Comment: Adds a new team, assigns a unique ID, and saves to PlayerPrefs --- //
	public void AddTeam(Team team)
		{
		if (team == null)
			{
			Debug.LogError("AddTeam failed: team is null.");
			return;
			}

		// --- Comment: Generate a unique ID; simple placeholder logic --- //
		team.id = teams.Count > 0 ? teams.Max(t => t.id) + 1 : 1;
		teams.Add(team);
		SaveData();
		Debug.Log("Team added: " + team.name);
		}

	// --- Comment: Removes a team (by name) and any associated players, then updates PlayerPrefs --- //
	public void RemoveTeam(string teamName)
		{
		Team team = teams.FirstOrDefault(t => t.name.Equals(teamName, StringComparison.OrdinalIgnoreCase));
		if (team != null)
			{
			teams.Remove(team);
			// --- Comment: Remove any players associated with this team --- //
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

	// --- Comment: Returns the list of all player names --- //
	public List<string> GetAllPlayerNames()
		{
		return players.Select(p => p.name).Distinct().ToList();
		}

	// --- Comment: Returns a list of players for a given team ID --- //
	public List<Player> GetPlayersByTeam(int teamId)
		{
		return players.FindAll(p => p.teamId == teamId);
		}

	// --- Comment: Adds a new player, assigns a unique ID, and saves to PlayerPrefs --- //
	public void AddPlayer(Player player)
		{
		if (player == null)
			{
			Debug.LogError("AddPlayer failed: player is null.");
			return;
			}

		// --- Comment: Generate a unique ID; simple placeholder logic --- //
		player.id = players.Count > 0 ? players.Max(p => p.id) + 1 : 1;
		players.Add(player);
		SaveData();
		Debug.Log("Player added: " + player.name);
		}

	// --- Comment: Saves or updates a player's data in PlayerPrefs --- //
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

	// --- Comment: Loads the teams list from PlayerPrefs --- //
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

	// --- Comment: Loads the players list from PlayerPrefs --- //
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

	// --- End Region: Data Loading and Saving --- //


	// --- Region: Additional Functions --- //

	// --- Comment: Saves all data (teams and players) to PlayerPrefs using JSON serialization --- //
	public void SaveData()
		{
		try
			{
			string teamsJson = JsonUtility.ToJson(new SerializationWrapper<Team>(teams));
			PlayerPrefs.SetString(TeamsKey, teamsJson);

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

	// --- End Region: Additional Functions --- //
	}
// --- End Region: Class Definition --- //


// --- Region: Serialization Wrapper Class --- //
[Serializable]
public class SerializationWrapper<T>
	{
	// --- Comment: List of items to be serialized (placeholder: target_variable) --- //
	public List<T> Items;

	// --- Comment: Constructor that wraps the provided list --- //
	public SerializationWrapper(List<T> items)
		{
		Items = items;
		}
	}
// --- End Region: Serialization Wrapper Class --- //
