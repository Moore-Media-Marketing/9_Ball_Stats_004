using System.Collections.Generic;

using UnityEngine;

public class SampleDataGenerator:MonoBehaviour
	{
	public int numberOfTeams = 10;
	public int numberOfPlayersPerTeam = 8;
	public PlayerWeightSettings weightSettings;

	private static readonly string[] firstNamesMale = { "James", "John", "Robert", "Michael", "William" };
	private static readonly string[] firstNamesFemale = { "Mary", "Patricia", "Linda", "Barbara", "Elizabeth" };
	private static readonly string[] lastNames = { "Smith", "Johnson", "Williams", "Jones", "Brown" };
	private static readonly string[] teamNames = { "Sharks", "Tigers", "Wolves", "Eagles", "Lions" };

	private Dictionary<int, int> skillLevelToPoints = new()
	{
		{ 1, 10 }, { 2, 20 }, { 3, 30 }, { 4, 40 }, { 5, 50 },
		{ 6, 60 }, { 7, 70 }, { 8, 80 }, { 9, 90 }
	};

	private void Start()
		{
		// Generate sample teams and players when the game starts
		GenerateSampleTeamsAndPlayers();
		}

	private void GenerateSampleTeamsAndPlayers()
		{
		try
			{
			Debug.Log("Generating sample teams and players...");
			List<Player> players = new();
			int teamId = 1;
			HashSet<string> existingTeamNames = new(); // To check for duplicate team names

			// Iterate over the predefined team names
			foreach (string teamName in teamNames)
				{
				if (existingTeamNames.Contains(teamName))
					{
					Debug.LogWarning($"Team name '{teamName}' already exists. Skipping...");
					continue; // Skip team creation if duplicate is found
					}

				existingTeamNames.Add(teamName);

				// Generate players for the current team
				for (int j = 0; j < numberOfPlayersPerTeam; j++)
					{
					string firstName = (Random.Range(0, 2) == 0)
						? firstNamesMale[Random.Range(0, firstNamesMale.Length)]
						: firstNamesFemale[Random.Range(0, firstNamesFemale.Length)];

					string lastName = lastNames[Random.Range(0, lastNames.Length)];
					string playerName = $"{firstName} {lastName}"; // Concatenate first and last name

					int skillLevel = WeightedRandomSkillLevel(); // Get weighted random skill level
					PlayerStats stats = GenerateRandomPlayerStats(skillLevel); // Generate random stats for player

					// Create player and add to the list
					Player player = new(playerName, teamName, skillLevel, stats, teamId);
					players.Add(player);
					}
				teamId++;
				}

			// Save players to CSV
			try
				{
				DatabaseManager.Instance.SavePlayersToCsv(players); // Save to database
				Debug.Log("Sample data generated and saved successfully.");
				}
			catch (System.Exception ex)
				{
				Debug.LogError("Error saving player data to CSV: " + ex.Message);
				}
			}
		catch (System.Exception ex)
			{
			Debug.LogError("Error generating sample data: " + ex.Message);
			}
		}

	private int WeightedRandomSkillLevel()
		{
		// Weighted random skill level where higher skill levels are more likely
		int[] weightDistribution = { 1, 2, 3, 4, 5, 6, 7, 8, 9 }; // Weighted favoring higher numbers
		return weightDistribution[Random.Range(0, weightDistribution.Length)];
		}

	private PlayerStats GenerateRandomPlayerStats(int skillLevel)
		{
		PlayerStats stats = new()
			{
			// Current season stats
			CurrentSeasonMatchesPlayed = Random.Range(5, 30),
			CurrentSeasonMatchesWon = Random.Range(0, 30),
			CurrentSeasonBreakAndRun = Random.Range(0, 10),
			CurrentSeasonDefensiveShotAverage = Random.Range(0f, 1f),
			CurrentSeasonMiniSlams = Random.Range(0, 5),
			CurrentSeasonNineOnTheSnap = Random.Range(0, 3),
			CurrentSeasonPaPercentage = Random.Range(50f, 100f),
			CurrentSeasonPointsAwarded = Random.Range(0, 100),
			CurrentSeasonPpm = Random.Range(0f, 5f),
			CurrentSeasonShutouts = Random.Range(0, 5),
			CurrentSeasonTotalPoints = skillLevelToPoints.ContainsKey(skillLevel) ? skillLevelToPoints[skillLevel] : 0,

			// Lifetime stats
			LifetimeGamesPlayed = Random.Range(50, 200),
			LifetimeGamesWon = Random.Range(30, 150),
			LifetimeMatchesPlayed = Random.Range(100, 300),
			LifetimeMatchesWon = Random.Range(50, 200),
			LifetimeMiniSlams = Random.Range(0, 10),
			LifetimeNineOnTheSnap = Random.Range(0, 5),
			LifetimeShutouts = Random.Range(0, 10),
			LifetimeDefensiveShotAverage = Random.Range(0f, 1f),
			LifetimeBreakAndRun = Random.Range(0, 20),
			LifetimePointsAwarded = Random.Range(100, 500),
			LifetimePpm = Random.Range(0f, 5f),
			LifetimeTotalPoints = Random.Range(0, 500),
			LifetimeMatchesPlayedInLast2Years = Random.Range(20, 100)
			};

		// Apply weight adjustments based on the PlayerWeightSettings
		ApplyWeightAdjustments(ref stats);
		return stats;
		}

	private void ApplyWeightAdjustments(ref PlayerStats stats)
		{
		// Adjust stats based on weight settings for both current season and lifetime stats
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

		// Lifetime Stats, explicitly casting to int for the required fields
		stats.LifetimeGamesWon = (int) (stats.LifetimeGamesWon * weightSettings.weightLifetimeGamesWon);
		stats.LifetimeMiniSlams = (int) (stats.LifetimeMiniSlams * weightSettings.weightLifetimeMiniSlams);
		stats.LifetimeNineOnTheSnap = (int) (stats.LifetimeNineOnTheSnap * weightSettings.weightLifetimeNineOnTheSnap);
		stats.LifetimeShutouts = (int) (stats.LifetimeShutouts * weightSettings.weightLifetimeShutouts);
		stats.LifetimeBreakAndRun = (int) (stats.LifetimeBreakAndRun * weightSettings.weightLifetimeBreakAndRun);

		// The following are floats, no casting needed:
		stats.LifetimeDefensiveShotAverage *= weightSettings.weightLifetimeDefensiveShotAverage;
		stats.LifetimeMatchesPlayed = (int) (stats.LifetimeMatchesPlayed * weightSettings.weightLifetimeMatchesPlayed);
		stats.LifetimeMatchesWon = (int) (stats.LifetimeMatchesWon * weightSettings.weightLifetimeMatchesWon);
		}
	}
