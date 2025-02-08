using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;

public class CSVManager
	{
	private string filePath;

	// Constructor to set the file path
	public CSVManager(string path)
		{
		filePath = path;
		}

	// Method to load data from CSV and map to objects
	public List<T> LoadDataFromCSV<T>(Func<string[], T> map)
		{
		List<T> data = new();

		try
			{
			foreach (var line in File.ReadLines(filePath).Skip(1)) // Skip header
				{
				string[] values = line.Split(','); // Split line by comma
				data.Add(map(values)); // Map values to the object
				}
			}
		catch (Exception ex)
			{
			Debug.LogError("Error loading CSV: " + ex.Message); // Static access to Debug.LogError
			}

		return data;
		}

	// Save players to CSV file
	public void SavePlayersToCSV(List<Player> players)
		{
		List<string> lines = new() { "PlayerName,TeamId,SkillLevel,LifetimeGamesPlayed,..." }; // CSV Header

		// Loop through each player and create a CSV line
		foreach (var player in players)
			{
			lines.Add($"{player.PlayerName},{player.TeamId},{player.SkillLevel}," +
					  $"{player.Stats.LifetimeGamesPlayed},{player.Stats.LifetimeGamesWon}," +
					  $"{player.Stats.LifetimeBreakAndRun},{player.Stats.LifetimeMatchesPlayed}," +
					  $"{player.Stats.LifetimeMatchesWon},{player.Stats.LifetimeMiniSlams}," +
					  $"{player.Stats.LifetimeNineOnTheSnap},{player.Stats.LifetimeShutouts}," +
					  $"{player.Stats.CurrentSeasonMatchesPlayed},{player.Stats.CurrentSeasonMatchesWon}," +
					  $"{player.Stats.CurrentSeasonBreakAndRun},{player.Stats.CurrentSeasonDefensiveShotAverage}," +
					  $"{player.Stats.CurrentSeasonSkillLevel},{player.Stats.CurrentSeasonPointsAwarded}"); // Add all stats
			}

		try
			{
			File.WriteAllLines(filePath, lines); // Save the CSV data to the file
			Debug.Log("Players saved to CSV.");
			}
		catch (Exception ex)
			{
			Debug.LogError("Error saving players to CSV: " + ex.Message);
			}
		}

	// Method to load players from CSV (Complete implementation)
	public List<Player> LoadPlayersFromCSV()
		{
		List<Player> players = new();

		try
			{
			// Assuming you have a reference to the teams data so you can map team names to team IDs
			var teamsData = LoadDataFromCSV<Team>((values) => new Team(int.Parse(values[0]), values[1]));
			Dictionary<string, int> teamNameToId = teamsData.ToDictionary(t => t.TeamName, t => t.TeamId); // Map team names to team IDs

			// Load player data
			foreach (var line in File.ReadLines(filePath).Skip(1)) // Skip header
				{
				string[] values = line.Split(',');

				// Get player stats from CSV values
				PlayerStats stats = new()
					{
					LifetimeGamesPlayed = int.Parse(values[3]),
					LifetimeGamesWon = int.Parse(values[4]),
					LifetimeBreakAndRun = int.Parse(values[5]),
					LifetimeMatchesPlayed = int.Parse(values[6]),
					LifetimeMatchesWon = int.Parse(values[7]),
					LifetimeMiniSlams = int.Parse(values[8]),
					LifetimeNineOnTheSnap = int.Parse(values[9]),
					LifetimeShutouts = int.Parse(values[10]),
					CurrentSeasonMatchesPlayed = int.Parse(values[11]),
					CurrentSeasonMatchesWon = int.Parse(values[12]),
					CurrentSeasonBreakAndRun = int.Parse(values[13]),
					CurrentSeasonDefensiveShotAverage = float.Parse(values[14]),
					CurrentSeasonSkillLevel = int.Parse(values[15]),
					CurrentSeasonPointsAwarded = int.Parse(values[16])
					};

				// Find the team ID using the team name
				string teamName = values[1].Trim().ToLower();
				int teamId = teamNameToId.ContainsKey(teamName) ? teamNameToId[teamName] : -1;

				// Create the player object
				Player player = new(values[0], teamName, int.Parse(values[2]), stats, teamId);
				players.Add(player);
				}
			}
		catch (Exception ex)
			{
			Debug.LogError("Error loading players from CSV: " + ex.Message);
			}

		return players;
		}
	}
