using System.Collections.Generic;
using System.IO;

using UnityEngine;

public class CSVManager
	{
	private string filePath;

	public CSVManager(string filePath)
		{
		this.filePath = filePath;
		}

	// Load players from the CSV file (no teamId needed here)
	public List<Player> LoadPlayersFromCSV()
		{
		List<Player> players = new();

		// Read the CSV file
		if (File.Exists(filePath))
			{
			string[] lines = File.ReadAllLines(filePath);
			foreach (string line in lines)
				{
				// Skip the header line
				if (line.StartsWith("PlayerName"))
					continue;

				string[] values = line.Split(',');

				string playerName = values[0];
				string teamName = values[1];
				int skillLevel = int.Parse(values[2]);
				int lifetimeGamesWon = int.Parse(values[3]);
				int lifetimeGamesPlayed = int.Parse(values[4]);

				// Assuming stats are available, create new PlayerStats (use appropriate constructor)
				PlayerStats stats = new()
					{
					LifetimeGamesWon = lifetimeGamesWon,
					LifetimeGamesPlayed = lifetimeGamesPlayed,
					};

				// Extracting teamId from teamName (make sure this is the correct logic)
				int teamId = teamName == "Team A" ? 1 : 2; // Example logic for teamId

				players.Add(new Player(playerName, teamName, skillLevel, stats, teamId));
				}
			}
		return players;
		}

	// Save players to the CSV file
	public void SavePlayersToCSV(List<Player> players)
		{
		List<string> lines = new()
			{
			// Add headers to CSV
			"PlayerName,TeamName,SkillLevel,LifetimeGamesWon,LifetimeGamesPlayed"
			};

		// Add player data to CSV
		foreach (var player in players)
			{
			string line = $"{player.PlayerName},{player.TeamName},{player.SkillLevel}," +
						  $"{player.Stats.LifetimeGamesWon},{player.Stats.LifetimeGamesPlayed}";
			lines.Add(line);
			}

		// Write to CSV file
		File.WriteAllLines(filePath, lines);
		Debug.Log("Players data saved to CSV.");
		}
	}
