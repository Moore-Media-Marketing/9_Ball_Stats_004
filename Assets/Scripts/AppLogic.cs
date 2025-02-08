using System;
using System.Collections.Generic;

public class AppLogic
	{
	private CSVManager csvManager;
	private List<Player> players;

	// Constructor to initialize CSVManager and load players from CSV on startup
	public AppLogic(string filePath)
		{
		csvManager = new CSVManager(filePath);
		players = csvManager.LoadPlayersFromCSV();  // Load existing players from CSV on startup
		}

	// Method to add a new player
	public void AddPlayer(string playerName, string teamName, int skillLevel, PlayerStats stats, int teamId)
		{
		// Create a new player object
		Player player = new(playerName, teamName, skillLevel, stats, teamId);
		players.Add(player);
		Console.WriteLine($"Player {playerName} added successfully.");
		}

	// Method to add or update stats for an existing player
	public void AddStatsToPlayer(string playerName, PlayerStats stats)
		{
		Player player = players.Find(p => p.PlayerName == playerName);
		if (player != null)
			{
			player.Stats = stats; // Update stats
			Console.WriteLine($"Stats updated for player {playerName}.");
			}
		else
			{
			Console.WriteLine($"Player {playerName} not found.");
			}
		}

	// Method to save all player data to CSV
	public void SavePlayers()
		{
		csvManager.SavePlayersToCSV(players); // Save the list of players to CSV
		Console.WriteLine("Player data saved to CSV.");
		}

	// Method to display stats for a specific player
	public void DisplayPlayerStats(string playerName)
		{
		Player player = players.Find(p => p.PlayerName == playerName);
		if (player != null)
			{
			Console.WriteLine($"Player: {player.PlayerName}");
			Console.WriteLine($"Skill Level: {player.SkillLevel}");
			Console.WriteLine($"Current Season Matches Played: {player.Stats.CurrentSeasonMatchesPlayed}");
			Console.WriteLine($"Lifetime Games Played: {player.Stats.LifetimeGamesPlayed}");
			}
		else
			{
			Console.WriteLine($"Player {playerName} not found.");
			}
		}

	// Method to display all players in the list
	public void DisplayAllPlayers()
		{
		if (players.Count == 0)
			{
			Console.WriteLine("No players available.");
			}
		else
			{
			Console.WriteLine("List of Players:");
			foreach (var player in players)
				{
				Console.WriteLine($"Player: {player.PlayerName}, Skill Level: {player.SkillLevel}");
				}
			}
		}
	}
