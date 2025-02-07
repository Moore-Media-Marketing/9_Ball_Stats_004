using System;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

public class DatabaseManager:MonoBehaviour
	{
	// Singleton pattern for global access to the DatabaseManager
	public static DatabaseManager Instance { get; private set; }

	private string filePath = "teamsandplayers.csv";

	// --- Initialize Singleton --- //
	private void Awake()
		{
		// Check if instance already exists
		if (Instance != null && Instance != this)
			{
			Destroy(this.gameObject);
			}
		else
			{
			Instance = this;
			}
		}

	// --- Load Players from CSV File --- //
	public List<Player> LoadPlayers()
		{
		List<Player> players = new List<Player>();
		try
			{
			// Check if the file exists
			if (File.Exists(filePath))
				{
				string[] allLines = File.ReadAllLines(filePath);
				foreach (string line in allLines)
					{
					// Skip header or empty lines
					if (string.IsNullOrEmpty(line) || line.StartsWith("PlayerName"))
						continue;

					players.Add(Player.FromCsv(line));
					}
				Debug.Log("Players loaded successfully.");
				}
			else
				{
				Debug.LogError($"File {filePath} not found!");
				}
			}
		catch (Exception ex)
			{
			Debug.LogError("Error loading players: " + ex.Message);
			}
		return players;
		}

	// --- Save Players to CSV File --- //
	public void SavePlayers(List<Player> players)
		{
		try
			{
			// Prepare the CSV content with a header
			List<string> lines = new List<string>
			{
				"PlayerName,SkillLevel,TeamId,TeamName,LifetimeGamesWon,LifetimeGamesPlayed,LifetimeDefensiveShotAverage," +
				"LifetimeMatchesPlayedInLast2Years,LifetimeMiniSlams,LifetimeShutouts,LifetimeBreakAndRun,LifetimeNineOnTheSnap," +
				"CurrentSeasonPointsAwarded,CurrentSeasonMatchesPlayed,CurrentSeasonMatchesWon"
			};

			// Add each player to the CSV content
			foreach (Player player in players)
				{
				lines.Add(player.ToCsv());
				}

			// Write the data to the CSV file
			File.WriteAllLines(filePath, lines);
			Debug.Log("Players saved successfully.");
			}
		catch (Exception ex)
			{
			Debug.LogError("Error saving players: " + ex.Message);
			}
		}

	// --- Update Player Data --- //
	public void UpdatePlayer(Player updatedPlayer)
		{
		try
			{
			List<Player> players = LoadPlayers();
			for (int i = 0; i < players.Count; i++)
				{
				if (players[i].TeamId == updatedPlayer.TeamId && players[i].PlayerName == updatedPlayer.PlayerName)
					{
					players[i] = updatedPlayer;
					break;
					}
				}
			SavePlayers(players);
			}
		catch (Exception ex)
			{
			Debug.LogError("Error updating player data: " + ex.Message);
			}
		}

	// --- Delete Player Data --- //
	public void DeletePlayer(Player playerToDelete)
		{
		try
			{
			List<Player> players = LoadPlayers();
			players.RemoveAll(player => player.TeamId == playerToDelete.TeamId && player.PlayerName == playerToDelete.PlayerName);
			SavePlayers(players);
			}
		catch (Exception ex)
			{
			Debug.LogError("Error deleting player: " + ex.Message);
			}
		}
	}
