using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using UnityEngine;

public class DatabaseManager:MonoBehaviour
	{
	public static DatabaseManager Instance { get; private set; }

	private string playersCsvPath;
	private string matchupsCsvPath;

	private void Awake()
		{
		if (Instance != null && Instance != this)
			{
			Destroy(gameObject);
			return;
			}
		Instance = this;
		DontDestroyOnLoad(gameObject);

		playersCsvPath = Path.Combine(Application.persistentDataPath, "players.csv");
		matchupsCsvPath = Path.Combine(Application.persistentDataPath, "matchups.csv");
		}

	public List<Player> LoadPlayersFromCsv(int teamId)
		{
		List<Player> players = new List<Player>();
		if (!File.Exists(playersCsvPath)) return players;

		try
			{
			using StreamReader reader = new StreamReader(playersCsvPath, Encoding.UTF8);
			string line;
			while ((line = reader.ReadLine()) != null)
				{
				line = line.Trim();
				if (string.IsNullOrEmpty(line)) continue;

				var player = Player.FromCsv(line);
				if (player.TeamId == teamId) players.Add(player);
				}
			}
		catch (Exception ex)
			{
			Debug.LogError($"Error loading players from CSV: {ex.Message}");
			}

		return players;
		}

	public void SavePlayersToCsv(List<Player> players)
		{
		try
			{
			StringBuilder csvBuilder = new StringBuilder();
			csvBuilder.AppendLine("PlayerId,TeamId,PlayerName,SkillLevel,DefensiveShotAvg,LifetimeMatchesPlayedInLast2Years,LifetimeGamesPlayed,LifetimeMatchesWon,LifetimeMiniSlams,LifetimeNineOnTheSnap,LifetimeShutouts,CurrentSeasonBreakAndRun,CurrentSeasonDefensiveShotAverage,CurrentSeasonMatchesPlayed,CurrentSeasonMatchesWon,CurrentSeasonMiniSlams,CurrentSeasonNineOnTheSnap,CurrentSeasonShutouts,CurrentSeasonSkillLevel,CurrentSeasonPointsAwarded,CurrentSeasonPointsPerMatch,CurrentSeasonPpm,CurrentSeasonTotalPoints");

			foreach (var player in players)
				csvBuilder.AppendLine(player.ToCsv());

			File.WriteAllText(playersCsvPath, csvBuilder.ToString(), Encoding.UTF8);
			}
		catch (Exception ex)
			{
			Debug.LogError($"Error saving players to CSV: {ex.Message}");
			}
		}
	}
