using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;

public class DatabaseManager:MonoBehaviour
	{
	public static DatabaseManager Instance { get; private set; }

	// Paths for CSV files
	private string playersDataFilePath = "Assets/PlayerData.csv";
	private string teamsDataFilePath = "Assets/TeamsData.csv";

	private void Awake()
		{
		if (Instance == null)
			{
			Instance = this;
			DontDestroyOnLoad(gameObject); // Keep the instance between scenes
			}
		else
			{
			Destroy(gameObject); // Destroy duplicates
			}
		}

	// Load teams from CSV
	public List<Team> LoadTeams()
		{
		List<Team> teams = new();

		if (File.Exists(teamsDataFilePath))
			{
			var lines = File.ReadAllLines(teamsDataFilePath);
			for (int i = 1; i < lines.Length; i++) // Skip header line
				{
				var line = lines[i].Split(',');
				int teamId = int.Parse(line[0]);
				string teamName = line[1];

				teams.Add(new Team(teamId, teamName));
				}
			}
		return teams;
		}

	// Load players from CSV
	public List<Player> LoadPlayersFromCsv(int teamId)
		{
		List<Player> players = new();

		if (File.Exists(playersDataFilePath))
			{
			var lines = File.ReadAllLines(playersDataFilePath);
			for (int i = 1; i < lines.Length; i++) // Skip header line
				{
				var line = lines[i].Split(',');
				string playerName = line[0];
				string teamName = line[1];
				int skillLevel = int.Parse(line[2]);
				int lifetimeGamesWon = int.Parse(line[3]);
				int lifetimeGamesPlayed = int.Parse(line[4]);

				PlayerStats stats = new()
					{
					LifetimeGamesWon = lifetimeGamesWon,
					LifetimeGamesPlayed = lifetimeGamesPlayed
					};

				// Find the correct team ID based on the name
				int v = LoadTeams().FirstOrDefault(t => t.TeamName == teamName)?.TeamId ?? 0;

				players.Add(new Player(playerName, teamName, skillLevel, stats,  v));
				}
			}

		return players;
		}

	// Save players to CSV
	public void SavePlayersToCsv(List<Player> players)
		{
		List<string> lines = new()
			{
			"PlayerName,TeamName,SkillLevel,LifetimeGamesWon,LifetimeGamesPlayed"
		};

		foreach (var player in players)
			{
			string line = $"{player.PlayerName},{player.TeamName},{player.SkillLevel}," +
						  $"{player.Stats.LifetimeGamesWon},{player.Stats.LifetimeGamesPlayed}";
			lines.Add(line);
			}

		File.WriteAllLines(playersDataFilePath, lines);
		Debug.Log("Players data saved to CSV.");
		}

	// Save teams to CSV
	public void SaveTeams(List<Team> teams)
		{
		List<string> lines = new()
			{
			"TeamId,TeamName"
		};

		foreach (var team in teams)
			{
			string line = $"{team.TeamId},{team.TeamName}";
			lines.Add(line);
			}

		File.WriteAllLines(teamsDataFilePath, lines);
		Debug.Log("Teams data saved to CSV.");
		}

	// Delete a team by ID
	public void DeleteTeam(int teamId)
		{
		List<Team> teams = LoadTeams();
		Team teamToDelete = teams.FirstOrDefault(t => t.TeamId == teamId);
		if (teamToDelete != null)
			{
			teams.Remove(teamToDelete);
			SaveTeams(teams);
			Debug.Log($"Team {teamToDelete.TeamName} deleted.");
			}
		else
			{
			Debug.Log($"Team with ID {teamId} not found.");
			}
		}

	internal void DeleteTeam(Team currentSelectedTeam)
		{
		throw new NotImplementedException();
		}
	}
