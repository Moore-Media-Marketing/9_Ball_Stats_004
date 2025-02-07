using System.Collections.Generic;

using UnityEngine;

// --- SampleDataGenerator Class --- //
public class SampleDataGenerator:MonoBehaviour
	{
	public int numberOfTeams = 10;
	public int numberOfPlayersPerTeam = 8;

	private static readonly string[] firstNamesMale = { "James", "John", "Robert", "Michael", "William" };
	private static readonly string[] firstNamesFemale = { "Mary", "Patricia", "Linda", "Barbara", "Elizabeth" };
	private static readonly string[] lastNames = { "Smith", "Johnson", "Williams", "Jones", "Brown" };
	private static readonly string[] teamNames = { "Sharks", "Tigers", "Wolves", "Eagles", "Lions" };

	// --- Initialize Data Generation --- //
	private void Start()
		{
		GenerateSampleTeamsAndPlayers();
		}

	// --- Generate Sample Teams and Players --- //
	private void GenerateSampleTeamsAndPlayers()
		{
		try
			{
			Debug.Log("Generating sample teams and players...");

			List<Player> players = new();
			int teamId = 1;
			int playerId = 1;

			foreach (string teamName in teamNames)
				{
				for (int j = 0; j < numberOfPlayersPerTeam; j++)
					{
					// --- Generate Player Name --- //
					string firstName = (Random.Range(0, 2) == 0)
						? firstNamesMale[Random.Range(0, firstNamesMale.Length)]
						: firstNamesFemale[Random.Range(0, firstNamesFemale.Length)];

					string lastName = lastNames[Random.Range(0, lastNames.Length)];
					string playerName = $"{firstName} {lastName}";

					// --- Randomly Generate Player Stats --- //
					int skillLevel = Random.Range(1, 10);
					int currentSeasonMatchesPlayed = Random.Range(1, 30);
					int currentSeasonMatchesWon = Random.Range(1, currentSeasonMatchesPlayed);
					int lifetimeMatchesPlayedInLast2Years = Random.Range(10, 100);
					int lifetimeMatchesWon = Random.Range(5, lifetimeMatchesPlayedInLast2Years);
					int lifetimeMiniSlams = Random.Range(0, 5);
					int currentSeasonPointsAwarded = Random.Range(10, 150);
					int lifetimeGamesPlayed = Random.Range(50, 500);
					int lifetimeGamesWon = Random.Range(20, lifetimeGamesPlayed);
					float lifetimeDefensiveShotAverage = Random.Range(0.1f, 0.9f);
					int lifetimeShutouts = Random.Range(0, 10);
					int currentSeasonShutouts = Random.Range(0, 5);

					// --- Create Player Object --- //
					Player player = new(
						playerName, skillLevel, teamId, teamName, lifetimeGamesWon, lifetimeGamesPlayed,
						lifetimeDefensiveShotAverage, lifetimeMatchesPlayedInLast2Years, lifetimeMiniSlams, lifetimeShutouts,
						currentSeasonPointsAwarded, currentSeasonMatchesPlayed, currentSeasonMatchesWon
					);

					players.Add(player);
					playerId++;
					}
				teamId++;
				}

			// --- Save Players to CSV --- //
			DatabaseManager.Instance.SavePlayers(players);
			Debug.Log("Sample data generated and saved successfully.");
			}
		catch (System.Exception ex)
			{
			Debug.LogError("Error generating sample data: " + ex.Message);
			}
		}
	}
