using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class SampleDataGenerator:MonoBehaviour
	{
	// --- Region: Public Variables --- //
	public int numberOfTeams = 10; // The number of teams to generate
								   // --- End Region: Public Variables --- //

	// --- Region: Sample Data --- //
	private static readonly string[] firstNamesMale = { "James", "John", "Robert", "Michael", "William", "David", "Richard", "Charles", "Joseph", "Thomas", "Christopher", "Daniel", "Paul", "Mark", "Donald", "George", "Kenneth", "Steven", "Edward", "Brian", "Anthony", "Kevin", "Jason", "Jeff", "Ryan" };
	private static readonly string[] firstNamesFemale = { "Mary", "Patricia", "Linda", "Barbara", "Elizabeth", "Jennifer", "Maria", "Susan", "Margaret", "Dorothy", "Lisa", "Nancy", "Karen", "Betty", "Helen", "Sandra", "Donna", "Carol", "Ruth", "Sharon", "Michelle", "Laura", "Sarah", "Kimberly", "Jessica" };
	private static readonly string[] lastNames = { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez", "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", "Thomas", "Taylor", "Moore", "Jackson", "Martin", "Lee", "Perez", "Thompson", "White", "Harris", "Sanchez", "Clark", "Lewis", "Robinson", "Walker", "Young", "Allen", "King", "Wright", "Scott", "Green", "Adams", "Baker", "Hall", "Nelson", "Carter", "Mitchell", "Parker", "Evans", "Edwards", "Collins", "Stewart", "Morris", "Morgan" };
	private static readonly string[] teamNames = { "Atomic Squirrels", "Crimson Cobras", "Electric Eels", "Phantom Phantoms", "Rainbow Raptors", "Mystic Moose", "Silent Serpents", "Cosmic Crusaders", "Neon Ninjas", "Lunar Lions", "Galactic Gorillas", "Iron Eagles", "Shadow Strikers", "Velocity Vipers", "Quicksilver Quails", "Turbo Turtles", "Diamond Dragons", "Emerald Emus", "Golden Geckos", "Icy Iguanas", "Jade Jaguars", "Kinetic Kangaroos", "Laser Lemurs", "Magma Monkeys", "Nova Narwhals", "Onyx Owls", "Plasma Pandas", "Quantum Quetzals", "Ruby Rhinos", "Sapphire Sharks", "Titanium Tigers", "Uranium Unicorns", "Violet Vultures", "Wild Wolverines", "X-Ray Xenopus", "Yellow Yetis", "Zebra Zephyrs" };
	// --- End Region: Sample Data --- //

	// --- Region: Handicap Points for Each Skill Level --- //
	private readonly int[] pointsRequiredToWin = { 14, 19, 25, 31, 38, 46, 55, 65, 75 }; // Points required based on skill level (1 to 9)
																						 // --- End Region: Handicap Points for Each Skill Level --- //

	// --- Region: CSV File Path --- //
	private string filePath;
	// --- End Region: CSV File Path --- //

	// --- Region: Start Method --- //
	private void Start()
		{
		filePath = Path.Combine(Application.persistentDataPath, "sampleData.csv");

		GenerateSampleTeamsAndPlayers();
		}
	// --- End Region: Start Method --- //

	// --- Region: Generate Sample Teams and Players --- //
	private void GenerateSampleTeamsAndPlayers()
		{
		using (StreamWriter writer = new(filePath, false))
			{
			writer.WriteLine("TeamName,FirstName,LastName,SkillLevel,CurrentSeasonBreakAndRun,CurrentSeasonDefensiveShotAverage,CurrentSeasonMatchesPlayed,CurrentSeasonMatchesWon,CurrentSeasonMiniSlams,CurrentSeasonNineOnTheSnap,CurrentSeasonPaPercentage,CurrentSeasonPointsAwarded,CurrentSeasonPointsPerMatch,CurrentSeasonShutouts,CurrentSeasonSkillLevel,CurrentSeasonTotalPoints,LifetimeBreakAndRun,LifetimeDefensiveShotAverage,LifetimeGamesPlayed,LifetimeGamesWon,LifetimeMatchesPlayed,LifetimeMatchesWon,LifetimeMiniSlams,LifetimeNineOnTheSnap,LifetimeShutouts");

			for (int i = 0; i < numberOfTeams; i++)
				{
				Team team = GenerateTeam();  // Generate a new team

				for (int j = 0; j < 8; j++)  // Generate 8 players for each team
					{
					Player newPlayer = GeneratePlayer(team.Name);
					writer.WriteLine(newPlayer.ToCSV());
					}
				}
			}

		Debug.Log("Sample teams and players generated and saved to CSV.");
		}
	// --- End Region: Generate Sample Teams and Players --- //

	// --- Region: Generate Team Method --- //
	private Team GenerateTeam()
		{
		string teamName = teamNames[Random.Range(0, teamNames.Length)];

		// Generate a new Team and return it
		Team newTeam = new()
			{
			Name = teamName
			};

		return newTeam;
		}
	// --- End Region: Generate Team Method --- //

	// --- Region: Generate Player Method --- //
	private Player GeneratePlayer(string teamName)
		{
		string firstName = Random.Range(0, 2) == 0 ? firstNamesMale[Random.Range(0, firstNamesMale.Length)] : firstNamesFemale[Random.Range(0, firstNamesFemale.Length)];
		string lastName = lastNames[Random.Range(0, lastNames.Length)];
		int skillLevel = Random.Range(1, 10); // Skill level between 1 and 9

		// Current Season Stats (calculated based on skill level)
		int currentSeasonBreakAndRun = CalculateBreakAndRun(skillLevel);
		float currentSeasonDefensiveShotAverage = CalculateDefensiveShotAverage(skillLevel);
		int currentSeasonMatchesPlayed = Random.Range(10, 50);
		int currentSeasonMatchesWon = Random.Range(5, currentSeasonMatchesPlayed);
		int currentSeasonMiniSlams = CalculateMiniSlams(skillLevel);
		int currentSeasonNineOnTheSnap = Random.Range(0, skillLevel); // Higher skill = more 9-balls
		float currentSeasonPaPercentage = CalculatePaPercentage(skillLevel);
		int currentSeasonPointsAwarded = CalculatePointsAwarded(skillLevel);
		float currentSeasonPointsPerMatch = Random.Range(5f, 20f);
		int currentSeasonShutouts = Random.Range(0, 3);
		int currentSeasonSkillLevel = skillLevel;
		int currentSeasonTotalPoints = Random.Range(1000, 5000);

		// Lifetime Stats (calculated based on skill level)
		int lifetimeBreakAndRun = CalculateLifetimeBreakAndRun(skillLevel);
		float lifetimeDefensiveShotAverage = CalculateLifetimeDefensiveShotAverage(skillLevel);
		int lifetimeGamesPlayed = Random.Range(50, 200);
		int lifetimeGamesWon = Random.Range(25, lifetimeGamesPlayed);
		int lifetimeMatchesPlayed = Random.Range(50, 200);
		int lifetimeMatchesWon = Random.Range(25, lifetimeMatchesPlayed);
		int lifetimeMiniSlams = CalculateLifetimeMiniSlams(skillLevel);
		int lifetimeNineOnTheSnap = Random.Range(0, skillLevel);
		int lifetimeShutouts = Random.Range(0, 5);

		Player newPlayer = new()
			{
			TeamName = teamName,
			FirstName = firstName,
			LastName = lastName,
			SkillLevel = skillLevel,

			// Current Season Stats
			CurrentSeasonBreakAndRun = currentSeasonBreakAndRun,
			CurrentSeasonDefensiveShotAverage = currentSeasonDefensiveShotAverage,
			CurrentSeasonMatchesPlayed = currentSeasonMatchesPlayed,
			CurrentSeasonMatchesWon = currentSeasonMatchesWon,
			CurrentSeasonMiniSlams = currentSeasonMiniSlams,
			CurrentSeasonNineOnTheSnap = currentSeasonNineOnTheSnap,
			CurrentSeasonPaPercentage = currentSeasonPaPercentage,
			CurrentSeasonPointsAwarded = currentSeasonPointsAwarded,
			CurrentSeasonPointsPerMatch = currentSeasonPointsPerMatch,
			CurrentSeasonShutouts = currentSeasonShutouts,
			CurrentSeasonSkillLevel = currentSeasonSkillLevel,
			CurrentSeasonTotalPoints = currentSeasonTotalPoints,

			// Lifetime Stats
			LifetimeBreakAndRun = lifetimeBreakAndRun,
			LifetimeDefensiveShotAverage = lifetimeDefensiveShotAverage,
			LifetimeGamesPlayed = lifetimeGamesPlayed,
			LifetimeGamesWon = lifetimeGamesWon,
			LifetimeMatchesPlayed = lifetimeMatchesPlayed,
			LifetimeMatchesWon = lifetimeMatchesWon,
			LifetimeMiniSlams = lifetimeMiniSlams,
			LifetimeNineOnTheSnap = lifetimeNineOnTheSnap,
			LifetimeShutouts = lifetimeShutouts
			};

		return newPlayer;
		}
	// --- End Region: Generate Player Method --- //

	// --- Region: Helper Methods for Stats Calculation Based on Skill Level --- //
	private int CalculateBreakAndRun(int skillLevel)
		{
		return Mathf.Clamp(skillLevel, 0, skillLevel - 1);
		}

	private float CalculateDefensiveShotAverage(int skillLevel)
		{
		return Mathf.Clamp(0.5f + (skillLevel * 0.05f), 0.5f, 0.95f);
		}

	private int CalculateMiniSlams(int skillLevel)
		{
		return Mathf.Clamp(skillLevel - 1, 0, 3);
		}

	private float CalculatePaPercentage(int skillLevel)
		{
		return Mathf.Clamp(0.6f + (skillLevel * 0.05f), 0.6f, 0.95f);
		}

	private int CalculatePointsAwarded(int skillLevel)
		{
		return Mathf.Clamp(skillLevel * 100, 100, 900);
		}

	private int CalculateLifetimeBreakAndRun(int skillLevel)
		{
		return Mathf.Clamp(skillLevel * 2, 0, skillLevel * 2);
		}

	private float CalculateLifetimeDefensiveShotAverage(int skillLevel)
		{
		return Mathf.Clamp(0.5f + (skillLevel * 0.04f), 0.5f, 0.9f);
		}

	private int CalculateLifetimeMiniSlams(int skillLevel)
		{
		return Mathf.Clamp(skillLevel - 1, 0, 5);
		}
	// --- End Region: Helper Methods for Stats Calculation --- //

	// --- Region: Player Class --- //
	public class Player
		{
		public string TeamName { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public int SkillLevel { get; set; }

		// Current Season Stats
		public int CurrentSeasonBreakAndRun { get; set; }
		public float CurrentSeasonDefensiveShotAverage { get; set; }
		public int CurrentSeasonMatchesPlayed { get; set; }
		public int CurrentSeasonMatchesWon { get; set; }
		public int CurrentSeasonMiniSlams { get; set; }
		public int CurrentSeasonNineOnTheSnap { get; set; }
		public float CurrentSeasonPaPercentage { get; set; }
		public int CurrentSeasonPointsAwarded { get; set; }
		public float CurrentSeasonPointsPerMatch { get; set; }
		public int CurrentSeasonShutouts { get; set; }
		public int CurrentSeasonSkillLevel { get; set; }
		public int CurrentSeasonTotalPoints { get; set; }

		// Lifetime Stats
		public int LifetimeBreakAndRun { get; set; }
		public float LifetimeDefensiveShotAverage { get; set; }
		public int LifetimeGamesPlayed { get; set; }
		public int LifetimeGamesWon { get; set; }
		public int LifetimeMatchesPlayed { get; set; }
		public int LifetimeMatchesWon { get; set; }
		public int LifetimeMiniSlams { get; set; }
		public int LifetimeNineOnTheSnap { get; set; }
		public int LifetimeShutouts { get; set; }

		// Convert Player data to CSV string
		public string ToCSV()
			{
			return $"{TeamName},{FirstName},{LastName},{SkillLevel},{CurrentSeasonBreakAndRun},{CurrentSeasonDefensiveShotAverage},{CurrentSeasonMatchesPlayed},{CurrentSeasonMatchesWon},{CurrentSeasonMiniSlams},{CurrentSeasonNineOnTheSnap},{CurrentSeasonPaPercentage},{CurrentSeasonPointsAwarded},{CurrentSeasonPointsPerMatch},{CurrentSeasonShutouts},{CurrentSeasonSkillLevel},{CurrentSeasonTotalPoints},{LifetimeBreakAndRun},{LifetimeDefensiveShotAverage},{LifetimeGamesPlayed},{LifetimeGamesWon},{LifetimeMatchesPlayed},{LifetimeMatchesWon},{LifetimeMiniSlams},{LifetimeNineOnTheSnap},{LifetimeShutouts}";
			}
		}
	// --- End Region: Player Class --- //

	// --- Region: Team Class --- //
	public class Team
		{
		public string Name { get; set; }
		}
	// --- End Region: Team Class --- //
	}
