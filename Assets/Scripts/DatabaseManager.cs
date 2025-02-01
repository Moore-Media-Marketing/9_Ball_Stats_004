// --- Region: Using Directives --- //
using System;
using System.Collections.Generic;

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
	private List<Team> teams = new();

	private List<Player> players = new();

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

	// --- Comment: Adds a new team, assigns a unique ID, and saves to PlayerPrefs --- //
	public void AddTeam(Team team)
		{
		if (team == null)
			{
			Debug.LogError("AddTeam failed: team is null.");
			return;
			}
		team.id = teams.Count + 1; // Placeholder for unique ID generation
		teams.Add(team);
		SaveTeams();
		Debug.Log("Team added: " + team.name);
		}

	// --- Comment: Removes a team and any associated players, then updates PlayerPrefs --- //
	public void RemoveTeam(Team team)
		{
		if (team == null)
			{
			Debug.LogError("RemoveTeam failed: team is null.");
			return;
			}
		teams.Remove(team);
		// --- Comment: Remove any players associated with this team --- //
		players.RemoveAll(p => p.teamId == team.id);
		SaveTeams();
		SavePlayers();
		Debug.Log("Team removed: " + team.name);
		}

	// --- Comment: Saves the teams list to PlayerPrefs using JSON serialization --- //
	public void SaveTeams()
		{
		try
			{
			// --- Comment: Convert teams list to JSON --- //
			string json = JsonUtility.ToJson(new SerializationWrapper<Team>(teams));
			PlayerPrefs.SetString(TeamsKey, json);
			PlayerPrefs.Save();
			Debug.Log("Teams saved to PlayerPrefs.");
			}
		catch (System.Exception ex)
			{
			Debug.LogError("Error saving teams: " + ex.Message);
			}
		}

	// --- Comment: Loads the teams list from PlayerPrefs --- //
	public void LoadTeams()
		{
		try
			{
			if (PlayerPrefs.HasKey(TeamsKey))
				{
				string json = PlayerPrefs.GetString(TeamsKey);
				SerializationWrapper<Team> wrapper = JsonUtility.FromJson<SerializationWrapper<Team>>(json);
				teams = wrapper.Items;
				}
			else
				{
				teams = new List<Team>();
				}
			Debug.Log("Teams loaded from PlayerPrefs.");
			}
		catch (System.Exception ex)
			{
			Debug.LogError("Error loading teams: " + ex.Message);
			teams = new List<Team>();
			}
		}

	// --- End Region: Team Methods --- //

	// --- Region: Player Methods --- //

	// --- Comment: Adds a new player, assigns a unique ID, and saves to PlayerPrefs --- //
	public void AddPlayer(Player player)
		{
		if (player == null)
			{
			Debug.LogError("AddPlayer failed: player is null.");
			return;
			}
		player.id = players.Count + 1; // Placeholder for unique ID generation
		players.Add(player);
		SavePlayers();

		// --- Comment: Optionally add the player to the team's internal list --- //
		Team team = teams.Find(t => t.id == player.teamId);
		team?.players.Add(player);
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
		SavePlayers();
		Debug.Log("Player data saved: " + player.name);
		}

	// --- Comment: Returns a list of players for a given team ID --- //
	public List<Player> GetPlayersByTeam(int teamId)
		{
		// --- Comment: Ensure players are loaded --- //
		if (players == null || players.Count == 0)
			{
			LoadPlayers();
			}
		return players.FindAll(p => p.teamId == teamId);
		}

	// --- Comment: Saves the players list to PlayerPrefs using JSON serialization --- //
	public void SavePlayers()
		{
		try
			{
			string json = JsonUtility.ToJson(new SerializationWrapper<Player>(players));
			PlayerPrefs.SetString(PlayersKey, json);
			PlayerPrefs.Save();
			Debug.Log("Players saved to PlayerPrefs.");
			}
		catch (System.Exception ex)
			{
			Debug.LogError("Error saving players: " + ex.Message);
			}
		}

	// --- Comment: Loads the players list from PlayerPrefs --- //
	public void LoadPlayers()
		{
		try
			{
			if (PlayerPrefs.HasKey(PlayersKey))
				{
				string json = PlayerPrefs.GetString(PlayersKey);
				SerializationWrapper<Player> wrapper = JsonUtility.FromJson<SerializationWrapper<Player>>(json);
				players = wrapper.Items;
				}
			else
				{
				players = new List<Player>();
				}
			Debug.Log("Players loaded from PlayerPrefs.");
			}
		catch (System.Exception ex)
			{
			Debug.LogError("Error loading players: " + ex.Message);
			players = new List<Player>();
			}
		}

	// --- End Region: Player Methods --- //

	// --- Region: Additional Functions --- //

	// --- Comment: Saves all data (teams and players) to PlayerPrefs --- //
	public void SaveData()
		{
		SaveTeams();
		SavePlayers();
		Debug.Log("All data saved to PlayerPrefs.");
		}

	// --- End Region: Additional Functions --- //
	}

// --- End Region: Class Definition --- //

// --- Region: Serialization Wrapper Class --- //
[Serializable]
public class SerializationWrapper<T>
	{
	// --- Region: Wrapper Data --- //
	// --- Comment: List of items to be serialized --- //
	public List<T> Items;

	// --- End Region: Wrapper Data --- //

	// --- Region: Constructor --- //
	// --- Comment: Constructor that wraps the provided list (placeholder: target_variable) --- //
	public SerializationWrapper(List<T> items)
		{
		Items = items;
		}

	// --- End Region: Constructor --- //
	}

// --- End Region: Serialization Wrapper Class --- //