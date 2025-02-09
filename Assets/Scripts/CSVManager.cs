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
	/// Loads a CSV file into a list of strings.
	/// </summary>
	/// <param name="filePath">Path to the CSV file.</param>
	/// <returns>A list of strings representing the CSV file lines.</returns>
	public static List<string> LoadCSV(string filePath)
		{
		// Check if the file exists before attempting to load
		if (!File.Exists(filePath))
			{
			Console.WriteLine($"File not found: {filePath}");
			return new List<string>(); // Return empty list if the file doesn't exist
			}

		try
			{
			// Read all lines from the file and return as a list
			return File.ReadAllLines(filePath).ToList();
			}
		catch (Exception ex)
			{
			// Log the exception and return an empty list
			Console.WriteLine($"Error loading CSV file: {ex.Message}");
			return new List<string>();
			}
		}

	/// <summary>
	/// Saves a list of players to a CSV file.
	/// </summary>
	/// <param name="filePath">Path to save the CSV file.</param>
	/// <param name="players">List of players to save.</param>
	public static void SavePlayersToCSV(string filePath, List<Player> players)
		{
		// Check if the list of players is empty
		if (players == null || players.Count == 0)
			{
			Console.WriteLine("No players to save.");
			return; // Exit if there are no players to save
			}

		try
			{
			// Create the CSV lines from the player data
			List<string> lines = players.Select(player =>
				$"{player.PlayerId},{player.PlayerName},{player.TeamId},{player.SkillLevel}").ToList();

			// Write the lines to the CSV file
			File.WriteAllLines(filePath, lines);
			Console.WriteLine($"Player data saved to {filePath}");
			}
		catch (Exception ex)
			{
			// Log the exception if something goes wrong
			Console.WriteLine($"Error saving player data to CSV: {ex.Message}");
			}
		}

	/// <summary>
	/// Saves a list of teams to a CSV file.
	/// </summary>
	/// <param name="filePath">Path to save the CSV file.</param>
	/// <param name="teams">List of teams to save.</param>
	public static void SaveTeamsToCSV(string filePath, List<Team> teams)
		{
		// Check if the list of teams is empty
		if (teams == null || teams.Count == 0)
			{
			Console.WriteLine("No teams to save.");
			return; // Exit if there are no teams to save
			}

		try
			{
			// Create the CSV lines from the team data
			List<string> lines = teams.Select(team => $"{team.TeamId},{team.TeamName}").ToList();

			// Write the lines to the CSV file
			File.WriteAllLines(filePath, lines);
			Console.WriteLine($"Team data saved to {filePath}");
			}
		catch (Exception ex)
			{
			// Log the exception if something goes wrong
			Console.WriteLine($"Error saving team data to CSV: {ex.Message}");
			}
		}
	}