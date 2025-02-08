using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;

public class DatabaseManager:MonoBehaviour
	{
	public static DatabaseManager Instance { get; private set; }

	private string playersDataFilePath = "Assets/PlayerData.csv";
	private string teamsDataFilePath = "Assets/TeamsData.csv";

	private List<Team> teams = new();
	private List<Player> players = new();

	private CSVManager playersCsvManager;
	private CSVManager teamsCsvManager;

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

		playersCsvManager = new CSVManager(playersDataFilePath);
		teamsCsvManager = new CSVManager(teamsDataFilePath);

		// Load the initial data
		LoadTeamsAndPlayers(); // Ensure this is correct
		}

	// --- Load Teams and Players --- //
	public void LoadTeamsAndPlayers()
		{
		// Load Teams data
		teams = LoadDataFromCSV<Team>(teamsCsvManager, values =>
		{
			int teamId = int.Parse(values[0]);
			string teamName = values[1];
			return new Team(teamId, teamName);
		});

		// Load Players data
		players = LoadDataFromCSV<Player>(playersCsvManager, values =>
		{
			string playerName = values[0];
			string teamName = values[1];
			int skillLevel = int.Parse(values[2]);
			PlayerStats stats = new()
				{
				LifetimeGamesWon = int.Parse(values[3]),
				LifetimeGamesPlayed = int.Parse(values[4]),
				LifetimeBreakAndRun = int.Parse(values[5]),
				LifetimeDefensiveShotAverage = float.Parse(values[6]),
				LifetimeMatchesPlayed = int.Parse(values[7]),
				LifetimeMatchesWon = int.Parse(values[8]),
				LifetimeMiniSlams = int.Parse(values[9]),
				LifetimeNineOnTheSnap = int.Parse(values[10]),
				LifetimeShutouts = int.Parse(values[11]),
				CurrentSeasonBreakAndRun = int.Parse(values[12]),
				CurrentSeasonDefensiveShotAverage = float.Parse(values[13]),
				CurrentSeasonMatchesPlayed = int.Parse(values[14]),
				CurrentSeasonMatchesWon = int.Parse(values[15]),
				CurrentSeasonMiniSlams = int.Parse(values[16]),
				CurrentSeasonNineOnTheSnap = int.Parse(values[17]),
				CurrentSeasonPaPercentage = float.Parse(values[18]),
				CurrentSeasonPointsAwarded = int.Parse(values[19]),
				CurrentSeasonPointsPerMatch = float.Parse(values[20]),
				CurrentSeasonPpm = float.Parse(values[21]),
				CurrentSeasonShutouts = int.Parse(values[22]),
				CurrentSeasonSkillLevel = int.Parse(values[23]),
				CurrentSeasonTotalPoints = int.Parse(values[24])
				};

			int teamId = teams.FirstOrDefault(t => t.TeamName == teamName)?.TeamId ?? -1;

			return new Player(playerName, teamName, skillLevel, stats, teamId);
		});
		}

	// --- Generalized CSV Loading Method --- //
	private List<T> LoadDataFromCSV<T>(CSVManager csvManager, Func<string[], T> map)
		{
		List<T> data = new();
		try
			{
			data = csvManager.LoadDataFromCSV(map); // Use CSVManager's LoadData method
			}
		catch (System.Exception ex)
			{
			Debug.LogError($"Error loading data from CSV: {ex.Message}");
			}
		return data;
		}

	// --- Save Teams to CSV --- //
	public void SaveTeams(List<Team> teams)
		{
		List<string> lines = new() { "TeamId,TeamName" }; // Ensure CSV Header

		foreach (var team in teams)
			{
			lines.Add($"{team.TeamId},{team.TeamName}");
			}

		try
			{
			File.WriteAllLines(teamsDataFilePath, lines);
			Debug.Log("Teams saved to CSV.");
			}
		catch (System.Exception ex)
			{
			Debug.LogError("Error saving teams to CSV: " + ex.Message);
			}
		}

	// --- Save Players to CSV --- //
	public void SavePlayersToCsv(List<Player> players)
		{
		playersCsvManager.SavePlayersToCSV(players); // Assuming this method is defined in CSVManager
		}

	// --- Load Players by Team ID --- //
	public List<Player> LoadPlayersByTeam(int teamId)
		{
		List<Player> playersByTeam = players.Where(p => p.TeamId == teamId).ToList();
		Debug.Log($"Loaded {playersByTeam.Count} players for team with ID {teamId}.");
		return playersByTeam;
		}

	// --- Delete Team by ID --- //
	public void DeleteTeam(int teamId)
		{
		Team teamToDelete = teams.FirstOrDefault(t => t.TeamId == teamId);
		if (teamToDelete != null)
			{
			teams.Remove(teamToDelete);
			players.RemoveAll(p => p.TeamId == teamId);
			SaveTeams(teams);
			SavePlayersToCsv(players);
			Debug.Log($"Team with ID {teamId} and its players have been deleted.");
			}
		else
			{
			Debug.LogWarning($"Team with ID {teamId} not found.");
			}
		}

	// --- Load Teams --- //
	public List<Team> LoadTeams()
		{
		try
			{
			teams = LoadDataFromCSV<Team>(teamsCsvManager, values =>
			{
				int teamId = int.Parse(values[0]);
				string teamName = values[1];
				return new Team(teamId, teamName);
			});
			return teams;
			}
		catch (Exception ex)
			{
			Debug.LogError($"Error loading teams: {ex.Message}");
			return new List<Team>(); // Return an empty list if something goes wrong
			}
		}

	// --- Save Player --- //
	public void SavePlayer(Player selectedPlayer)
		{
		try
			{
			var playerLine = $"{selectedPlayer.PlayerName},{selectedPlayer.TeamName},{selectedPlayer.SkillLevel}," +
							 $"{selectedPlayer.Stats.LifetimeGamesWon},{selectedPlayer.Stats.LifetimeGamesPlayed}," +
							 $"{selectedPlayer.Stats.LifetimeBreakAndRun},{selectedPlayer.Stats.LifetimeDefensiveShotAverage}," +
							 $"{selectedPlayer.Stats.LifetimeMatchesPlayed},{selectedPlayer.Stats.LifetimeMatchesWon}," +
							 $"{selectedPlayer.Stats.LifetimeMiniSlams},{selectedPlayer.Stats.LifetimeNineOnTheSnap}," +
							 $"{selectedPlayer.Stats.LifetimeShutouts},{selectedPlayer.Stats.CurrentSeasonBreakAndRun}," +
							 $"{selectedPlayer.Stats.CurrentSeasonDefensiveShotAverage},{selectedPlayer.Stats.CurrentSeasonMatchesPlayed}," +
							 $"{selectedPlayer.Stats.CurrentSeasonMatchesWon},{selectedPlayer.Stats.CurrentSeasonMiniSlams}," +
							 $"{selectedPlayer.Stats.CurrentSeasonNineOnTheSnap},{selectedPlayer.Stats.CurrentSeasonPaPercentage}," +
							 $"{selectedPlayer.Stats.CurrentSeasonPointsAwarded},{selectedPlayer.Stats.CurrentSeasonPointsPerMatch}," +
							 $"{selectedPlayer.Stats.CurrentSeasonPpm},{selectedPlayer.Stats.CurrentSeasonShutouts}," +
							 $"{selectedPlayer.Stats.CurrentSeasonSkillLevel},{selectedPlayer.Stats.CurrentSeasonTotalPoints}";

			File.AppendAllText(playersDataFilePath, playerLine + "\n");
			Debug.Log($"Player {selectedPlayer.PlayerName} saved.");
			}
		catch (Exception ex)
			{
			Debug.LogError($"Error saving player: {ex.Message}");
			}
		}
	}
