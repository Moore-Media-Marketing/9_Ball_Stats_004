using System.Collections.Generic;
using System.IO;
using System.Text;

using UnityEngine;

public class SampleDataGenerator:MonoBehaviour
	{
	public int numberOfTeams = 10; // The number of teams to generate
	public int numberOfPlayersPerTeam = 8; // The number of players per team

	// Sample Data Arrays for names and teams
	private static readonly string[] firstNamesMale = { "James", "John", "Robert", "Michael", "William" };
	private static readonly string[] firstNamesFemale = { "Mary", "Patricia", "Linda", "Barbara", "Elizabeth" };
	private static readonly string[] lastNames = { "Smith", "Johnson", "Williams", "Jones", "Brown" };
	private static readonly string[] teamNames = { "Sharks", "Tigers", "Wolves", "Eagles", "Lions" };

	private string playersFilePath;
	private string teamsFilePath;

	private void Start()
		{
		playersFilePath = Path.Combine(Application.persistentDataPath, "players.csv");
		teamsFilePath = Path.Combine(Application.persistentDataPath, "teams.csv");

		GenerateSampleTeamsAndPlayers();
		}

	private void GenerateSampleTeamsAndPlayers()
		{
		List<Team> teams = new();
		for (int i = 0; i < numberOfTeams; i++)
			{
			// For simplicity, use teamNames cyclically.
			string teamName = teamNames[i % teamNames.Length];
			teams.Add(new Team(i + 1, teamName));
			}
		SaveTeams(teams, teamsFilePath);

		List<Player> players = new();
		foreach (Team team in teams)
			{
			for (int j = 0; j < numberOfPlayersPerTeam; j++)
				{
				string firstName = (Random.Range(0, 2) == 0) ? firstNamesMale[Random.Range(0, firstNamesMale.Length)] : firstNamesFemale[Random.Range(0, firstNamesFemale.Length)];
				string lastName = lastNames[Random.Range(0, lastNames.Length)];
				string playerName = $"{firstName} {lastName}";
				int skillLevel = Random.Range(1, 10);

				// Generate random values for each required parameter:
				int currentSeasonMatchesPlayed = Random.Range(10, 50);
				int currentSeasonMatchesWon = Random.Range(0, currentSeasonMatchesPlayed);
				int currentSeasonPointsAwarded = Random.Range(0, 100);
				float currentSeasonPointsPerMatch = (float) currentSeasonPointsAwarded / currentSeasonMatchesPlayed;
				int currentSeasonBreakAndRun = Random.Range(0, 5);
				float currentSeasonDefensiveShotAverage = Random.Range(0f, 1f);
				int currentSeasonShutouts = Random.Range(0, 5);
				// We'll set CurrentSeasonPaPercentage arbitrarily:
				float currentSeasonPaPercentage = Random.Range(0f, 1f);
				int currentSeasonTotalPoints = Random.Range(0, 100);

				int lifetimeGamesPlayed = Random.Range(100, 300);
				int lifetimeGamesWon = Random.Range(30, lifetimeGamesPlayed);
				int lifetimeMiniSlams = Random.Range(0, 5);
				int lifetimeNineOnTheSnap = Random.Range(0, 5);
				int lifetimeShutouts = Random.Range(0, 5);
				int lifetimeMatchesPlayedInLast2Years = Random.Range(10, 50);
				int lifetimeMatchesPlayed = Random.Range(100, 300);
				int lifetimeMatchesWon = Random.Range(30, lifetimeMatchesPlayed);
				int lifetimeBreakAndRun = Random.Range(0, 5);
				float lifetimeDefensiveShotAverage = Random.Range(0f, 1f);

				// Create the player using all 21 parameters in the correct order.
				Player player = new(
					team.TeamId,                   // teamId
					team.TeamName,                 // teamName
					playerName,                    // playerName
					skillLevel,                    // skillLevel
					currentSeasonMatchesPlayed,    // currentSeasonMatchesPlayed
					currentSeasonMatchesWon,       // currentSeasonMatchesWon
					currentSeasonPointsAwarded,    // currentSeasonPointsAwarded
					currentSeasonPointsPerMatch,   // currentSeasonPointsPerMatch
					currentSeasonBreakAndRun,      // currentSeasonBreakAndRun
					currentSeasonDefensiveShotAverage, // currentSeasonDefensiveShotAverage
					currentSeasonShutouts,         // currentSeasonShutouts
					lifetimeGamesPlayed,           // lifetimeGamesPlayed
					lifetimeGamesWon,              // lifetimeGamesWon
					lifetimeMiniSlams,             // lifetimeMiniSlams
					lifetimeNineOnTheSnap,         // lifetimeNineOnTheSnap
					lifetimeShutouts,              // lifetimeShutouts
					lifetimeMatchesPlayedInLast2Years, // lifetimeMatchesPlayedInLast2Years
					lifetimeMatchesPlayed,         // lifetimeMatchesPlayed
					lifetimeMatchesWon,            // lifetimeMatchesWon
					lifetimeBreakAndRun,           // lifetimeBreakAndRun
					lifetimeDefensiveShotAverage   // lifetimeDefensiveShotAverage
				);
				players.Add(player);
				}
			}
		SavePlayersToCsv(players, playersFilePath);
		Debug.Log("Sample data generation complete.");
		}

	private void SavePlayersToCsv(List<Player> players, string path)
		{
		try
			{
			StringBuilder sb = new();
			foreach (Player player in players)
				{
				sb.AppendLine(player.ToCsv());
				}
			File.WriteAllText(path, sb.ToString(), System.Text.Encoding.UTF8);
			}
		catch (System.Exception ex)
			{
			Debug.LogError($"Error saving players to CSV: {ex.Message}");
			}
		}

	private void SaveTeams(List<Team> teams, string path)
		{
		try
			{
			StringBuilder sb = new();
			foreach (Team team in teams)
				{
				sb.AppendLine(team.ToCsv());
				}
			File.WriteAllText(path, sb.ToString(), System.Text.Encoding.UTF8);
			}
		catch (System.Exception ex)
			{
			Debug.LogError($"Error saving teams to CSV: {ex.Message}");
			}
		}
	}
