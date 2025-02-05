using System.Collections.Generic;
using System.IO;

using UnityEngine;

public class DatabaseManager:MonoBehaviour
	{
	// --- Singleton Pattern --- //
	public static DatabaseManager Instance { get; private set; }

	private string playersCsvPath = "Assets/Resources/players.csv";  // Update to Assets/Resources/
	private string teamsCsvPath = "Assets/Resources/teams.csv";      // Update to Assets/Resources/

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

	// --- Save Player Data --- //
	public void SavePlayerData(Player player)
		{
		try
			{
			List<Player> players = LoadPlayersFromCsv();
			var existingPlayer = players.Find(p => p.PlayerId == player.PlayerId);

			if (existingPlayer != null)
				{
				players.Remove(existingPlayer);
				}
			players.Add(player);
			SavePlayersToCsv(players);
			Debug.Log($"Player data for {player.PlayerName} saved/updated successfully.");
			}
		catch (System.Exception ex)
			{
			Debug.LogError($"Error saving player data: {ex.Message}");
			}
		}

	// --- Load Player Data by ID --- //
	public Player LoadPlayerData(int playerId)
		{
		try
			{
			List<Player> players = LoadPlayersFromCsv();
			return players.Find(p => p.PlayerId == playerId);
			}
		catch (System.Exception ex)
			{
			Debug.LogError($"Error loading player data: {ex.Message}");
			return null;
			}
		}

	// --- Load Players from CSV --- //
	public List<Player> LoadPlayersFromCsv()
		{
		List<Player> players = new();
		try
			{
			if (File.Exists(playersCsvPath))
				{
				string[] lines = File.ReadAllLines(playersCsvPath);
				foreach (string line in lines)
					{
					string[] values = line.Split(',');
					if (values.Length >= 20)  // Ensure all stats are included
						{
						Player player = new()
							{
							PlayerId = int.Parse(values[0]),
							PlayerName = values[1],
							SkillLevel = int.Parse(values[2]),
							CurrentSeasonBreakAndRun = int.Parse(values[3]),
							CurrentSeasonDefensiveShotAverage = float.Parse(values[4]),
							CurrentSeasonMatchesPlayed = int.Parse(values[5]),
							CurrentSeasonMatchesWon = int.Parse(values[6]),
							CurrentSeasonMiniSlams = int.Parse(values[7]),
							CurrentSeasonNineOnTheSnap = int.Parse(values[8]),
							CurrentSeasonPaPercentage = float.Parse(values[9]),
							CurrentSeasonPointsAwarded = int.Parse(values[10]),
							CurrentSeasonPointsPerMatch = float.Parse(values[11]),
							CurrentSeasonPpm = float.Parse(values[12]),
							CurrentSeasonShutouts = int.Parse(values[13]),
							CurrentSeasonSkillLevel = int.Parse(values[14]),
							CurrentSeasonTotalPoints = int.Parse(values[15]),
							LifetimeBreakAndRun = int.Parse(values[16]),
							LifetimeDefensiveShotAverage = float.Parse(values[17]),
							LifetimeGamesPlayed = int.Parse(values[18]),
							LifetimeGamesWon = int.Parse(values[19]),
							LifetimeMatchesPlayed = int.Parse(values[20]),
							LifetimeMatchesWon = int.Parse(values[21]),
							LifetimeMiniSlams = int.Parse(values[22]),
							LifetimeNineOnTheSnap = int.Parse(values[23]),
							LifetimeShutouts = int.Parse(values[24])
							};
						players.Add(player);
						}
					}
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
				lines.Add($"{player.PlayerId},{player.PlayerName},{player.SkillLevel},{player.CurrentSeasonBreakAndRun},{player.CurrentSeasonDefensiveShotAverage},{player.CurrentSeasonMatchesPlayed},{player.CurrentSeasonMatchesWon},{player.CurrentSeasonMiniSlams},{player.CurrentSeasonNineOnTheSnap},{player.CurrentSeasonPaPercentage},{player.CurrentSeasonPointsAwarded},{player.CurrentSeasonPointsPerMatch},{player.CurrentSeasonPpm},{player.CurrentSeasonShutouts},{player.CurrentSeasonSkillLevel},{player.CurrentSeasonTotalPoints},{player.LifetimeBreakAndRun},{player.LifetimeDefensiveShotAverage},{player.LifetimeGamesPlayed},{player.LifetimeGamesWon},{player.LifetimeMatchesPlayed},{player.LifetimeMatchesWon},{player.LifetimeMiniSlams},{player.LifetimeNineOnTheSnap},{player.LifetimeShutouts}");
				}
			File.WriteAllLines(playersCsvPath, lines);
			}
		catch (System.Exception ex)
			{
			Debug.LogError($"Error saving players to CSV: {ex.Message}");
			}
		}

	// --- Load Teams from CSV --- //
	public List<Team> LoadTeamsFromCsv()
		{
		List<Team> teams = new();
		try
			{
			if (File.Exists(teamsCsvPath))
				{
				string[] lines = File.ReadAllLines(teamsCsvPath);
				foreach (string line in lines)
					{
					string[] values = line.Split(',');
					if (values.Length == 2)  // Assuming 2 columns: TeamId, TeamName
						{
						int teamId = int.Parse(values[0]);
						string teamName = values[1];
						Team team = new(teamId, teamName);
						teams.Add(team);
						}
					}
				}
			}
		catch (System.Exception ex)
			{
			Debug.LogError($"Error loading teams from CSV: {ex.Message}");
			}
		return teams;
		}

	// --- Save Teams to CSV --- //
	public void SaveTeamsToCsv(List<Team> teams)
		{
		try
			{
			List<string> lines = new();
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
