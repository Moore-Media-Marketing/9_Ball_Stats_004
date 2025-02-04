using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class Team
	{
	// --- Team Attributes --- //
	public int TeamId { get; set; }  // Unique ID for each team
	public string TeamName { get; set; }  // Team's name

	// Constructor
	public Team(int teamId, string teamName)
		{
		this.TeamId = teamId;
		this.TeamName = teamName;
		}

	// --- Add Player to Team --- //
	public bool AddPlayer(Player player)
		{
		int teamSkillLevel = GetTeamSkillLevel(); // Get the current skill level of the team

		// Check if adding the player would violate the 23-Rule or exceed the max senior player limit
		if (teamSkillLevel + player.SkillLevel > 23 || CountSeniorPlayers() >= 2)
			{
			Debug.LogWarning("Cannot add player: Violates 23-Rule or too many senior players.");
			return false; // Reject player if it violates the 23-Rule or too many senior players
			}

		// Add player to the team's list in CSV
		AddPlayerToTeamInCsv(player);
		return true;
		}

	// --- Get Team Skill Level --- //
	public int GetTeamSkillLevel()
		{
		int totalSkillLevel = 0;
		List<Player> players = LoadPlayersFromCsv();
		foreach (var player in players)
			{
			if (player.TeamId == TeamId)  // Only include players from this team
				{
				totalSkillLevel += player.SkillLevel;
				}
			}
		return totalSkillLevel;
		}

	// --- Count Senior Players --- //
	public int CountSeniorPlayers()
		{
		int seniorPlayers = 0;
		List<Player> players = LoadPlayersFromCsv();
		foreach (var player in players)
			{
			if (player.TeamId == TeamId && player.SkillLevel >= 6)  // Count senior players (skill level 6+)
				{
				seniorPlayers++;
				}
			}
		return seniorPlayers;
		}

	// --- Remove Player from Team --- //
	public void RemovePlayer(Player player)
		{
		RemovePlayerFromTeamInCsv(player);  // Remove player from the team's CSV data
		}

	// --- CSV Helper Methods --- //
	// Adds a player to the team in the CSV file
	private void AddPlayerToTeamInCsv(Player player)
		{
		List<Player> players = LoadPlayersFromCsv();
		player.TeamId = TeamId;  // Assign the player to the current team

		players.Add(player);  // Add player to the list

		SavePlayersToCsv(players);  // Save updated players list to CSV
		}

	// Removes a player from the team in the CSV file
	private void RemovePlayerFromTeamInCsv(Player player)
		{
		List<Player> players = LoadPlayersFromCsv();
		Player playerToRemove = players.Find(p => p.PlayerId == player.PlayerId);

		if (playerToRemove != null)
			{
			players.Remove(playerToRemove);  // Remove the player from the list
			SavePlayersToCsv(players);  // Save updated players list to CSV
			}
		}

	// Loads all players from the CSV file
	private List<Player> LoadPlayersFromCsv()
		{
		List<Player> players = new();

		string path = "Assets/Players.csv";  // Assuming the file is stored in the Assets folder
		if (File.Exists(path))
			{
			string[] lines = File.ReadAllLines(path);
			foreach (string line in lines)
				{
				players.Add(Player.FromCsv(line));  // Convert CSV string to Player object
				}
			}

		return players;
		}

	// Saves the list of players to the CSV file
	private void SavePlayersToCsv(List<Player> players)
		{
		List<string> lines = new();

		foreach (var player in players)
			{
			lines.Add(player.ToCsv());  // Convert Player object to CSV string
			}

		string path = "Assets/Players.csv";  // Path to save the CSV file
		File.WriteAllLines(path, lines.ToArray());
		}
	}
