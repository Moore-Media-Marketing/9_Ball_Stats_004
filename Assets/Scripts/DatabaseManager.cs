using System.Collections.Generic;
using System.IO;

using UnityEngine;

public class DatabaseManager:MonoBehaviour
	{
	// --- Singleton Pattern --- //
	public static DatabaseManager Instance { get; private set; }

	private string playersCsvPath = "Assets/Resources/players.csv";
	private string teamsCsvPath = "Assets/Resources/teams.csv";

	private void Awake()
		{
		if (Instance == null)
			{
			Instance = this;
			DontDestroyOnLoad(gameObject);
			}
		else
			{
			Destroy(gameObject);
			}
		}

	// --- Load Players from CSV --- //
	public List<Player> LoadPlayersFromCsv()
		{
		List<Player> players = new List<Player>();
		try
			{
			if (File.Exists(playersCsvPath))
				{
				string[] lines = File.ReadAllLines(playersCsvPath);
				foreach (string line in lines)
					{
					string[] values = line.Split(',');
					if (values.Length >= 20)  // Ensure there are enough values in the CSV
						{
						// Handle missing or invalid data
						if (int.TryParse(values[0], out int playerId) &&
							int.TryParse(values[2], out int skillLevel) &&
							int.TryParse(values[3], out int breakAndRun) &&
							float.TryParse(values[4], out float defensiveShotAvg))
							{
							// Use the Player constructor with the required parameters
							Player player = new Player(playerId, values[1], skillLevel)
								{
								CurrentSeasonBreakAndRun = breakAndRun,
								CurrentSeasonDefensiveShotAverage = defensiveShotAvg,
								// Add other properties similarly, with appropriate error handling if necessary
								};
							players.Add(player);
							}
						else
							{
							Debug.LogWarning($"Invalid data in CSV for player with ID: {values[0]}");
							}
						}
					else
						{
						Debug.LogWarning($"Skipping line with insufficient data: {line}");
						}
					}
				}
			else
				{
				Debug.LogWarning("Players CSV file not found, creating new file.");
				SavePlayersToCsv(players); // Optionally create an empty file if not found
				}
			}
		catch (System.Exception ex)
			{
			Debug.LogError($"Error loading players from CSV: {ex.Message}");
			}
		return players;
		}

	// --- Save Players to CSV --- //
	public void SavePlayersToCsv(List<Player> players)
		{
		try
			{
			List<string> lines = new List<string>();
			foreach (Player player in players)
				{
				lines.Add(player.ToCsv()); // Assuming Player class has ToCsv method
				}
			File.WriteAllLines(playersCsvPath, lines);
			}
		catch (System.Exception ex)
			{
			Debug.LogError($"Error saving players to CSV: {ex.Message}");
			}
		}

	// --- Load Teams from CSV --- //
	public List<Team> LoadTeams()
		{
		List<Team> teams = new List<Team>();
		try
			{
			if (File.Exists(teamsCsvPath))
				{
				string[] lines = File.ReadAllLines(teamsCsvPath);
				foreach (string line in lines)
					{
					string[] values = line.Split(',');
					if (values.Length == 2)
						{
						int teamId = int.Parse(values[0]);
						string teamName = values[1];
						Team team = new Team(teamId, teamName);
						teams.Add(team);
						}
					}
				}
			else
				{
				Debug.LogWarning("Teams CSV file not found, creating new file.");
				SaveTeams(teams); // Optionally create an empty file if not found
				}
			}
		catch (System.Exception ex)
			{
			Debug.LogError($"Error loading teams from CSV: {ex.Message}");
			}
		return teams;
		}

	// --- Save Teams to CSV --- //
	public void SaveTeams(List<Team> teams)
		{
		try
			{
			List<string> lines = new List<string>();
			foreach (Team team in teams)
				{
				lines.Add($"{team.TeamId},{team.TeamName}");
				}
			File.WriteAllLines(teamsCsvPath, lines);
			}
		catch (System.Exception ex)
			{
			Debug.LogError($"Error saving teams to CSV: {ex.Message}");
			}
		}
	}
