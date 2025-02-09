using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class SampleDataGenerator:MonoBehaviour
	{
	// Singleton instance
	public static SampleDataGenerator Instance { get; private set; }

	public int numberOfTeams = 10; // Default number of teams
	public float malePercentage = 0.65f; // 65% probability for male players

	// --- Name Pools ---  
	private static readonly string[] firstNamesMale = { "James", "John", "Robert", "Michael", "William", "David", "Richard", "Charles", "Joseph", "Thomas" };
	private static readonly string[] firstNamesFemale = { "Mary", "Patricia", "Linda", "Barbara", "Elizabeth", "Jennifer", "Maria", "Susan", "Margaret", "Dorothy" };
	private static readonly string[] lastNames = { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez" };
	private static readonly string[] teamNames = { "Atomic Squirrels", "Crimson Cobras", "Electric Eels", "Phantom Phantoms", "Rainbow Raptors" };

	private void Awake()
		{
		// Ensure only one instance exists
		if (Instance == null) Instance = this;
		else Destroy(gameObject);
		}

	private void Start()
		{
		GenerateSampleTeamsAndPlayers();
		}

	// Generate sample teams and players
	public void GenerateSampleTeamsAndPlayers()
		{
		int generatedTeams = Random.Range(3, 6); // Generate 3 to 5 teams
		List<Team> existingTeams = DatabaseManager.Instance.GetAllTeams();

		for (int i = 0; i < generatedTeams; i++)
			{
			string teamName = teamNames[Random.Range(0, teamNames.Length)];

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

			// Generate players for the team
			int numberOfPlayers = Random.Range(5, 9);
			for (int j = 0; j < numberOfPlayers; j++)
				{
				Player newPlayer = GeneratePlayer(newTeam.TeamId);
				DatabaseManager.Instance.AddPlayer(newPlayer.PlayerName, newPlayer.PlayerId, newPlayer.TeamId, newPlayer.Stats);
				}
			}

		Debug.Log($"Generated {generatedTeams} teams with players.");
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
			CurrentSeasonMatchesPlayed = Random.Range(5, 30),
			CurrentSeasonMatchesWon = Mathf.Clamp((int) (Random.Range(5, 30) * skillLevel * 0.1f), 0, 30),
			CurrentSeasonBreakAndRun = Mathf.Clamp((int) (Random.Range(1, 10) * (skillLevel * 0.1f)), 0, 10),
			CurrentSeasonDefensiveShotAverage = Mathf.Clamp(skillLevel * 0.1f + Random.Range(0.0f, 0.5f), 0.0f, 1.0f),
			CurrentSeasonMiniSlams = Mathf.Clamp((int) (Random.Range(1, 10) * (skillLevel * 0.05f)), 0, 10),
			CurrentSeasonNineOnTheSnap = Mathf.Clamp((int) (Random.Range(1, 10) * (skillLevel * 0.03f)), 0, 10),
			CurrentSeasonPaPercentage = Mathf.Clamp(skillLevel * 0.1f + Random.Range(0.0f, 0.5f), 0.0f, 1.0f),
			CurrentSeasonPointsAwarded = Mathf.Clamp(Random.Range(10, 300), 0, 300),
			CurrentSeasonPointsPerMatch = Mathf.Clamp(Random.Range(1, 10), 0, 10),
			CurrentSeasonPpm = Mathf.Clamp(Random.Range(1, 10), 0, 10),
			CurrentSeasonShutouts = Mathf.Clamp((int) (Random.Range(1, 10) * (skillLevel * 0.02f)), 0, 10),
			CurrentSeasonSkillLevel = skillLevel,
			CurrentSeasonTotalPoints = Mathf.Clamp(Random.Range(10, 300), 0, 300),

			LifetimeMatchesPlayed = Random.Range(10, 100),
			LifetimeMatchesWon = Mathf.Clamp((int) (Random.Range(10, 100) * skillLevel * 0.1f), 0, 100),
			LifetimeBreakAndRun = Mathf.Clamp((int) (Random.Range(1, 10) * (skillLevel * 0.05f)), 0, 10),
			LifetimeDefensiveShotAverage = Mathf.Clamp(skillLevel * 0.1f + Random.Range(0.0f, 0.5f), 0.0f, 1.0f),
			LifetimeMiniSlams = Mathf.Clamp((int) (Random.Range(1, 10) * (skillLevel * 0.05f)), 0, 10),
			LifetimeNineOnTheSnap = Mathf.Clamp((int) (Random.Range(1, 10) * (skillLevel * 0.03f)), 0, 10),
			LifetimeShutouts = Mathf.Clamp((int) (Random.Range(1, 10) * (skillLevel * 0.02f)), 0, 10),
			};

		return stats;
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
