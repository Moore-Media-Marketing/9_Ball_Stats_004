using UnityEngine;
using System.Collections.Generic;

public class DataManager:MonoBehaviour
	{
	private const string TEAM_DATA_KEY = "teams";  // Key for storing team data in PlayerPrefs
	private const string PLAYER_DATA_KEY = "players";  // Key for storing player data in PlayerPrefs

	private List<Team> teams = new List<Team>();  // List of all teams
	private Dictionary<string, Player> players = new Dictionary<string, Player>();  // Dictionary for quick access to players by name

	// --- Start ---
	private void Start()
		{
		LoadData();  // Load data on start
		}

	// --- Save Data ---
	public void SaveData()
		{
		// Save teams
		string teamDataJson = JsonUtility.ToJson(new TeamDataWrapper { teams = teams });
		PlayerPrefs.SetString(TEAM_DATA_KEY, teamDataJson);

		// Save players
		string playerDataJson = JsonUtility.ToJson(new PlayerDataWrapper { players = new List<Player>(players.Values) });
		PlayerPrefs.SetString(PLAYER_DATA_KEY, playerDataJson);

		PlayerPrefs.Save();  // Save changes to PlayerPrefs
		}

	// --- Load Data ---
	public void LoadData()
		{
		// Load teams
		if (PlayerPrefs.HasKey(TEAM_DATA_KEY))
			{
			string teamDataJson = PlayerPrefs.GetString(TEAM_DATA_KEY);
			TeamDataWrapper teamDataWrapper = JsonUtility.FromJson<TeamDataWrapper>(teamDataJson);
			teams = teamDataWrapper.teams;
			}

		// Load players
		if (PlayerPrefs.HasKey(PLAYER_DATA_KEY))
			{
			string playerDataJson = PlayerPrefs.GetString(PLAYER_DATA_KEY);
			PlayerDataWrapper playerDataWrapper = JsonUtility.FromJson<PlayerDataWrapper>(playerDataJson);
			foreach (Player player in playerDataWrapper.players)
				{
				players[player.name] = player;
				}
			}
		}

	// --- Add Team ---
	public void AddTeam(string teamName)
		{
		// Check if team already exists
		if (teams.Exists(t => t.name == teamName))
			{
			Debug.LogError("Team already exists.");
			return;
			}

		// Create and add new team
		Team newTeam = new Team { name = teamName };
		teams.Add(newTeam);

		SaveData();  // Save updated data
		}

	// --- Remove Team ---
	public void RemoveTeam(string teamName)
		{
		Team teamToRemove = teams.Find(t => t.name == teamName);

		if (teamToRemove != null)
			{
			teams.Remove(teamToRemove);
			SaveData();  // Save updated data
			}
		else
			{
			Debug.LogError("Team not found.");
			}
		}

	// --- Add Player ---
	public void AddPlayer(string teamName, string playerName, int skillLevel)
		{
		// Check if player already exists
		if (players.ContainsKey(playerName))
			{
			Debug.LogError("Player already exists.");
			return;
			}

		Player newPlayer = new Player
			{
			name = playerName,
			skillLevel = skillLevel,
			matchesWon = 0,
			matchesPlayed = 0
			};

		players[playerName] = newPlayer;

		// Add player to the team
		Team team = teams.Find(t => t.name == teamName);
		if (team != null)
			{
			team.players.Add(newPlayer);
			}

		SaveData();  // Save updated data
		}

	// --- Remove Player ---
	public void RemovePlayer(string teamName, string playerName)
		{
		// Remove player from the players dictionary
		if (players.ContainsKey(playerName))
			{
			players.Remove(playerName);
			}

		// Remove player from the team
		Team team = teams.Find(t => t.name == teamName);
		if (team != null)
			{
			Player playerToRemove = team.players.Find(p => p.name == playerName);
			if (playerToRemove != null)
				{
				team.players.Remove(playerToRemove);
				}
			}

		SaveData();  // Save updated data
		}

	// --- Get All Teams ---
	public List<Team> GetAllTeams()
		{
		return teams;
		}

	// --- Get Player By Name ---
	public Player GetPlayerByName(string playerName)
		{
		if (players.ContainsKey(playerName))
			{
			return players[playerName];
			}

		return null;
		}
	}

// Wrapper classes for JSON serialization/deserialization
[System.Serializable]
public class Team
	{
	public string name;  // Team name
	public List<Player> players = new List<Player>();  // List of players in the team
	}

[System.Serializable]
public class Player
	{
	public string name;  // Player name
	public int skillLevel;  // Player skill level
	public int matchesWon;  // Matches won by player
	public int matchesPlayed;  // Matches played by player
	public float winPercentage => matchesPlayed > 0 ? (float) matchesWon / matchesPlayed * 100 : 0;  // Calculate win percentage
	}

[System.Serializable]
public class TeamDataWrapper
	{
	public List<Team> teams;  // List of teams
	}

[System.Serializable]
public class PlayerDataWrapper
	{
	public List<Player> players;  // List of players
	}
