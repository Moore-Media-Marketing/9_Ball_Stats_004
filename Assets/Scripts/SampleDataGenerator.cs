using System.Linq;

using UnityEngine;

public class SampleDataGenerator:MonoBehaviour
	{
	public int numberOfTeams = 10; // --- Number of teams to generate --- //
	public int numberOfPlayersPerTeam; // --- Number of players per team --- //
	public float malePercentage = 0.65f; // --- Percentage of male players (65%) --- //

	// --- Name arrays --- 
	private static readonly string[] firstNamesMale = { "James", "John", "Robert", "Michael", "William", "David", "Richard", "Charles", "Joseph", "Thomas", "Christopher", "Daniel", "Paul", "Mark", "Donald", "George", "Kenneth", "Steven", "Edward", "Brian", "Anthony", "Kevin", "Jason", "Jeff", "Ryan" };

	private static readonly string[] firstNamesFemale = { "Mary", "Patricia", "Linda", "Barbara", "Elizabeth", "Jennifer", "Maria", "Susan", "Margaret", "Dorothy", "Lisa", "Nancy", "Karen", "Betty", "Helen", "Sandra", "Donna", "Carol", "Ruth", "Sharon", "Michelle", "Laura", "Sarah", "Kimberly", "Jessica" };

	private static readonly string[] lastNames = { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez", "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", "Thomas", "Taylor", "Moore", "Jackson", "Martin", "Lee", "Perez", "Thompson", "White", "Harris", "Sanchez", "Clark", "Lewis", "Robinson", "Walker", "Young", "Allen", "King", "Wright", "Scott", "Green", "Adams", "Baker", "Hall", "Nelson", "Carter", "Mitchell", "Parker", "Evans", "Edwards", "Collins", "Stewart", "Morris", "Morgan" };

	private static readonly string[] teamNames = { "Atomic Squirrels", "Crimson Cobras", "Electric Eels", "Phantom Phantoms", "Rainbow Raptors", "Mystic Moose", "Silent Serpents", "Cosmic Crusaders", "Neon Ninjas", "Lunar Lions", "Galactic Gorillas", "Iron Eagles", "Shadow Strikers", "Velocity Vipers", "Quicksilver Quails", "Turbo Turtles", "Diamond Dragons", "Emerald Emus", "Golden Geckos", "Icy Iguanas", "Jade Jaguars", "Kinetic Kangaroos", "Laser Lemurs", "Magma Monkeys", "Nova Narwhals", "Onyx Owls", "Plasma Pandas", "Quantum Quetzals", "Ruby Rhinos", "Sapphire Sharks", "Titanium Tigers", "Uranium Unicorns", "Violet Vultures", "Wild Wolverines", "X-Ray Xenopus", "Yellow Yetis", "Zebra Zephyrs" };

	private void Start()
		{
		numberOfPlayersPerTeam = Random.Range(5, 9); // Set random number of players per team
		GenerateSampleTeamsAndPlayers();
		}

	public void GenerateSampleTeamsAndPlayers()
		{
		int numberOfTeams = Random.Range(3, 6); // Random number of teams between 3 and 5
		int teamId = 1; // Start at team ID 1

		for (int i = 0; i < numberOfTeams; i++)
			{
			Team newTeam = new Team(teamId++, teamNames[Random.Range(0, teamNames.Length)]); // Random team name from the array

			// --- Updated to call AddTeam with the team name instead of Team object --- //
			DatabaseManager.Instance.AddTeam(newTeam.TeamName); // Assuming AddTeam expects teamName as a string

			int numberOfPlayers = Random.Range(5, 9); // Random number of players between 5 and 8
			for (int j = 0; j < numberOfPlayers; j++)
				{
				Player newPlayer = GeneratePlayer(newTeam.TeamId); // Use GeneratePlayer to create player details
																   // Generate player stats (using SampleDataGenerator) for testing
				DatabaseManager.Instance.AddPlayer(newPlayer.PlayerName, newPlayer.PlayerId, newPlayer.TeamId, this); // Pass 'this' (current SampleDataGenerator instance)
				GeneratePlayerStats(newPlayer.PlayerId, newPlayer.SkillLevel);
				}
			}
		}

	private Player GeneratePlayer(int teamId)
		{
		// --- Determine gender based on percentage --- //
		bool isMale = Random.Range(0f, 1f) <= malePercentage;

		// --- Generate first name based on gender --- //
		string firstName = isMale ? firstNamesMale[Random.Range(0, firstNamesMale.Length)] : firstNamesFemale[Random.Range(0, firstNamesFemale.Length)];

		// --- Generate a random last name for the player --- //
		string lastName = lastNames[Random.Range(0, lastNames.Length)];
		string playerName = $"{firstName} {lastName}";

		// --- Generate skill level as a whole number between 1 and 9 --- //
		int skillLevel = Random.Range(1, 9); // --- Whole number between 1 and 9 --- //

		// --- Generating player stats based on skill level --- //
		PlayerStats stats = GeneratePlayerStats(skillLevel);

		// --- Return new player with stats --- //
		return new Player(Random.Range(1, 1000), playerName, teamId, stats, skillLevel); // Assuming PlayerId is generated randomly
		}

	private PlayerStats GeneratePlayerStats(int skillLevel)
		{
		// --- Placeholder for actual player stats generation logic --- //
		// Stats will be generated in `GeneratePlayerStats` method below
		return new PlayerStats(); // Returning empty stats here, will populate them in the stats generation logic
		}

	public void GeneratePlayerStats(int playerId, int skillLevel)
		{
		// Find the player by their ID in the database
		Player player = DatabaseManager.Instance.players.FirstOrDefault(p => p.PlayerId == playerId);
		if (player == null)
			{
			Debug.LogWarning($"Player with ID '{playerId}' not found.");
			return;
			}

		// --- Initialize PlayerStats object before use --- //
		PlayerStats stats = new PlayerStats
			{
			// --- Current Season Stats --- //
			CurrentSeasonMatchesPlayed = Random.Range(5, 30)
			};
		stats.CurrentSeasonMatchesWon = Mathf.Clamp((int) (stats.CurrentSeasonMatchesPlayed * skillLevel * 0.1f), 0, stats.CurrentSeasonMatchesPlayed);
		stats.CurrentSeasonBreakAndRun = Mathf.Clamp((int) (stats.CurrentSeasonMatchesWon * (skillLevel * 0.1f)), 0, stats.CurrentSeasonMatchesPlayed);
		stats.CurrentSeasonDefensiveShotAverage = Mathf.Clamp(skillLevel * 0.1f + Random.Range(0.0f, 0.5f), 0.0f, 1.0f);
		stats.CurrentSeasonMiniSlams = Mathf.Clamp((int) (stats.CurrentSeasonMatchesWon * (skillLevel * 0.05f)), 0, stats.CurrentSeasonMatchesPlayed);
		stats.CurrentSeasonNineOnTheSnap = Mathf.Clamp((int) (stats.CurrentSeasonMatchesWon * (skillLevel * 0.03f)), 0, stats.CurrentSeasonMatchesPlayed);
		stats.CurrentSeasonPaPercentage = Mathf.Clamp(skillLevel * 0.1f + Random.Range(0.0f, 0.5f), 0.0f, 1.0f);
		stats.CurrentSeasonPointsAwarded = Mathf.Clamp(stats.CurrentSeasonMatchesPlayed * skillLevel, 0, stats.CurrentSeasonMatchesPlayed * 10);
		stats.CurrentSeasonPointsPerMatch = Mathf.Clamp(stats.CurrentSeasonPointsAwarded / stats.CurrentSeasonMatchesPlayed, 0, 10);
		stats.CurrentSeasonPpm = Mathf.Clamp((int) (stats.CurrentSeasonPointsPerMatch), 0, 10);
		stats.CurrentSeasonShutouts = Mathf.Clamp((int) (stats.CurrentSeasonMatchesWon * (skillLevel * 0.02f)), 0, stats.CurrentSeasonMatchesPlayed);
		stats.CurrentSeasonSkillLevel = skillLevel;
		stats.CurrentSeasonTotalPoints = Mathf.Clamp(stats.CurrentSeasonPointsAwarded, 0, stats.CurrentSeasonMatchesPlayed * 10);

		// --- Lifetime Stats --- //
		stats.LifetimeMatchesPlayed = Random.Range(10, 100);
		stats.LifetimeMatchesWon = Mathf.Clamp((int) (stats.LifetimeMatchesPlayed * skillLevel * 0.1f), 0, stats.LifetimeMatchesPlayed);
		stats.LifetimeBreakAndRun = Mathf.Clamp((int) (stats.LifetimeMatchesWon * (skillLevel * 0.05f)), 0, stats.LifetimeMatchesPlayed);
		stats.LifetimeDefensiveShotAverage = Mathf.Clamp(skillLevel * 0.1f + Random.Range(0.0f, 0.5f), 0.0f, 1.0f);
		stats.LifetimeMiniSlams = Mathf.Clamp((int) (stats.LifetimeMatchesWon * (skillLevel * 0.05f)), 0, stats.LifetimeMatchesPlayed);
		stats.LifetimeNineOnTheSnap = Mathf.Clamp((int) (stats.LifetimeMatchesWon * (skillLevel * 0.03f)), 0, stats.LifetimeMatchesPlayed);
		stats.LifetimeShutouts = Mathf.Clamp((int) (stats.LifetimeMatchesWon * (skillLevel * 0.02f)), 0, stats.LifetimeMatchesPlayed);

		// Now update the player's stats in the database
		DatabaseManager.Instance.UpdatePlayerStats(playerId, stats);
		}
	}
