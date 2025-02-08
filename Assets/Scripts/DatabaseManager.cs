using System;
using System.Collections.Generic;
using System.IO;

public class DatabaseManager
	{
	private string teamsFilePath = "Teams.csv";
	private string playersFilePath = "Players.csv";

	// Method to load teams from CSV
	public List<Team> LoadTeams()
		{
		var teams = new List<Team>();
		try
			{
			using (var reader = new StreamReader(teamsFilePath))
			using (var csv = new CsvReader(reader, new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)))
				{
				teams = csv.GetRecords<Team>().ToList();
				}
			}
		catch (Exception ex)
			{
			Debug.LogError("Error loading teams: " + ex.Message);
			}
		return teams;
		}

	// Method to save teams to CSV
	public void SaveTeams(List<Team> teams)
		{
		try
			{
			using (var writer = new StreamWriter(teamsFilePath))
			using (var csv = new CsvWriter(writer, new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)))
				{
				csv.WriteRecords(teams);
				}
			}
		catch (Exception ex)
			{
			Debug.LogError("Error saving teams: " + ex.Message);
			}
		}

	// Method to load players from CSV
	public List<Player> LoadPlayersFromCsv(string filePath)
		{
		var players = new List<Player>();
		try
			{
			using (var reader = new StreamReader(filePath))
			using (var csv = new CsvReader(reader, new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)))
				{
				players = csv.GetRecords<Player>().ToList();
				}
			}
		catch (Exception ex)
			{
			Debug.LogError("Error loading players from CSV: " + ex.Message);
			}
		return players;
		}

	// Method to save players to CSV
	public void SavePlayersToCsv(string filePath, List<Player> players)
		{
		try
			{
			using (var writer = new StreamWriter(filePath))
			using (var csv = new CsvWriter(writer, new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)))
				{
				csv.WriteRecords(players);
				}
			}
		catch (Exception ex)
			{
			Debug.LogError("Error saving players to CSV: " + ex.Message);
			}
		}
	}
