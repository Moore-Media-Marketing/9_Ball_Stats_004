using UnityEngine;

public class SampleDataGenerator:MonoBehaviour
	{
	public int numberOfTeams = 10; // --- Number of teams to generate --- //
	public int numberOfPlayersPerTeam = Random.Range(5, 8); // --- Number of players per team --- //

	private static readonly string[] firstNamesMale = { "James", "John", "Robert", "Michael", "William", "David", "Richard", "Charles", "Joseph", "Thomas", "Christopher", "Daniel", "Paul", "Mark", "Donald", "George", "Kenneth", "Steven", "Edward", "Brian", "Anthony", "Kevin", "Jason", "Jeff", "Ryan" };
	private static readonly string[] firstNamesFemale = { "Mary", "Patricia", "Linda", "Barbara", "Elizabeth", "Jennifer", "Maria", "Susan", "Margaret", "Dorothy", "Lisa", "Nancy", "Karen", "Betty", "Helen", "Sandra", "Donna", "Carol", "Ruth", "Sharon", "Michelle", "Laura", "Sarah", "Kimberly", "Jessica" };
	private static readonly string[] lastNames = { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez", "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", "Thomas", "Taylor", "Moore", "Jackson", "Martin", "Lee", "Perez", "Thompson", "White", "Harris", "Sanchez", "Clark", "Lewis", "Robinson", "Walker", "Young", "Allen", "King", "Wright", "Scott", "Green", "Adams", "Baker", "Hall", "Nelson", "Carter", "Mitchell", "Parker", "Evans", "Edwards", "Collins", "Stewart", "Morris", "Morgan" };
	private static readonly string[] teamNames = { "Atomic Squirrels", "Crimson Cobras", "Electric Eels", "Phantom Phantoms", "Rainbow Raptors", "Mystic Moose", "Silent Serpents", "Cosmic Crusaders", "Neon Ninjas", "Lunar Lions", "Galactic Gorillas", "Iron Eagles", "Shadow Strikers", "Velocity Vipers", "Quicksilver Quails", "Turbo Turtles", "Diamond Dragons", "Emerald Emus", "Golden Geckos", "Icy Iguanas", "Jade Jaguars", "Kinetic Kangaroos", "Laser Lemurs", "Magma Monkeys", "Nova Narwhals", "Onyx Owls", "Plasma Pandas", "Quantum Quetzals", "Ruby Rhinos", "Sapphire Sharks", "Titanium Tigers", "Uranium Unicorns", "Violet Vultures", "Wild Wolverines", "X-Ray Xenopus", "Yellow Yetis", "Zebra Zephyrs" };

	private void Start()
		{
		GenerateSampleTeamsAndPlayers();
		}

	private void GenerateSampleTeamsAndPlayers()
		{
		for (int i = 0; i < numberOfTeams; i++)
			{
			Team newTeam = GenerateTeam();
			DatabaseManager.Instance.AddTeam(newTeam);

			for (int j = 0; j < numberOfPlayersPerTeam; j++)
				{
				Player newPlayer = GeneratePlayer(newTeam.Id);
				DatabaseManager.Instance.AddPlayer(newPlayer);
				}
			}
		}

	private Team GenerateTeam()
		{
		string teamName = teamNames[Random.Range(0, teamNames.Length)];

		// --- Create a Team with the name --- //
		return new Team(teamName);
		}

	private Player GeneratePlayer(int teamId)
		{
		// --- Here we have a 65% chance of generating a male name //

		string firstName = firstNamesMale[Random.Range(0, firstNamesMale.Length)];

		// --- Here we have a 35% chance of generating a female name //

		string firstName = firstNamesFemale[Random.Range(0, firstNamesFemale.Length)];

		// --- Here we decided to generate a random last name for the player. //

		string lastName = lastNames[Random.Range(0, lastNames.Length)];
		string playerName = $"{firstName} {lastName}";
		int skillLevel = Random.Range(1, 9); // --- Must be a whole number between 1 and 9 --- //

		// --- Generating player stats based on skill level --- //
		int currentSeasonMatchesPlayed = Random.Range(5, 30);  // --- Must be a whole number --- //
		int currentSeasonMatchesWon = Mathf.Clamp((int) (currentSeasonMatchesPlayed * skillLevel * 0.1f), 0, currentSeasonMatchesPlayed);  // --- Must be a whole number --- //
		int currentSeasonBreakAndRun = Mathf.Clamp((int) (currentSeasonMatchesWon * (skillLevel * 0.1f)), 0, currentSeasonMatchesPlayed);  // --- Must be a whole number --- //

		// --- Keep these as float values --- //
		float currentSeasonDefensiveShotAverage = Mathf.Clamp(skillLevel * 0.1f + Random.Range(0.0f, 0.5f), 0.0f, 1.0f); // --- Float value --- //
		int currentSeasonMiniSlams = Mathf.Clamp((int) (currentSeasonMatchesWon * (skillLevel * 0.05f)), 0, currentSeasonMatchesPlayed);  // --- Must be a whole number --- //
		int currentSeasonNineOnTheSnap = Mathf.Clamp((int) (currentSeasonMatchesWon * (skillLevel * 0.03f)), 0, currentSeasonMatchesPlayed);  // --- Must be a whole number --- //

		// --- Keep these as float values --- //
		float currentSeasonPaPercentage = Mathf.Clamp(skillLevel * 0.1f + Random.Range(0.0f, 0.5f), 0.0f, 1.0f); // Float value
		int currentSeasonPointsAwarded = Mathf.Clamp(currentSeasonMatchesPlayed * skillLevel, 0, currentSeasonMatchesPlayed * 10);  // --- Must be a whole number --- //
		int currentSeasonPointsPerMatch = Mathf.Clamp(currentSeasonPointsAwarded / currentSeasonMatchesPlayed, 0, 10);  // --- Must be a whole number --- //
		int currentSeasonPpm = Mathf.Clamp((int) (currentSeasonPointsPerMatch), 0, 10);  // --- Must be a whole number --- //
		int currentSeasonShutouts = Mathf.Clamp((int) (currentSeasonMatchesWon * (skillLevel * 0.02f)), 0, currentSeasonMatchesPlayed);   // --- Must be a whole number --- //
		int currentSeasonTotalPoints = Mathf.Clamp(currentSeasonPointsAwarded, 0, currentSeasonMatchesPlayed * 10);  // --- Must be a whole number --- //

		// --- Lifetime stats based on skill level --- //
		int lifetimeMatchesPlayed = Random.Range(10, 100);  // --- Must be a whole number --- //
		int lifetimeMatchesWon = Mathf.Clamp((int) (lifetimeMatchesPlayed * skillLevel * 0.1f), 0, lifetimeMatchesPlayed);  // --- Must be a whole number --- //
		int lifetimeBreakAndRun = Mathf.Clamp((int) (lifetimeMatchesWon * (skillLevel * 0.05f)), 0, lifetimeMatchesPlayed);  // --- Must be a whole number --- //

		// --- Keep these as float values --- //
		float lifetimeDefensiveShotAverage = Mathf.Clamp(skillLevel * 0.1f + Random.Range(0.0f, 0.5f), 0.0f, 1.0f); // --- Float value --- //
		int lifetimeMiniSlams = Mathf.Clamp((int) (lifetimeMatchesWon * (skillLevel * 0.05f)), 0, lifetimeMatchesPlayed);  // --- Must be a whole number --- //
		int lifetimeNineOnTheSnap = Mathf.Clamp((int) (lifetimeMatchesWon * (skillLevel * 0.03f)), 0, lifetimeMatchesPlayed);  // --- Must be a whole number --- //
		int lifetimeShutouts = Mathf.Clamp((int) (lifetimeMatchesWon * (skillLevel * 0.02f)), 0, lifetimeMatchesPlayed);  // --- Must be a whole number --- //

		// --- Return new player with all the stats --- //
		return new Player(playerName, skillLevel, teamId)
			{
			CurrentSeasonMatchesPlayed = currentSeasonMatchesPlayed,  // --- Must be a whole number --- //
			CurrentSeasonMatchesWon = currentSeasonMatchesWon,  // --- Must be a whole number --- //
			CurrentSeasonBreakAndRun = currentSeasonBreakAndRun,  // --- Must be a whole number --- //
			CurrentSeasonDefensiveShotAverage = currentSeasonDefensiveShotAverage, // Float value --- //
			CurrentSeasonMiniSlams = currentSeasonMiniSlams,  // --- Must be a whole number --- //
			CurrentSeasonNineOnTheSnap = currentSeasonNineOnTheSnap,  // --- Must be a whole number --- //
			CurrentSeasonPaPercentage = currentSeasonPaPercentage, // --- Float value --- //
			CurrentSeasonPointsAwarded = currentSeasonPointsAwarded,  // --- Must be a whole number --- //
			CurrentSeasonPointsPerMatch = currentSeasonPointsPerMatch,  // --- Must be a whole number --- //
			CurrentSeasonPpm = currentSeasonPpm,  // --- Must be a whole number --- //
			CurrentSeasonShutouts = currentSeasonShutouts,  // --- Must be a whole number --- //

			// --- Here we decided to generate a random skill level for the player. Cannot implicitly convert type 'int' to 'string' //
			CurrentSeasonSkillLevel = skillLevel,  // --- Must be a whole number --- //


			CurrentSeasonTotalPoints = currentSeasonTotalPoints,  // --- Must be a whole number --- //
			LifetimeMatchesPlayed = lifetimeMatchesPlayed,  // --- Must be a whole number --- //
			LifetimeMatchesWon = lifetimeMatchesWon,  // --- Must be a whole number --- //
			LifetimeBreakAndRun = lifetimeBreakAndRun,  // --- Must be a whole number --- //
			LifetimeDefensiveShotAverage = lifetimeDefensiveShotAverage, // --- Float value --- //
			LifetimeMiniSlams = lifetimeMiniSlams,  // --- Must be a whole number --- //
			LifetimeNineOnTheSnap = lifetimeNineOnTheSnap,  // --- Must be a whole number --- //
			LifetimeShutouts = lifetimeShutouts  // --- Must be a whole number --- //
			};
		}
	}