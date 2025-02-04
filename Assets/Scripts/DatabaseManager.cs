using System;
using System.Collections.Generic;
using System.IO;
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

	private const string TeamsFilePath = "teams.json";
	private const string PlayersFilePath = "players.json";

	private List<Team> teams = new();
	private List<Player> players = new();

	// --- Region: Team Methods --- //
	public List<string> GetAllTeamNames()
		{
		if (teams == null || teams.Count == 0)
			{
			LoadTeams(); // Load teams if they are not loaded
			}
		return teams.Select(t => t.name).Distinct().ToList(); // Get distinct team names
		}

	public List<Team> GetAllTeams()
		{
		if (teams == null || teams.Count == 0)
			{
			LoadTeams();  // Ensure teams are loaded before returning
			}
		return teams;
		}

	public Team GetTeamByName(string teamName)
		{
		return teams.FirstOrDefault(t => t.name.Equals(teamName, StringComparison.OrdinalIgnoreCase)); // Find team by name
		}

	public void AddTeam(Team team)
		{
		if (team == null)
			{
			Debug.LogError("AddTeam failed: team is null.");
			return;
			}

		teams.Add(team);
		SaveTeams(); // Save after adding team
		Debug.Log("Team added: " + team.name);
		}

	public void RemoveTeam(string teamName)
		{
		Team team = teams.FirstOrDefault(t => t.name.Equals(teamName, StringComparison.OrdinalIgnoreCase));
		if (team != null)
			{
			teams.Remove(team);
			SaveTeams(); // Save after removing team
			Debug.Log("Team removed: " + team.name);
			}
		else
			{
			Debug.LogError("RemoveTeam failed: team not found - " + teamName);
			}
		}

	public void UpdateTeamName(string oldName, string newName)
		{
		Team teamToUpdate = teams.FirstOrDefault(t => t.name.Equals(oldName, StringComparison.OrdinalIgnoreCase));
		if (teamToUpdate != null)
			{
			teamToUpdate.name = newName;
			SaveTeams(); // Save after updating team name
			Debug.Log("Updated team name to: " + newName);
			}
		else
			{
			Debug.LogError("UpdateTeamName failed: team with name " + oldName + " not found.");
			}
		}

	// New SaveTeams Method --- //
	public void SaveTeams()
		{
		try
			{
			string teamJson = JsonUtility.ToJson(new TeamListWrapper(teams)); // Save the list of teams
			File.WriteAllText(TeamsFilePath, teamJson); // Save teams to JSON file
			Debug.Log("Teams saved successfully.");
			}
		catch (Exception ex)
			{
			Debug.LogError("Error saving teams: " + ex.Message);
			}
		}

	// --- End Region --- //

	// --- Region: Player Methods --- //
	public List<string> GetAllPlayerNames()
		{
		if (players == null || players.Count == 0)
			{
			LoadPlayers(); // Load players if they are not loaded
			}
		return players.Select(p => p.name).Distinct().ToList(); // Get distinct player names
		}

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
		return players.Where(p => p.teamId == teamId).ToList(); // Get players by team ID
		}

	public void AddPlayer(Player player)
		{
		if (player == null)
			{
			Debug.LogError("AddPlayer failed: player is null.");
			return;
			}

		players.Add(player);
		SaveData(); // Save data after adding player
		Debug.Log("Player added: " + player.name);
		}

	public void RemovePlayer(string playerName)
		{
		Player player = players.FirstOrDefault(p => p.name.Equals(playerName, StringComparison.OrdinalIgnoreCase));
		if (player != null)
			{
			players.Remove(player);
			SaveData(); // Save data after removing player
			Debug.Log("Player removed: " + player.name);
			}
		else
			{
			Debug.LogError("RemovePlayer failed: player not found - " + playerName);
			}
		}

	public void UpdatePlayerName(string oldName, string newName)
		{
		Player playerToUpdate = players.FirstOrDefault(p => p.name.Equals(oldName, StringComparison.OrdinalIgnoreCase));
		if (playerToUpdate != null)
			{
			playerToUpdate.name = newName;
			SaveData(); // Save data after updating player name
			Debug.Log("Updated player name to: " + newName);
			}
		else
			{
			Debug.LogError("UpdatePlayerName failed: player with name " + oldName + " not found.");
			}
		}

	// New SavePlayer Method --- //
	public void SavePlayer(Player player)
		{
		if (player == null)
			{
			Debug.LogError("SavePlayer failed: player is null.");
			return;
			}

		// Check if player already exists, then update or add
		Player existingPlayer = players.FirstOrDefault(p => p.name.Equals(player.name, StringComparison.OrdinalIgnoreCase));
		if (existingPlayer != null)
			{
			existingPlayer = player; // Update player
			Debug.Log("Player updated: " + player.name);
			}
		else
			{
			players.Add(player); // Add new player
			Debug.Log("New player added: " + player.name);
			}

		SaveData(); // Save after updating or adding player
		}
	// --- End Region --- //

	// --- Region: Save Data --- //
	private void SaveData()
		{
		try
			{
			string teamJson = JsonUtility.ToJson(new TeamListWrapper(teams)); // Save the list of teams
			File.WriteAllText(TeamsFilePath, teamJson); // Save teams to JSON file

			string playerJson = JsonUtility.ToJson(new PlayerListWrapper(players)); // Save the list of players
			File.WriteAllText(PlayersFilePath, playerJson); // Save players to JSON file

			Debug.Log("Data saved successfully.");
			}
		catch (Exception ex)
			{
			Debug.LogError("Error saving data: " + ex.Message);
			}
		}
	// --- End Region --- //

	// --- Region: Load Data --- //
	private void LoadTeams()
		{
		try
			{
			if (File.Exists(TeamsFilePath))
				{
				string teamJson = File.ReadAllText(TeamsFilePath);
				TeamListWrapper wrapper = JsonUtility.FromJson<TeamListWrapper>(teamJson);
				teams = wrapper.teams;
				Debug.Log("Teams loaded successfully.");
				}
			else
				{
				Debug.LogWarning("No teams found in file.");
				}
			}
		catch (Exception ex)
			{
			Debug.LogError("Error loading teams: " + ex.Message);
			}
		}

	private void LoadPlayers()
		{
		try
			{
			if (File.Exists(PlayersFilePath))
				{
				string playerJson = File.ReadAllText(PlayersFilePath);
				PlayerListWrapper wrapper = JsonUtility.FromJson<PlayerListWrapper>(playerJson);
				players = wrapper.players;
				Debug.Log("Players loaded successfully.");
				}
			else
				{
				Debug.LogWarning("No players found in file.");
				}
			}
		catch (Exception ex)
			{
			Debug.LogError("Error loading players: " + ex.Message);
			}
		}
	// --- End Region --- //

	// --- Region: Additional Functions --- //
	[Serializable]
	public class TeamListWrapper
		{
		public List<Team> teams;

		public TeamListWrapper(List<Team> teams)
			{
			this.teams = teams;
			}
		}

	[Serializable]
	public class PlayerListWrapper
		{
		public List<Player> players;

		public PlayerListWrapper(List<Player> players)
			{
			this.players = players;
			}
		}
	// --- End Region --- //
	}
