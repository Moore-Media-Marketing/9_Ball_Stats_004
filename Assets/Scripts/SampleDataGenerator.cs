using TMPro;

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
			Team newTeam = new Team()
				{
				Id = teamId++, // Increment team ID starting at 1
				Name = teamNames[Random.Range(0, teamNames.Length)], // Random team name from the array
				};

			DatabaseManager.Instance.AddTeam(newTeam);

			int numberOfPlayers = Random.Range(5, 9); // Random number of players between 5 and 8
			for (int j = 0; j < numberOfPlayers; j++)
				{
				Player newPlayer = GeneratePlayer(newTeam.Id); // Use GeneratePlayer to create player details
				DatabaseManager.Instance.AddPlayer(newPlayer);
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
		int currentSeasonMatchesPlayed = Random.Range(5, 30);
		int currentSeasonMatchesWon = Mathf.Clamp((int) (currentSeasonMatchesPlayed * skillLevel * 0.1f), 0, currentSeasonMatchesPlayed);
		int currentSeasonBreakAndRun = Mathf.Clamp((int) (currentSeasonMatchesWon * (skillLevel * 0.1f)), 0, currentSeasonMatchesPlayed);

		// --- Keep these as float values --- //
		float currentSeasonDefensiveShotAverage = Mathf.Clamp(skillLevel * 0.1f + Random.Range(0.0f, 0.5f), 0.0f, 1.0f);
		int currentSeasonMiniSlams = Mathf.Clamp((int) (currentSeasonMatchesWon * (skillLevel * 0.05f)), 0, currentSeasonMatchesPlayed);
		int currentSeasonNineOnTheSnap = Mathf.Clamp((int) (currentSeasonMatchesWon * (skillLevel * 0.03f)), 0, currentSeasonMatchesPlayed);

		// --- Keep these as float values --- //
		float currentSeasonPaPercentage = Mathf.Clamp(skillLevel * 0.1f + Random.Range(0.0f, 0.5f), 0.0f, 1.0f);
		int currentSeasonPointsAwarded = Mathf.Clamp(currentSeasonMatchesPlayed * skillLevel, 0, currentSeasonMatchesPlayed * 10);
		int currentSeasonPointsPerMatch = Mathf.Clamp(currentSeasonPointsAwarded / currentSeasonMatchesPlayed, 0, 10);
		int currentSeasonPpm = Mathf.Clamp((int) (currentSeasonPointsPerMatch), 0, 10);
		int currentSeasonShutouts = Mathf.Clamp((int) (currentSeasonMatchesWon * (skillLevel * 0.02f)), 0, currentSeasonMatchesPlayed);
		int currentSeasonTotalPoints = Mathf.Clamp(currentSeasonPointsAwarded, 0, currentSeasonMatchesPlayed * 10);

		// --- Lifetime stats based on skill level --- //
		int lifetimeMatchesPlayed = Random.Range(10, 100);
		int lifetimeMatchesWon = Mathf.Clamp((int) (lifetimeMatchesPlayed * skillLevel * 0.1f), 0, lifetimeMatchesPlayed);
		int lifetimeBreakAndRun = Mathf.Clamp((int) (lifetimeMatchesWon * (skillLevel * 0.05f)), 0, lifetimeMatchesPlayed);

		// --- Keep these as float values --- //
		float lifetimeDefensiveShotAverage = Mathf.Clamp(skillLevel * 0.1f + Random.Range(0.0f, 0.5f), 0.0f, 1.0f);
		int lifetimeMiniSlams = Mathf.Clamp((int) (lifetimeMatchesWon * (skillLevel * 0.05f)), 0, lifetimeMatchesPlayed);
		int lifetimeNineOnTheSnap = Mathf.Clamp((int) (lifetimeMatchesWon * (skillLevel * 0.03f)), 0, lifetimeMatchesPlayed);
		int lifetimeShutouts = Mathf.Clamp((int) (lifetimeMatchesWon * (skillLevel * 0.02f)), 0, lifetimeMatchesPlayed);

		// --- Return new player with all the stats --- //
		return new Player(playerName, skillLevel, teamId)
			{
			CurrentSeasonMatchesPlayed = currentSeasonMatchesPlayed,
			CurrentSeasonMatchesWon = currentSeasonMatchesWon,
			CurrentSeasonBreakAndRun = currentSeasonBreakAndRun,
			CurrentSeasonDefensiveShotAverage = currentSeasonDefensiveShotAverage,
			CurrentSeasonMiniSlams = currentSeasonMiniSlams,
			CurrentSeasonNineOnTheSnap = currentSeasonNineOnTheSnap,
			CurrentSeasonPaPercentage = currentSeasonPaPercentage,
			CurrentSeasonPointsAwarded = currentSeasonPointsAwarded,
			CurrentSeasonPointsPerMatch = currentSeasonPointsPerMatch,
			CurrentSeasonPpm = currentSeasonPpm,
			CurrentSeasonShutouts = currentSeasonShutouts,
			CurrentSeasonSkillLevel = skillLevel,
			CurrentSeasonTotalPoints = currentSeasonTotalPoints,
			LifetimeMatchesPlayed = lifetimeMatchesPlayed,
			LifetimeMatchesWon = lifetimeMatchesWon,
			LifetimeBreakAndRun = lifetimeBreakAndRun,
			LifetimeDefensiveShotAverage = lifetimeDefensiveShotAverage,
			LifetimeMiniSlams = lifetimeMiniSlams,
			LifetimeNineOnTheSnap = lifetimeNineOnTheSnap,
			LifetimeShutouts = lifetimeShutouts
			};
		}
	}
