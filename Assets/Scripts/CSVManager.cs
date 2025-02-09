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
	public static List<string> LoadCSV(string filePath)
		{
		return File.Exists(filePath) ? File.ReadAllLines(filePath).ToList() : new List<string>();
		}

	/// <summary>
	/// Saves a list of players to a CSV file.
	/// </summary>
	public static void SavePlayersToCSV(string filePath, List<Player> players)
		{
		List<string> lines = players.Select(player =>
			$"{player.PlayerId},{player.PlayerName},{player.TeamId},{player.SkillLevel}").ToList();

		File.WriteAllLines(filePath, lines);
		}

	/// <summary>
	/// Saves a list of teams to a CSV file.
	/// </summary>
	public static void SaveTeamsToCSV(string filePath, List<Team> teams)
		{
		List<string> lines = teams.Select(team => $"{team.TeamId},{team.TeamName}").ToList();
		File.WriteAllLines(filePath, lines);
		}
	}
