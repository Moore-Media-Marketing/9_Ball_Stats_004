using System.Collections.Generic;
using System.Linq;

using UnityEngine;

/// <summary>
/// Generates sample teams and players for testing purposes.
/// </summary>
public class SampleDataGenerator:MonoBehaviour
	{
	// --- Region: Public Variables --- //

	[Tooltip("The number of teams to generate.")]
	public int numberOfTeams = 10;

	// --- End Region: Public Variables --- //

	// --- Region: Sample Data --- //

	private static readonly string[] firstNamesMale = { "James", "John", "Robert", "Michael", "William", "David", "Richard", "Charles", "Joseph", "Thomas" };
	private static readonly string[] firstNamesFemale = { "Mary", "Patricia", "Linda", "Barbara", "Elizabeth", "Jennifer", "Maria", "Susan", "Margaret", "Dorothy" };
	private static readonly string[] lastNames = { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez" };
	private static readonly string[] teamNames = { "Atomic Squirrels", "Crimson Cobras", "Electric Eels", "Phantom Phantoms", "Mystic Moose" };

	// --- End Region: Sample Data --- //

	// --- Region: Handicap Points for Each Skill Level --- //

	private readonly int[] pointsRequiredToWin = { 14, 19, 25, 31, 38, 46, 55, 65, 75 }; // Points required based on skill level (1 to 9)

	// --- End Region: Handicap Points for Each Skill Level --- //

	// --- Region: Start Method --- //

	private void Start()
		{
		GenerateSampleTeamsAndPlayers();
		}

	// --- End Region: Start Method --- //

	// --- Region: Generate Sample Teams and Players --- //

	private void GenerateSampleTeamsAndPlayers()
		{
		if (DatabaseManager.Instance == null)
			{
			Debug.LogError("DatabaseManager instance is null. Cannot generate sample data.");
			return;
			}

		for (int i = 0; i < numberOfTeams; i++)
			{
			Team newTeam = GenerateTeam();
			DatabaseManager.Instance.AddTeam(newTeam);

			int numberOfPlayersForTeam = Random.Range(5, 9);

			for (int j = 0; j < numberOfPlayersForTeam; j++)
				{
				Player newPlayer = GeneratePlayer(newTeam.id);
				DatabaseManager.Instance.AddPlayer(newPlayer);
				}

			SaveToPlayerPrefs();
			}
		Debug.Log("Sample teams and players generated successfully.");
		}

	// --- End Region: Generate Sample Teams and Players --- //

	// --- Region: Generate Team Method --- //

	private Team GenerateTeam()
		{
		string teamName = teamNames[Random.Range(0, teamNames.Length)];
		var allTeams = DatabaseManager.Instance.GetAllTeams();

		int teamId = allTeams.Any() ? allTeams.Max(t => t.id) + 1 : 1;

		Team newTeam = new(teamId, teamName);
		Debug.Log($"Generated Team: {newTeam.name} (ID: {newTeam.id})");

		return newTeam;
		}

	// --- End Region: Generate Team Method --- //

	// --- Region: Generate Player Method --- //

	private Player GeneratePlayer(int teamId)
		{
		string firstName = Random.Range(0, 2) == 0 ? firstNamesMale[Random.Range(0, firstNamesMale.Length)] : firstNamesFemale[Random.Range(0, firstNamesFemale.Length)];
		string lastName = lastNames[Random.Range(0, lastNames.Length)];

		string playerName = $"{firstName} {lastName}";
		int skillLevel = Random.Range(1, 10);

		Player newPlayer = new(playerName, skillLevel, teamId)
			{
			PointsRequiredToWin = pointsRequiredToWin[skillLevel - 1],
			lifetimeMatchesPlayed = Random.Range(0, 100),
			lifetimeMatchesWon = Random.Range(0, 50),
			lifetimeDefensiveShotAvg = Random.Range(0f, 10f),
			lifetimeGamesPlayed = Random.Range(0, 200),
			lifetimeGamesWon = Random.Range(0, 150),
			lifetimeMiniSlams = Random.Range(0, 10),
			lifetimeNineOnTheSnap = Random.Range(0, 5),
			lifetimeShutouts = Random.Range(0, 10),
			currentSeasonBreakAndRun = Random.Range(0, 5),
			currentSeasonDefensiveShotAverage = Random.Range(0f, 10f),
			currentSeasonMatchesPlayed = Random.Range(0, 50),
			currentSeasonMatchesWon = Random.Range(0, 30),
			currentSeasonMiniSlams = Random.Range(0, 5),
			currentSeasonNineOnTheSnap = Random.Range(0, 3),
			currentSeasonPaPercentage = Random.Range(0f, 100f),
			currentSeasonPointsAwarded = Random.Range(0, 100),
			currentSeasonPointsPerMatch = Random.Range(0f, 10f),
			currentSeasonShutouts = Random.Range(0, 5),
			currentSeasonSkillLevel = skillLevel,
			currentSeasonTotalPoints = Random.Range(0, 200)
			};

		Debug.Log($"Generated Player: {newPlayer.name} (Skill Level: {newPlayer.skillLevel}) for Team ID: {teamId}");

		return newPlayer;
		}

	// --- End Region: Generate Player Method --- //

	// --- Region: Save to PlayerPrefs --- //

	private void SaveToPlayerPrefs()
		{
		var allTeams = DatabaseManager.Instance.GetAllTeams();
		foreach (var team in allTeams)
			{
			PlayerPrefs.SetString($"Team_{team.id}_Name", team.name);
			PlayerPrefs.SetString($"Team_{team.id}_PlayerSkillLevels", string.Join(",", team.playerSkillLevels));
			}

		PlayerPrefs.Save();
		Debug.Log("Data saved to PlayerPrefs.");
		}

	// --- End Region: Save to PlayerPrefs --- //

	// --- Region: Additional Functions --- //
	// Any other utility functions would go here
	// --- End Region: Additional Functions --- //
	}
