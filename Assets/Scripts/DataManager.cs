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
	public List<Team> teams;
	public List<Player> players;

	private const string TeamsKey = "TeamsData";
	private const string PlayersKey = "PlayersData";

	// Event to notify about team list changes
	public event Action OnTeamListChanged;

	private void Awake()
		{
		// Initialize the singleton instance
		if (Instance == null)
			{
			Instance = this;
			DontDestroyOnLoad(gameObject); // Prevents destruction when loading new scenes
			}
		else
			{
			Destroy(gameObject); // Destroy duplicate instances
			return;
			}

		// Initialize lists
		teams = new List<Team>();
		players = new List<Player>();

		LoadData(); // Load saved data if necessary
		}

	// --- Get All Team Names --- //
	public List<string> GetTeamNames()
		{
		return teams.Select(t => t.Name).ToList(); // Get all team names
		}

	// --- Get All Teams --- //
	public List<Team> GetTeams()
		{
		return teams; // Return the list of teams
		}

	// --- Update Team --- //
	public void UpdateTeam(Team updatedTeam)
		{
		Team existingTeam = teams.FirstOrDefault(t => t.Name == updatedTeam.Name);
		if (existingTeam != null)
			{
			// Replace the old team with the updated team
			int index = teams.IndexOf(existingTeam);
			teams[index] = updatedTeam;
			SaveTeamsToPlayerPrefs(); // Save the changes to PlayerPrefs
			OnTeamListChanged?.Invoke(); // Notify listeners of the update
			}
		else
			{
			Debug.LogError($"Team with name {updatedTeam.Name} not found.");
			}
		}

	// --- Add Team --- //
	public void AddTeam(Team team)
		{
		if (!DoesTeamExist(team.Name))
			{
			teams.Add(team); // Add the new team
			SaveTeamsToPlayerPrefs(); // Save the changes to PlayerPrefs
			OnTeamListChanged?.Invoke(); // Notify listeners of the addition
			}
		else
			{
			Debug.LogError($"Team with name {team.Name} already exists.");
			}
		}

	// --- Save Teams to PlayerPrefs --- //
	public void SaveTeamsToPlayerPrefs()
		{
		List<string> teamJsonList = new();
		foreach (var team in teams)
			{
			teamJsonList.Add(JsonUtility.ToJson(team)); // Serialize each team object
			}
		PlayerPrefs.SetString(TeamsKey, JsonUtility.ToJson(new Serialization<List<string>>(teamJsonList)));
		PlayerPrefs.Save(); // Save changes to PlayerPrefs
		}

	// --- Remove Team --- //
	public void RemoveTeam(string teamName)
		{
		Team teamToRemove = teams.FirstOrDefault(t => t.Name == teamName);
		if (teamToRemove != null)
			{
			teams.Remove(teamToRemove); // Remove the team
			SaveTeamsToPlayerPrefs(); // Save the changes to PlayerPrefs
			OnTeamListChanged?.Invoke(); // Notify listeners of the removal
			}
		else
			{
			Debug.LogError($"Team with name {teamName} not found.");
			}
		}

	// --- Player Management Methods --- //
	public bool PlayerExists(string playerName)
		{
		return players.Any(p => p.name == playerName);
		}

	public void AddPlayer(Player player)
		{
		players.Add(player); // Add the new player
		SaveData(); // Save the changes to PlayerPrefs
		}

	public void RemovePlayer(string playerName)
		{
		Player playerToRemove = players.FirstOrDefault(p => p.name == playerName);
		if (playerToRemove != null)
			{
			players.Remove(playerToRemove); // Remove the player
			SaveData(); // Save the changes to PlayerPrefs
			}
		else
			{
			Debug.LogError($"Player with name {playerName} not found.");
			}
		}

	// --- Get All Player Names --- //
	public List<string> GetPlayerNames()
		{
		return players.Select(p => p.name).ToList(); // Get all player names
		}

	// --- Get Skill Level Options --- //
	public List<int> GetSkillLevelOptions()
		{
		return Enumerable.Range(1, 9).ToList(); // Skill levels are between 1 and 9
		}

	// --- Get Players for a Specific Team --- //
	public List<string> GetPlayersForTeam(string teamName)
		{
		// Assuming each team holds a list of player names in its data
		Team team = GetTeamByName(teamName);
		if (team == null)
			{
			return new List<string>(); // Return an empty list if the team doesn't exist
			}

		List<string> playerNames = team.Players.Select(p => p.name).ToList(); // Get player names for the team
		return playerNames;
		}

	// --- Team Management Methods --- //
	public Team GetTeamByName(string teamName)
		{
		return teams.FirstOrDefault(t => t.Name == teamName);
		}

	public bool DoesTeamExist(string teamName)
		{
		return teams.Any(t => t.Name == teamName);
		}

	// --- Save Data to PlayerPrefs --- //
	public void SaveData()
		{
		List<string> playerJsonList = new();
		foreach (var player in players)
			{
			playerJsonList.Add(JsonUtility.ToJson(player)); // Serialize each player object
			}
		PlayerPrefs.SetString(PlayersKey, JsonUtility.ToJson(new Serialization<List<string>>(playerJsonList)));
		PlayerPrefs.Save(); // Save changes to PlayerPrefs
		}

	// --- Load Data from PlayerPrefs --- //
	private void LoadData()
		{
		if (PlayerPrefs.HasKey(TeamsKey))
			{
			string teamsJson = PlayerPrefs.GetString(TeamsKey);
			List<string> teamJsonList = JsonUtility.FromJson<Serialization<List<string>>>(teamsJson).data;
			foreach (var teamJson in teamJsonList)
				{
				Team team = JsonUtility.FromJson<Team>(teamJson);
				teams.Add(team);
				}
			}

		if (PlayerPrefs.HasKey(PlayersKey))
			{
			string playersJson = PlayerPrefs.GetString(PlayersKey);
			List<string> playerJsonList = JsonUtility.FromJson<Serialization<List<string>>>(playersJson).data;
			foreach (var playerJson in playerJsonList)
				{
				Player player = JsonUtility.FromJson<Player>(playerJson);
				players.Add(player);
				}
			}
		}
	}

// Serialization helper class
[System.Serializable]
public class Serialization<T>
	{
	public T data;

	public Serialization(T data)
		{
		this.data = data;
		}
	}
