using System.Collections.Generic;

using UnityEngine;

public class SampleDataGenerator:MonoBehaviour
	{
	// Reference to PlayerWeightSettings ScriptableObject.
	public PlayerWeightSettings weightSettings;
	private object skillLevelToPoints;

	// Method to generate random player stats with adjustable weights
	public void GenerateAndSavePlayers()
		{
		// Load teams from DatabaseManager (assuming this will provide a list of teams)
		List<Team> teams = DatabaseManager.Instance.LoadTeams();

		// Create a list to hold generated players
		List<Player> generatedPlayers = new();

		// Generate players for each team (for demonstration, we'll generate 5 players per team)
		foreach (var team in teams)
			{
			for (int i = 0; i < 5; i++)  // Generate 5 players for each team
				{
				string playerName = $"Player_{i + 1}_{team.TeamName}";  // Fix the naming to correctly represent the player
				Player player = GenerateRandomPlayer(playerName, team.TeamName, team.TeamId);  // Pass teamId to player
				generatedPlayers.Add(player);
				}
			}

		// Save generated players to the CSV file via DatabaseManager
		DatabaseManager.Instance.SavePlayersToCsv(generatedPlayers);
		}

	// Method to generate random player stats with adjustable weights
	public Player GenerateRandomPlayer(string playerName, string teamName, int teamId)
		{
		// Generate random skill level between 1 and 9
		int skillLevel = UnityEngine.Random.Range(1, 10);

		// Generate the player's stats
		PlayerStats stats = GenerateRandomPlayerStats(skillLevel, GetSkillLevelToPoints());

		// Adjust stats based on weight settings from PlayerWeightSettings
		stats.CurrentSeasonPointsAwarded *= (int) weightSettings.weightCurrentSeasonPointsAwarded;
		stats.CurrentSeasonMatchesWon *= (int) weightSettings.weightCurrentSeasonMatchesWon;
		stats.CurrentSeasonDefensiveShotAverage *= weightSettings.weightCurrentSeasonDefensiveShotAverage;
		stats.CurrentSeasonSkillLevel *= (int) weightSettings.weightCurrentSeasonSkillLevel;
		stats.CurrentSeasonPpm *= weightSettings.weightCurrentSeasonPpm;
		stats.CurrentSeasonShutouts *= (int) weightSettings.weightCurrentSeasonShutouts;
		stats.CurrentSeasonMiniSlams *= (int) weightSettings.weightCurrentSeasonMiniSlams;
		stats.CurrentSeasonNineOnTheSnap *= (int) weightSettings.weightCurrentSeasonNineOnTheSnap;
		stats.CurrentSeasonPaPercentage *= weightSettings.weightCurrentSeasonPaPercentage;
		stats.CurrentSeasonBreakAndRun *= (int) weightSettings.weightCurrentSeasonBreakAndRun;

		stats.LifetimeGamesWon *= (int) weightSettings.weightLifetimeGamesWon;
		stats.LifetimeMiniSlams *= (int) weightSettings.weightLifetimeMiniSlams;
		stats.LifetimeNineOnTheSnap *= (int) weightSettings.weightLifetimeNineOnTheSnap;
		stats.LifetimeShutouts *= (int) weightSettings.weightLifetimeShutouts;
		stats.LifetimeBreakAndRun *= (int) weightSettings.weightLifetimeBreakAndRun;
		stats.LifetimeDefensiveShotAverage *= weightSettings.weightLifetimeDefensiveShotAverage;
		stats.LifetimeMatchesPlayed *= (int) weightSettings.weightLifetimeMatchesPlayed;
		stats.LifetimeMatchesWon *= (int) weightSettings.weightLifetimeMatchesWon;

		// Return the newly created player with correct teamId
		return new Player(playerName, teamName, skillLevel, stats, teamId);  // Pass teamId here
		}

	private object GetSkillLevelToPoints()
		{
		return skillLevelToPoints;
		}

	// Method to generate random stats based on skill level and handicap system
	private PlayerStats GenerateRandomPlayerStats(int skillLevel, object skillLevelToPoints)
		{
		PlayerStats stats = new()
			{
			// Generate random values for current season stats
			CurrentSeasonMatchesPlayed = UnityEngine.Random.Range(5, 30)
			};
		stats.CurrentSeasonMatchesWon = UnityEngine.Random.Range(0, stats.CurrentSeasonMatchesPlayed);
		stats.CurrentSeasonBreakAndRun = UnityEngine.Random.Range(0, 10);
		stats.CurrentSeasonDefensiveShotAverage = UnityEngine.Random.Range(0f, 1f);
		stats.CurrentSeasonMiniSlams = UnityEngine.Random.Range(0, 5);
		stats.CurrentSeasonNineOnTheSnap = UnityEngine.Random.Range(0, 3);
		stats.CurrentSeasonPaPercentage = UnityEngine.Random.Range(50f, 100f);
		stats.CurrentSeasonPointsAwarded = UnityEngine.Random.Range(0, 100);
		stats.CurrentSeasonPointsPerMatch = UnityEngine.Random.Range(1f, 10f);
		stats.CurrentSeasonPpm = UnityEngine.Random.Range(0f, 5f);
		stats.CurrentSeasonShutouts = UnityEngine.Random.Range(0, 5);

		// Apply handicap system for points based on skill level
		stats.CurrentSeasonTotalPoints = (int) skillLevelToPoints.GetType().GetProperty("skillLevel").GetValue(skillLevelToPoints);

		// Generate random values for lifetime stats
		stats.LifetimeGamesPlayed = UnityEngine.Random.Range(50, 200);
		stats.LifetimeGamesWon = UnityEngine.Random.Range(30, 150);
		stats.LifetimeMatchesPlayed = UnityEngine.Random.Range(100, 300);
		stats.LifetimeMatchesWon = UnityEngine.Random.Range(50, 200);
		stats.LifetimeMiniSlams = UnityEngine.Random.Range(0, 10);
		stats.LifetimeNineOnTheSnap = UnityEngine.Random.Range(0, 5);
		stats.LifetimeShutouts = UnityEngine.Random.Range(0, 10);
		stats.LifetimeDefensiveShotAverage = UnityEngine.Random.Range(0f, 1f);
		stats.LifetimeBreakAndRun = UnityEngine.Random.Range(0, 20);
		stats.LifetimePointsAwarded = UnityEngine.Random.Range(100, 500);
		stats.LifetimePointsPerMatch = UnityEngine.Random.Range(1f, 10f);
		stats.LifetimePpm = UnityEngine.Random.Range(0f, 5f);
		stats.LifetimeTotalPoints = UnityEngine.Random.Range(0, 500);
		stats.LifetimeMatchesPlayedInLast2Years = UnityEngine.Random.Range(20, 100);

		return stats;
		}
	}
