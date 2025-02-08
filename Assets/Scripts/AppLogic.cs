using System;
using System.Collections.Generic;

public class AppLogic
	{
	private CSVManager csvManager;
	private List<Player> players;

	public AppLogic(string filePath)
		{
		csvManager = new CSVManager(filePath);
		players = csvManager.LoadPlayersFromCSV();  // Load existing players from CSV on startup
		}

	public void AddPlayer(string playerName, string teamName, int skillLevel, PlayerStats stats, int teamId)
		{
		// Assuming you already have the necessary data, like 'teamName', 'skillLevel' and 'stats'
		Player player = new(playerName, teamName, skillLevel, stats, teamId);
		players.Add(player);
		Console.WriteLine($"Player {playerName} added successfully.");
		}

	public void AddStatsToPlayer(string playerName, PlayerStats stats)
		{
		Player player = players.Find(p => p.PlayerName == playerName);
		if (player != null)
			{
			player.Stats = stats;
			Console.WriteLine($"Stats updated for player {playerName}.");
			}
		else
			{
			Console.WriteLine($"Player {playerName} not found.");
			}
		}

	public void SavePlayers()
		{
		csvManager.SavePlayersToCSV(players);
		Console.WriteLine("Player data saved to CSV.");
		}

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

	public void DisplayAllPlayers()
		{
		if (players.Count == 0)
			{
			Console.WriteLine("No players available.");
			}
		else
			{
			foreach (var player in players)
				{
				Console.WriteLine($"Player: {player.PlayerName}, Skill Level: {player.SkillLevel}");
				}
			}
		}
	}
