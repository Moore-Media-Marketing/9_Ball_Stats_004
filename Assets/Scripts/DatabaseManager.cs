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

		LoadTeamsAndPlayers();
		}

	// --- Load Teams and Players --- //
	public void LoadTeamsAndPlayers()
		{
		teams = LoadDataFromCSV<Team>(teamsCsvManager, values =>
		{
			int teamId = int.Parse(values[0]);
			string teamName = values[1];
			return new Team(teamId, teamName);
		});

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
			data = csvManager.LoadDataFromCSV(map);
			}
		catch (Exception ex)
			{
			Debug.LogError($"Error loading data from CSV: {ex.Message}");
			}
		return data;
		}

	// --- Add Team --- //
	public void AddTeam(string teamName)
		{
		if (teams.Any(t => t.TeamName == teamName))
			{
			Debug.LogWarning("Team already exists.");
			return;
			}

		int newTeamId = teams.Count > 0 ? teams.Max(t => t.TeamId) + 1 : 1;
		teams.Add(new Team(newTeamId, teamName));
		SaveTeams();
		}

	// --- Modify Team Name --- //
	public void ModifyTeam(int teamId, string newTeamName)
		{
		Team team = teams.FirstOrDefault(t => t.TeamId == teamId);
		if (team != null)
			{
			team.TeamName = newTeamName;
			SaveTeams();
			}
		}

	// --- Delete Team (Moves Players to Unassigned) --- //
	public void DeleteTeam(int teamId)
		{
		Team teamToDelete = teams.FirstOrDefault(t => t.TeamId == teamId);
		if (teamToDelete != null)
			{
			teams.Remove(teamToDelete);
			foreach (var player in players.Where(p => p.TeamId == teamId))
				{
				player.TeamId = -1;
				player.TeamName = "Unassigned";
				}
			SaveTeams();
			SavePlayers();
			}
		}

	// --- Add Player --- //
	public void AddPlayer(string playerName, string teamName, int skillLevel)
		{
		int teamId = teams.FirstOrDefault(t => t.TeamName == teamName)?.TeamId ?? -1;
		players.Add(new Player(playerName, teamName, skillLevel, new PlayerStats(), teamId));
		SavePlayers();
		}

	// --- Assign Player to Another Team --- //
	public void AssignPlayerToTeam(string playerName, string newTeamName)
		{
		Player player = players.FirstOrDefault(p => p.PlayerName == playerName);
		if (player != null)
			{
			player.TeamName = newTeamName;
			player.TeamId = teams.FirstOrDefault(t => t.TeamName == newTeamName)?.TeamId ?? -1;
			SavePlayers();
			}
		}

	// --- Delete Player --- //
	public void DeletePlayer(string playerName)
		{
		players.RemoveAll(p => p.PlayerName == playerName);
		SavePlayers();
		}

	// --- Save Teams to CSV --- //
	private void SaveTeams()
		{
		List<string> lines = new() { "TeamId,TeamName" };

		foreach (var team in teams)
			{
			lines.Add($"{team.TeamId},{team.TeamName}");
			}

		File.WriteAllLines(teamsDataFilePath, lines);
		}

	// --- Save Players to CSV --- //
	private void SavePlayers()
		{
		playersCsvManager.SavePlayersToCSV(players);
		}

	// --- Get All Players --- //
	public List<Player> GetAllPlayers()
		{
		return new List<Player>(players);
		}

	// --- Get All Teams --- //
	public List<Team> GetAllTeams()
		{
		return new List<Team>(teams);
		}

	// --- Update Player Stats --- //
	public void UpdatePlayerStats(string playerName, PlayerStats newStats)
		{
		Player player = players.FirstOrDefault(p => p.PlayerName == playerName);
		if (player != null)
			{
			player.Stats = newStats;
			SavePlayers();
			}
		}
	}
