// --- Region: Database Manager --- //
using System;
using System.Collections.Generic;

using UnityEngine;

public class DatabaseManager:MonoBehaviour
	{
	#region Singleton

	public static DatabaseManager Instance { get; private set; }

	#endregion Singleton

	#region Inspector Fields

	[Header("Data")]
	[Tooltip("List of all teams in the database.")]
	public List<Team> allTeams = new();  // List of teams loaded from PlayerPrefs

	[Tooltip("List of all players in the database.")]
	public List<Player> allPlayers = new();  // List of players loaded from PlayerPrefs

	#endregion Inspector Fields

	#region Unity Methods

	private void Awake()
		{
		// Ensure only one instance of DatabaseManager exists
		if (Instance == null)
			{
			Instance = this;
			DontDestroyOnLoad(gameObject);  // Persist this object across scenes
			}
		else
			{
			Destroy(gameObject);
			return;
			}

		LoadData();  // Load teams and players from PlayerPrefs
		}

	private void OnApplicationQuit()
		{
		SaveData();  // Save data to PlayerPrefs before quitting
		}

	#endregion Unity Methods

	#region Database Methods

	public void LoadData()
		{
		// Load teams and players from PlayerPrefs
		allTeams.Clear();
		allPlayers.Clear();

		int teamCount = PlayerPrefs.GetInt("TeamCount", 0);
		for (int i = 0; i < teamCount; i++)
			{
			string teamData = PlayerPrefs.GetString("Team_" + i);
			if (!string.IsNullOrEmpty(teamData))
				{
				Team team = JsonUtility.FromJson<Team>(teamData);
				allTeams.Add(team);

				// Load players for the team
				int playerCount = PlayerPrefs.GetInt($"PlayerCount_{team.Id}", 0);
				for (int j = 0; j < playerCount; j++)
					{
					string playerData = PlayerPrefs.GetString($"Player_{team.Id}_{j}");
					if (!string.IsNullOrEmpty(playerData))
						{
						Player player = JsonUtility.FromJson<Player>(playerData);
						allPlayers.Add(player);
						}
					}
				}
			}

		Debug.Log($"Loaded {allTeams.Count} teams and {allPlayers.Count} players from PlayerPrefs.");
		}

	public void SaveData()
		{
		// Save teams and players to PlayerPrefs
		PlayerPrefs.SetInt("TeamCount", allTeams.Count);
		for (int i = 0; i < allTeams.Count; i++)
			{
			string teamData = JsonUtility.ToJson(allTeams[i]);
			PlayerPrefs.SetString("Team_" + i, teamData);

			// Save players for the team
			int playerCount = 0;
			foreach (var player in allPlayers)
				{
				if (player.TeamId == allTeams[i].Id)
					{
					string playerData = JsonUtility.ToJson(player);
					PlayerPrefs.SetString($"Player_{allTeams[i].Id}_{playerCount}", playerData);
					playerCount++;
					}
				}
			PlayerPrefs.SetInt($"PlayerCount_{allTeams[i].Id}", playerCount);
			}
		PlayerPrefs.Save();  // Save the changes to PlayerPrefs
		Debug.Log("Data saved to PlayerPrefs.");
		}

	public void AddTeam(Team team)
		{
		// Add a new team to PlayerPrefs
		if (string.IsNullOrWhiteSpace(team.Name))
			{
			Debug.LogError("Team name cannot be empty!");
			return;
			}

		allTeams.Add(team);
		SaveData();  // Save data after adding the team
		Debug.Log($"Team '{team.Name}' added to PlayerPrefs.");
		}

	public void AddPlayer(Player player)
		{
		// Add a new player to PlayerPrefs
		if (string.IsNullOrWhiteSpace(player.Name))
			{
			Debug.LogError("Player name cannot be empty!");
			return;
			}

		if (player.TeamId <= 0 || allTeams.Find(t => t.Id == player.TeamId) == null)
			{
			Debug.LogError("Player must be assigned to a valid team.");
			return;
			}

		allPlayers.Add(player);
		SaveData();  // Save data after adding the player
		Debug.Log($"Player '{player.Name}' added to PlayerPrefs.");
		}

	public void UpdateTeam(Team team)
		{
		// Update an existing team's data
		var existingTeam = allTeams.Find(t => t.Id == team.Id);
		if (existingTeam == null)
			{
			Debug.LogError("Team not found.");
			return;
			}

		// Update team data (example: update team name)
		existingTeam.Name = team.Name;
		SaveData();  // Save data after updating the team
		Debug.Log($"Team '{team.Name}' updated.");
		}

	public void SavePlayer(Player player)
		{
		// Save player to PlayerPrefs
		var existingPlayer = allPlayers.Find(p => p.Id == player.Id);
		if (existingPlayer == null)
			{
			Debug.LogError("Player not found.");
			return;
			}

		// Update player data (example: update player's stats)
		existingPlayer.Name = player.Name;
		existingPlayer.TeamId = player.TeamId;
		existingPlayer.CurrentSeasonMatchesPlayed = player.CurrentSeasonMatchesPlayed;
		existingPlayer.CurrentSeasonMatchesWon = player.CurrentSeasonMatchesWon;

		SaveData();  // Save data after updating the player
		Debug.Log($"Player '{player.Name}' updated.");
		}

	public List<Team> GetAllTeams() => allTeams;

	public Team GetTeamById(int teamId)
		{
		// Retrieve a team by its ID
		var team = allTeams.Find(t => t.Id == teamId);
		if (team == null) Debug.LogError($"Team with ID {teamId} not found.");
		return team;
		}

	public Team GetTeamByName(string teamName)
		{
		// Retrieve a team by its name (case-insensitive)
		var team = allTeams.Find(t => string.Equals(t.Name, teamName, StringComparison.OrdinalIgnoreCase));
		if (team == null) Debug.LogError($"Team '{teamName}' not found.");
		return team;
		}

	public List<Player> GetAllPlayers() => allPlayers;

	public List<Player> GetPlayersByTeam(int teamId) => allPlayers.FindAll(p => p.TeamId == teamId);

	public void RemoveTeam(Team team)
		{
		// Remove a team from PlayerPrefs
		allTeams.Remove(team);
		allPlayers.RemoveAll(p => p.TeamId == team.Id);
		SaveData();  // Save data after removing the team
		Debug.Log($"Team '{team.Name}' removed from PlayerPrefs.");
		}

	public void RemovePlayer(Player player)
		{
		// Remove a player from PlayerPrefs
		allPlayers.Remove(player);
		SaveData();  // Save data after removing the player
		Debug.Log($"Player '{player.Name}' removed from PlayerPrefs.");
		}

	#endregion Database Methods

	#region Custom Methods for Inspector Display

	[ContextMenu("Refresh Data")]
	public void RefreshData()
		{
		// Manually refresh data from PlayerPrefs
		LoadData();
		}

	[ContextMenu("Show Teams and Players")]
	public void ShowTeamsAndPlayers()
		{
		// Log teams and their associated players
		foreach (var team in allTeams)
			{
			Debug.Log($"Team: {team.Name}");
			var playersOnTeam = GetPlayersByTeam(team.Id);
			if (playersOnTeam.Count > 0)
				{
				foreach (var player in playersOnTeam)
					{
					Debug.Log($"    Player: {player.Name} | Season: {player.CurrentSeasonMatchesPlayed}/{player.CurrentSeasonMatchesWon}");
					}
				}
			else
				{
				Debug.Log("No players on this team.");
				}
			}
		}

	#endregion Custom Methods for Inspector Display
	}