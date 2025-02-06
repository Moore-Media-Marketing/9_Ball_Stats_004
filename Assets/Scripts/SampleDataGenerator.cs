using System.IO;

using UnityEngine;

public class SampleDataGenerator:MonoBehaviour
	{
	public int numberOfTeams = 10; // The number of teams to generate
	public int matchesPerSeason = 16; // Matches per season

	// Sample Data Arrays (for names and team names)
	private static readonly string[] firstNamesMale = { "James", "John", "Robert", "Michael", "William", "David", "Richard", "Charles", "Joseph", "Thomas", "Christopher", "Daniel", "Paul", "Mark", "Donald", "George", "Kenneth", "Steven", "Edward", "Brian", "Anthony", "Kevin", "Jason", "Jeff", "Ryan" };
	private static readonly string[] firstNamesFemale = { "Mary", "Patricia", "Linda", "Barbara", "Elizabeth", "Jennifer", "Maria", "Susan", "Margaret", "Dorothy", "Lisa", "Nancy", "Karen", "Betty", "Helen", "Sandra", "Donna", "Carol", "Ruth", "Sharon", "Michelle", "Laura", "Sarah", "Kimberly", "Jessica" };
	private static readonly string[] lastNames = { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez", "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", "Thomas", "Taylor", "Moore", "Jackson", "Martin", "Lee", "Perez", "Thompson", "White", "Harris", "Sanchez", "Clark", "Lewis", "Robinson", "Walker", "Young", "Allen", "King", "Wright", "Scott", "Green", "Adams", "Baker", "Hall", "Nelson", "Carter", "Mitchell", "Parker", "Evans", "Edwards", "Collins", "Stewart", "Morris", "Morgan" };
	private static readonly string[] teamNames = { "Atomic Squirrels", "Crimson Cobras", "Electric Eels", "Phantom Phantoms", "Rainbow Raptors", "Mystic Moose", "Silent Serpents", "Cosmic Crusaders", "Neon Ninjas", "Lunar Lions", "Galactic Gorillas", "Iron Eagles", "Shadow Strikers", "Velocity Vipers", "Quicksilver Quails", "Turbo Turtles", "Diamond Dragons", "Emerald Emus", "Golden Geckos", "Icy Iguanas", "Jade Jaguars", "Kinetic Kangaroos", "Laser Lemurs", "Magma Monkeys", "Nova Narwhals", "Onyx Owls", "Plasma Pandas", "Quantum Quetzals", "Ruby Rhinos", "Sapphire Sharks", "Titanium Tigers", "Uranium Unicorns", "Violet Vultures", "Wild Wolverines", "X-Ray Xenopus", "Yellow Yetis", "Zebra Zephyrs" };

	private string playersFilePath;
	private string teamsFilePath;

	// Points required for each skill level to win
	private static readonly int[] pointsRequiredToWin = { 14, 19, 25, 31, 38, 46, 55, 65, 75 };

	private void Start()
		{
		playersFilePath = "Assets/Resources/players.csv";  // Update path to Assets/Resources/
		teamsFilePath = "Assets/Resources/teams.csv";      // Update path to Assets/Resources/

		GenerateSampleTeamsAndPlayers();
		}

	private void GenerateSampleTeamsAndPlayers()
		{
		using (StreamWriter playersWriter = new(playersFilePath, false))
		using (StreamWriter teamsWriter = new(teamsFilePath, false))
			{
			// Write header to players.csv
			playersWriter.WriteLine("TeamName,TeamId,PlayerName,LifetimeGamesPlayed,LifetimeGamesWon,CurrentSeasonPointsAwarded,LifetimeGamesPlayed," +
				"CurrentSeasonBreakAndRun,CurrentSeasonDefensiveShotAverage,CurrentSeasonMatchesPlayed,CurrentSeasonMatchesWon,CurrentSeasonMiniSlams," +
				"CurrentSeasonNineOnTheSnap,CurrentSeasonPaPercentage,CurrentSeasonPointsAwarded,CurrentSeasonPointsPerMatch,CurrentSeasonPpm,CurrentSeasonShutouts," +
				"CurrentSeasonSkillLevel,CurrentSeasonTotalPoints,LifetimeBreakAndRun,LifetimeDefensiveShotAverage,LifetimeGamesPlayed," +
				"LifetimeGamesWon,LifetimeMatchesPlayed,LifetimeMatchesWon,LifetimeMiniSlams,LifetimeNineOnTheSnap,LifetimeShutouts,LifetimeMatchesPlayedInLast2Years");

			// Write header to teams.csv
			teamsWriter.WriteLine("TeamName,TeamId,TeamSkillLevel");

			for (int i = 0; i < numberOfTeams; i++)
				{
				int teamId = i + 1;
				string teamName = teamNames[Random.Range(0, teamNames.Length)];
				int teamSkillLevel = Random.Range(1, 10); // Skill level between 1 and 9

				// Write the team data to teams.csv
				teamsWriter.WriteLine($"{teamName},{teamId},{teamSkillLevel}");

				// Generate 3 players for each team
				for (int j = 0; j < 3; j++)
					{
					string playerName = GetRandomName();
					int skillLevel = Random.Range(1, 10); // Skill level between 1 and 9
					int totalPointsEarned = Random.Range(0, pointsRequiredToWin[skillLevel - 1]); // Random points based on skill level

					Player player = GenerateRandomPlayer(teamId, teamName, playerName, skillLevel, totalPointsEarned);

					// Write the player data to players.csv
					playersWriter.WriteLine(player.ToCsv());
					}
				}
			}

		Debug.Log("Sample data generation complete.");
		}

	private string GetRandomName()
		{
		string firstName = Random.Range(0, 2) == 0 ? firstNamesMale[Random.Range(0, firstNamesMale.Length)] : firstNamesFemale[Random.Range(0, firstNamesFemale.Length)];
		string lastName = lastNames[Random.Range(0, lastNames.Length)];

		return $"{firstName} {lastName}";
		}

	// Generates a random player with random stats
	private Player GenerateRandomPlayer(int teamId, string teamName, string playerName, int skillLevel, int totalPointsEarned)
		{
		int lifetimeGamesPlayed = Random.Range(100, 300);
		int lifetimeGamesWon = Random.Range(30, lifetimeGamesPlayed);
		int lifetimeMiniSlams = Random.Range(0, 5);
		int lifetimeNineOnTheSnap = Random.Range(0, 5);
		int lifetimeShutouts = Random.Range(0, 5);
		int lifetimeMatchesPlayedInLast2Years = Random.Range(10, 50);

		int currentSeasonMatchesPlayed = Random.Range(10, 50);
		int currentSeasonMatchesWon = Random.Range(0, currentSeasonMatchesPlayed);
		int currentSeasonPointsAwarded = Random.Range(0, totalPointsEarned);
		float currentSeasonPointsPerMatch = currentSeasonMatchesPlayed == 0 ? 0 : (float) currentSeasonPointsAwarded / currentSeasonMatchesPlayed;

		return new Player(
			teamId, teamName, playerName, skillLevel, currentSeasonMatchesPlayed, currentSeasonMatchesWon,
			currentSeasonPointsAwarded, currentSeasonPointsPerMatch, Random.Range(0, 5), skillLevel, totalPointsEarned,
			lifetimeGamesPlayed, lifetimeGamesWon, lifetimeMiniSlams, lifetimeNineOnTheSnap, lifetimeShutouts,
			lifetimeMatchesPlayedInLast2Years, Random.Range(0, 5), Random.Range(0f, 1f)
		);
		}
	}
