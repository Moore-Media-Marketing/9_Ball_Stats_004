using System.Collections.Generic;
using System.Linq;  // --- Importing Linq for handling collections ---

using UnityEngine;  // --- Importing UnityEngine for Unity-related functionality ---

// --- Region: Sample Data Generator Class Definition --- //
public class SampleDataGenerator:MonoBehaviour
	{
	// --- Region: Public Variables --- //
	[Tooltip("The number of teams to generate.")]
	public int numberOfTeams = 10; // --- Number of teams to generate ---

	// --- End Region --- //

	// --- Region: Sample Data --- //
	private static readonly string[] firstNamesMale = { "James", "John", "Robert", "Michael", "William", "David", "Richard", "Charles", "Joseph", "Thomas" };

	private static readonly string[] firstNamesFemale = { "Mary", "Patricia", "Linda", "Barbara", "Elizabeth", "Jennifer", "Maria", "Susan", "Margaret", "Dorothy" };
	private static readonly string[] lastNames = { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez" };
	private static readonly string[] teamNames = { "Atomic Squirrels", "Crimson Cobras", "Electric Eels", "Phantom Phantoms", "Mystic Moose" };
	// --- End Region --- //

	// --- Region: Handicap Points for Each Skill Level --- //
	private readonly int[] pointsRequiredToWin = { 14, 19, 25, 31, 38, 46, 55, 65, 75 }; // --- Points required based on skill level (1 to 9) ---

	// --- End Region --- //

	// --- Region: Start Method --- //
	private void Start()
		{
		GenerateSampleTeamsAndPlayers(); // --- Generate sample data on start ---
		}

	// --- End Region --- //

	// --- Region: Generate Sample Data --- //
	public void GenerateSampleTeamsAndPlayers() // --- Make this method public ---
		{
		if (DatabaseManager.Instance == null)
			{
			Debug.LogError("DatabaseManager instance is null. Cannot generate sample data.");
			return;
			}

		for (int i = 0; i < numberOfTeams; i++)
			{
			Team newTeam = GenerateTeam(); // --- Generate a new team ---
			DatabaseManager.Instance.AddTeam(newTeam); // --- Add team to database ---

			int numberOfPlayersForTeam = Random.Range(5, 9); // --- Randomly choose number of players for the team ---

			List<int> playerSkillLevels = new(); // --- List to store player skill levels ---
			List<string> playerNames = new();  // --- List to store player names ---

			for (int j = 0; j < numberOfPlayersForTeam; j++)
				{
				Player newPlayer = GeneratePlayer(newTeam.id); // --- Generate a new player for the team ---
				DatabaseManager.Instance.AddPlayer(newPlayer); // --- Add player to database ---
				playerSkillLevels.Add(newPlayer.skillLevel); // --- Add player skill level to the list ---
				playerNames.Add(newPlayer.name); // --- Add player name to the list ---
				}

			SaveToJson(newTeam, playerNames, playerSkillLevels); // --- Save data after generating ---
			}

		Debug.Log("Sample teams and players generated successfully.");
		}

	// --- End Region --- //

	// --- Region: Clear Sample Data --- //
	public void ClearSampleData() // --- Public method to clear the sample data ---
		{
		// Clear all JSON data (consider clearing file or database instead of PlayerPrefs)
		// Example: File.Delete(pathToJsonData);
		Debug.Log("Sample Data cleared.");
		}

	// --- End Region --- //

	// --- Region: Generate Team Method --- //
	private Team GenerateTeam()
		{
		string teamName = teamNames[Random.Range(0, teamNames.Length)];
		var allTeams = DatabaseManager.Instance.GetAllTeams(); // --- Get all existing teams ---

		// --- Generate a new team ID by checking the max ID from existing teams ---
		int teamId = allTeams.Any() ? allTeams.Max(t => t.id) + 1 : 1;

		Team newTeam = new(teamId, teamName); // --- Create a new team ---
		Debug.Log($"Generated Team: {newTeam.name} (ID: {newTeam.id})");

		return newTeam;
		}

	// --- End Region --- //

	// --- Region: Generate Player Method --- //
	private Player GeneratePlayer(int teamId)
		{
		// --- Randomly select first name and last name ---
		string firstName = Random.Range(0, 2) == 0 ? firstNamesMale[Random.Range(0, firstNamesMale.Length)] : firstNamesFemale[Random.Range(0, firstNamesFemale.Length)];
		string lastName = lastNames[Random.Range(0, lastNames.Length)];

		string playerName = $"{firstName} {lastName}"; // --- Full name of the player ---
		int skillLevel = Random.Range(1, 10); // --- Random skill level ---

		// --- Generate lifetime and current season stats ---
		int lifetimeMatchesPlayed = Random.Range(0, 100);
		int lifetimeMatchesWon = Mathf.Min(lifetimeMatchesPlayed, Random.Range(0, 50));

		Player newPlayer = new(playerName, skillLevel, teamId)
			{
			PointsRequiredToWin = pointsRequiredToWin[skillLevel - 1],
			lifetimeMatchesPlayed = lifetimeMatchesPlayed,
			lifetimeMatchesWon = lifetimeMatchesWon,
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
			currentSeasonPaPercentage = (lifetimeMatchesPlayed > 0 && lifetimeMatchesWon >= 0)
				? (float) lifetimeMatchesWon / lifetimeMatchesPlayed * 100
				: 0, // --- Prevent division by zero ---
			currentSeasonPointsAwarded = Random.Range(0, 100),
			currentSeasonPointsPerMatch = Random.Range(0f, 10f),
			currentSeasonShutouts = Random.Range(0, 5),
			currentSeasonSkillLevel = skillLevel,
			currentSeasonTotalPoints = Random.Range(0, 200)
			};

		Debug.Log($"Generated Player: {newPlayer.name} (Skill Level: {newPlayer.skillLevel}) for Team ID: {teamId}");

		return newPlayer;
		}

	// --- End Region --- //

	// --- Region: Save to JSON --- //
	private void SaveToJson(Team team, List<string> playerNames, List<int> playerSkillLevels)
		{
		// Create a team data structure with player info
		var teamData = new
			{
			TeamName = team.name,
			PlayerNames = playerNames,
			PlayerSkillLevels = playerSkillLevels
			};

		string json = JsonUtility.ToJson(teamData); // --- Serialize to JSON ---

		// Save the generated data to a JSON file (example using file system)
		string path = $"Assets/Resources/Team_{team.id}_Data.json";
		System.IO.File.WriteAllText(path, json); // --- Save to file ---

		Debug.Log("Data saved to JSON.");
		}

	// --- End Region --- //

	// --- Region: Additional Functions --- //
	// Additional utility functions can be added here if needed
	// --- End Region --- //
	}

// --- End Region: Sample Data Generator Class Definition --- //