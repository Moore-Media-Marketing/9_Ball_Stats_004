using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

/// <summary>
/// Manages loading and saving CSV data.
/// </summary>
public class CSVManager
	{
	/// <summary>
	/// Loads a CSV file into a list of strings. If the file does not exist, it creates one with a header.
	/// </summary>
	/// <param name="filePath">Path to the CSV file.</param>
	/// <param name="header">Header row to include if file needs to be created.</param>
	/// <returns>A list of strings representing the CSV file lines.</returns>
	public static List<string> LoadCSV(string filePath, string header)
		{
		if (!File.Exists(filePath))
			{
			Console.WriteLine($"File not found: {filePath}. Creating a new file...");
			try
				{
				File.WriteAllText(filePath, header + Environment.NewLine);
				return new List<string> { header };
				}
			catch (Exception ex)
				{
				Console.WriteLine($"Error creating file {filePath}: {ex.Message}");
				return new List<string>();
				}
			}

		try
			{
			return File.ReadAllLines(filePath).ToList();
			}
		catch (Exception ex)
			{
			Console.WriteLine($"Error loading CSV file: {ex.Message}");
			return new List<string>();
			}
		}

	/// <summary>
	/// Saves a list of players to a CSV file with headers.
	/// </summary>
	/// <param name="filePath">Path to save the CSV file.</param>
	/// <param name="players">List of players to save.</param>
	public static void SavePlayersToCSV(string filePath, List<Player> players)
		{
		if (players == null || players.Count == 0)
			{
			Console.WriteLine("No players to save.");
			return;
			}

		try
			{
			List<string> lines = new() { "PlayerId,PlayerName,TeamId,SkillLevel" };
			lines.AddRange(players.Select(player => $"{player.PlayerId},{player.PlayerName},{player.TeamId},{player.SkillLevel}"));

			File.WriteAllLines(filePath, lines);
			Console.WriteLine($"Player data saved to {filePath}");
			}
		catch (Exception ex)
			{
			Console.WriteLine($"Error saving player data to CSV: {ex.Message}");
			}
		}

	/// <summary>
	/// Saves a list of teams to a CSV file with headers.
	/// </summary>
	/// <param name="filePath">Path to save the CSV file.</param>
	/// <param name="teams">List of teams to save.</param>
	public static void SaveTeamsToCSV(string filePath, List<Team> teams)
		{
		if (teams == null || teams.Count == 0)
			{
			Console.WriteLine("No teams to save.");
			return;
			}

		try
			{
			List<string> lines = new() { "TeamId,TeamName" };
			lines.AddRange(teams.Select(team => $"{team.TeamId},{team.TeamName}"));

			File.WriteAllLines(filePath, lines);
			Console.WriteLine($"Team data saved to {filePath}");
			}
		catch (Exception ex)
			{
			Console.WriteLine($"Error saving team data to CSV: {ex.Message}");
			}
		}
	}
