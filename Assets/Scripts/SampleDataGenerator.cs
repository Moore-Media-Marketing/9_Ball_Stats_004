using System.Collections.Generic;
using System.Linq;

using UnityEngine;

/// <summary>
/// Class for generating sample teams and players.
/// </summary>
public class SampleDataGenerator:MonoBehaviour
	{
	// Singleton instance
	public static SampleDataGenerator Instance { get; private set; }

	public int numberOfTeams = 10; // Fixed to 10 teams
	public float malePercentage = 0.65f; // 65% probability for male players

	// Flag to control sample data generation
	private bool isSampleDataGenerationEnabled = false;

	// --- Name Pools ---
	private static readonly string[] firstNamesMale = { "James", "John", "Robert", "Michael", "William", "David", "Richard", "Charles", "Joseph", "Thomas" };
	private static readonly string[] firstNamesFemale = { "Mary", "Patricia", "Linda", "Barbara", "Elizabeth", "Jennifer", "Maria", "Susan", "Margaret", "Dorothy" };
	private static readonly string[] lastNames = { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez" };

	// List of adjectives and nouns for creating team names
	private static readonly string[] adjectives = { "Atomic", "Crimson", "Electric", "Phantom", "Rainbow", "Flying", "Silent", "Thunder", "Rapid", "Shining" };
	private static readonly string[] nouns = { "Squirrels", "Cobras", "Eels", "Phantoms", "Raptors", "Tigers", "Dragons", "Wolves", "Bears", "Sharks", "Tide", "Wolves", "Eagles", "Knights", "Devils", "Dragons", "Panthers", "Horses", "Hawks", "Sharks", "Cats", "Ducks", "Foxes", "Stalkers", "Lions", "Bolts", "Rays", "Winds", "Storms", "Warriors", "Legends", "Heroes", "Pioneers", "Ships", "Raptors" };

	// --- Predefined Team Names ---
	private static readonly string[] predefinedTeamNames = { "Atomic Squirrels", "Crimson Cobras", "Electric Eels", "Phantom Phantoms", "Rainbow Raptors" };

	private void Awake()
		{
		// Ensure only one instance exists
		if (Instance == null) Instance = this;
		else Destroy(gameObject);
		}

	private void Start()
		{
		if (isSampleDataGenerationEnabled)
			{
			GenerateSampleTeamsAndPlayers();
			}
		}

	// Enable sample data generation
	public void EnableSampleDataGeneration()
		{
		isSampleDataGenerationEnabled = true;
		GenerateSampleTeamsAndPlayers(); // Generate data immediately when enabled
		}

	// Disable sample data generation
	public void DisableSampleDataGeneration()
		{
		isSampleDataGenerationEnabled = false; // Just stop generating data
		Debug.Log("Sample Data Generation Disabled");
		}

	// Generate random team name
	private string GenerateRandomTeamName()
		{
		// Randomly pick an adjective and a noun to create a team name
		string adjective = adjectives[Random.Range(0, adjectives.Length)];
		string noun = nouns[Random.Range(0, nouns.Length)];

		// Combine them to form a team name
		return $"{adjective} {noun}";
		}

	// Generate sample teams and players
	public void GenerateSampleTeamsAndPlayers()
		{
		List<Team> existingTeams = DatabaseManager.Instance.GetAllTeams();

		// Fixed numberOfTeams to 10
		numberOfTeams = 10;

		for (int i = 0; i < numberOfTeams; i++)
			{
			// Use either predefined or generated team names
			string teamName = predefinedTeamNames[Random.Range(0, predefinedTeamNames.Length)];

			// Check if team already exists
			if (existingTeams.Any(t => t.TeamName == teamName))
				{
				Debug.Log($"Skipping duplicate team: {teamName}");
				continue;
				}

			// Add team to database
			DatabaseManager.Instance.AddTeam(teamName);
			Team newTeam = DatabaseManager.Instance.GetAllTeams().FirstOrDefault(t => t.TeamName == teamName);

			if (newTeam == null)
				{
				Debug.LogError($"Failed to add team: {teamName}");
				continue;
				}

			// Ensure 8 players per team
			int numberOfPlayers = 8;

			// Generate players for the team
			for (int j = 0; j < numberOfPlayers; j++)
				{
				Player newPlayer = GeneratePlayer(newTeam.TeamId);
				// Corrected argument order for AddPlayer method
				DatabaseManager.Instance.AddPlayer(newPlayer.PlayerId, newPlayer.PlayerName, newPlayer.TeamId, newPlayer.Stats);
				}
			}

		Debug.Log($"Generated {numberOfTeams} teams with players.");
		}

	// Generate a single player
	private Player GeneratePlayer(int teamId)
		{
		// Determine gender probability
		bool isMale = Random.Range(0f, 1f) <= malePercentage;
		string firstName = isMale ? firstNamesMale[Random.Range(0, firstNamesMale.Length)] : firstNamesFemale[Random.Range(0, firstNamesFemale.Length)];
		string lastName = lastNames[Random.Range(0, lastNames.Length)];
		string playerName = $"{firstName} {lastName}";

		// Generate unique PlayerId
		int playerId = GenerateUniquePlayerId();

		// Random skill level between 1 and 9
		int skillLevel = Random.Range(1, 10);

		// Generate player stats
		PlayerStats stats = GeneratePlayerStats(skillLevel);

		return new Player(playerId, playerName, teamId, stats);
		}

	/// <summary>
	/// Generates the player stats based on the skill level.
	/// </summary>
	public PlayerStats GeneratePlayerStats(int skillLevel)
		{
		PlayerStats stats = new()
			{
			// Current Season Stats
			CurrentSeasonMatchesPlayed = Mathf.RoundToInt(Random.Range(5, 30)),  // Whole number
			CurrentSeasonMatchesWon = Mathf.Clamp((int) (Random.Range(5, 30) * skillLevel * 0.1f), 0, 30),  // Whole number
			CurrentSeasonBreakAndRun = Mathf.Clamp((int) (Random.Range(1, 10) * (skillLevel * 0.1f)), 0, 10),  // Whole number
			CurrentSeasonDefensiveShotAverage = Mathf.Round((skillLevel * 0.1f + Random.Range(0.0f, 0.5f)) * 100f) / 100f, // Rounded to 2 decimals
			CurrentSeasonMiniSlams = Mathf.Clamp((int) (Random.Range(1, 10) * (skillLevel * 0.05f)), 0, 10),  // Whole number
			CurrentSeasonNineOnTheSnap = Mathf.Clamp((int) (Random.Range(1, 10) * (skillLevel * 0.03f)), 0, 10),  // Whole number
			CurrentSeasonPaPercentage = Mathf.Round((skillLevel * 0.1f + Random.Range(0.0f, 0.5f)) * 100f) / 100f,  // Rounded to 2 decimals
			CurrentSeasonPointsAwarded = Mathf.Clamp(Random.Range(10, 300), 0, 300),  // Whole number
			CurrentSeasonPointsPerMatch = Mathf.Clamp(Random.Range(1, 10), 0, 10),  // Whole number
			CurrentSeasonPpm = Mathf.Clamp(Random.Range(1, 10), 0, 10),  // Whole number
			CurrentSeasonShutouts = Mathf.Clamp((int) (Random.Range(1, 10) * (skillLevel * 0.02f)), 0, 10),  // Whole number
			CurrentSeasonSkillLevel = skillLevel,  // Whole number
			CurrentSeasonTotalPoints = Mathf.Clamp(Random.Range(10, 300), 0, 300),  // Whole number

			// Lifetime Stats
			LifetimeMatchesPlayed = Mathf.RoundToInt(Random.Range(10, 100)),  // Whole number
			LifetimeMatchesWon = Mathf.Clamp((int) (Random.Range(10, 100) * skillLevel * 0.1f), 0, 100),  // Whole number
			LifetimeBreakAndRun = Mathf.Clamp((int) (Random.Range(1, 10) * (skillLevel * 0.05f)), 0, 10),  // Whole number
			LifetimeDefensiveShotAverage = Mathf.Round((skillLevel * 0.1f + Random.Range(0.0f, 0.5f)) * 100f) / 100f, // Rounded to 2 decimals
			LifetimeMiniSlams = Mathf.Clamp((int) (Random.Range(1, 10) * (skillLevel * 0.05f)), 0, 10),  // Whole number
			LifetimeNineOnTheSnap = Mathf.Clamp((int) (Random.Range(1, 10) * (skillLevel * 0.03f)), 0, 10),  // Whole number
			LifetimeShutouts = Mathf.Clamp((int) (Random.Range(1, 10) * (skillLevel * 0.02f)), 0, 10),  // Whole number
			};

		return stats;
		}

	// Check if sample data generation is enabled
	public bool IsSampleDataEnabled()
		{
		return isSampleDataGenerationEnabled;
		}

	// Generate a unique player ID
	private int GenerateUniquePlayerId()
		{
		List<int> existingIds = DatabaseManager.Instance.GetAllPlayers().Select(p => p.PlayerId).ToList();
		int newId;

		do
			{
			newId = Random.Range(1, 10000);
			} while (existingIds.Contains(newId));

		return newId;
		}
	}
