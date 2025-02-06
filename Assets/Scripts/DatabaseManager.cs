using System.Collections.Generic;
using System.IO;

using UnityEngine;

public class DatabaseManager:MonoBehaviour
	{
	// --- Singleton Pattern --- //
	public static DatabaseManager Instance { get; private set; }

	private string playersCsvPath = "Assets/Resources/players.csv"; // Path to players CSV file
	private string teamsCsvPath = "Assets/Resources/teams.csv"; // Path to teams CSV file
	private string matchupsCsvPath = "Assets/Resources/matchups.csv"; // Path to matchups CSV file

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
	public List<Player> LoadPlayersFromCsv(int teamId)
		{
		List<Player> players = new();
		try
			{
			if (File.Exists(playersCsvPath))
				{
				string[] lines = File.ReadAllLines(playersCsvPath, System.Text.Encoding.UTF8);
				foreach (string line in lines)
					{
					string[] values = line.Split(',');
					if (values.Length >= 27) // Adjusted length for CSV structure
						{
						// Parse required values safely
						if (int.TryParse(values[0], out int playerId) &&
							int.TryParse(values[2], out int skillLevel) &&
							int.TryParse(values[3], out int teamIdCsv) &&  // Using teamIdCsv to distinguish
							float.TryParse(values[4], out float defensiveShotAvg) &&
							int.TryParse(values[5], out int currentSeasonMatchesPlayed) &&
							int.TryParse(values[6], out int currentSeasonMatchesWon) &&
							int.TryParse(values[7], out int currentSeasonBreakAndRun) &&
							int.TryParse(values[9], out int currentSeasonMiniSlams) &&
							int.TryParse(values[10], out int currentSeasonNineOnTheSnap) &&
							int.TryParse(values[11], out int currentSeasonShutouts) &&
							int.TryParse(values[12], out int currentSeasonPointsAwarded) &&
							float.TryParse(values[13], out float currentSeasonPointsPerMatch) &&
							int.TryParse(values[14], out int lifetimeMatchesPlayed) &&
							float.TryParse(values[15], out float lifetimeDefensiveShotAverage) &&
							int.TryParse(values[16], out int lifetimeGamesPlayed) &&
							int.TryParse(values[17], out int lifetimeGamesWon) &&
							int.TryParse(values[18], out int lifetimeMatchesWon) &&
							int.TryParse(values[19], out int lifetimeMiniSlams) &&
							int.TryParse(values[20], out int lifetimeNineOnTheSnap) &&
							int.TryParse(values[21], out int lifetimeBreakAndRun) &&
							int.TryParse(values[22], out int lifetimeShutouts) &&
							int.TryParse(values[23], out int currentSeasonSkillLevel) &&
							int.TryParse(values[24], out int currentSeasonTotalPoints) &&
							int.TryParse(values[25], out int lifetimeMatchesPlayedInLast2Years))
							{
							Player player = new(
								teamIdCsv, // Using teamIdCsv to filter by team
								values[8], // TeamName
								values[1], // PlayerName
								skillLevel,
								currentSeasonMatchesPlayed,
								currentSeasonMatchesWon,
								currentSeasonPointsAwarded,
								currentSeasonPointsPerMatch,
								currentSeasonShutouts,
								currentSeasonSkillLevel,
								currentSeasonTotalPoints,
								lifetimeGamesPlayed,
								lifetimeGamesWon,
								lifetimeMiniSlams,
								lifetimeNineOnTheSnap,
								lifetimeShutouts,
								lifetimeMatchesPlayedInLast2Years,
								lifetimeBreakAndRun,
								lifetimeDefensiveShotAverage
							);

							if (player.TeamId == teamId) // Filter by teamId
								{
								players.Add(player);
								}
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
				SavePlayersToCsv(players); // Optionally create a new file if not found
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
			List<string> lines = new();
			foreach (Player player in players)
				{
				lines.Add(player.ToCsv()); // Assuming Player has ToCsv method
				}
			File.WriteAllLines(playersCsvPath, lines, System.Text.Encoding.UTF8);
			}
		catch (System.Exception ex)
			{
			Debug.LogError($"Error saving players to CSV: {ex.Message}");
			}
		}

	// --- Load Teams from CSV --- //
	public List<Team> LoadTeams()
		{
		List<Team> teams = new();
		try
			{
			if (File.Exists(teamsCsvPath))
				{
				string[] lines = File.ReadAllLines(teamsCsvPath, System.Text.Encoding.UTF8);
				foreach (string line in lines)
					{
					if (!string.IsNullOrWhiteSpace(line))
						{
						Team team = Team.FromCsv(line);
						teams.Add(team);
						}
					}
				}
			else
				{
				Debug.LogWarning("Teams CSV file not found, creating new file.");
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
			List<string> lines = new();
			foreach (Team team in teams)
				{
				lines.Add(team.ToCsv()); // Assuming Team has ToCsv method
				}
			File.WriteAllLines(teamsCsvPath, lines, System.Text.Encoding.UTF8);
			}
		catch (System.Exception ex)
			{
			Debug.LogError($"Error saving teams to CSV: {ex.Message}");
			}
		}

	// --- Load Matchups from CSV --- //
	public List<Match> LoadMatchupsFromCsv()
		{
		List<Match> matchups = new();
		try
			{
			if (File.Exists(matchupsCsvPath))
				{
				string[] lines = File.ReadAllLines(matchupsCsvPath, System.Text.Encoding.UTF8);
				foreach (string line in lines)
					{
					string[] values = line.Split(',');
					if (values.Length == 6) // MatchId, Player1Id, Player2Id, Player1Score, Player2Score, WinnerId
						{
						// Parse values and create Match object
						if (int.TryParse(values[0], out int matchId) &&
							int.TryParse(values[1], out int player1Id) &&
							int.TryParse(values[2], out int player2Id) &&
							int.TryParse(values[3], out int player1Score) &&
							int.TryParse(values[4], out int player2Score) &&
							int.TryParse(values[5], out int winnerId))
							{
							Match match = new(matchId, player1Id, player2Id, player1Score, player2Score, winnerId);
							matchups.Add(match);
							}
						else
							{
							Debug.LogWarning($"Invalid data in CSV for match with ID: {values[0]}");
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
				Debug.LogWarning("Matchups CSV file not found, creating new file.");
				// Optionally, create an empty file if it doesn't exist
				}
			}
		catch (System.Exception ex)
			{
			Debug.LogError($"Error loading matchups from CSV: {ex.Message}");
			}
		return matchups;
		}

	// --- Save Matchups to CSV --- //
	public void SaveMatchupsToCsv(List<Match> matchups)
		{
		try
			{
			List<string> lines = new();
			foreach (Match match in matchups)
				{
				lines.Add($"{match.MatchId},{match.Player1Id},{match.Player2Id},{match.Player1Score},{match.Player2Score},{match.WinnerId}");
				}
			File.WriteAllLines(matchupsCsvPath, lines, System.Text.Encoding.UTF8);
			}
		catch (System.Exception ex)
			{
			Debug.LogError($"Error saving matchups to CSV: {ex.Message}");
			}
		}
	}
