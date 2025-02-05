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
			playersWriter.WriteLine("TeamName,PlayerName,SkillLevel,LifetimeGamesPlayed,LifetimeGamesWon,LifetimeMiniSlams,LifetimeNineOnTheSnap,LifetimeShutouts,CurrentSeasonBreakAndRun,CurrentSeasonDefensiveShotAverage,CurrentSeasonMatchesPlayed,CurrentSeasonMatchesWon,CurrentSeasonMiniSlams,CurrentSeasonNineOnTheSnap,CurrentSeasonPaPercentage,CurrentSeasonPointsAwarded,CurrentSeasonPointsPerMatch,CurrentSeasonPpm,CurrentSeasonShutouts,CurrentSeasonSkillLevel,CurrentSeasonTotalPoints");

			// Write header to teams.csv
			teamsWriter.WriteLine("TeamName");

			for (int i = 0; i < numberOfTeams; i++)
				{
				Team team = GenerateTeam(i); // Generate a new team, passing the team ID
				teamsWriter.WriteLine(team.TeamName); // Write team name to teams.csv

				for (int j = 0; j < 8; j++) // Generate 8 players for each team
					{
					Player newPlayer = GeneratePlayer(team.TeamId, team.TeamName); // Pass the team ID and team name
					playersWriter.WriteLine(newPlayer.ToCsv()); // Write player data to players.csv
					}
				}
			}

		Debug.Log("Sample teams and players generated and saved to CSV.");
		}

	private Team GenerateTeam(int teamId)
		{
		string teamName = teamNames[Random.Range(0, teamNames.Length)];
		return new Team(teamId, teamName);
		}

	private Player GeneratePlayer(int teamId, string teamName)
		{
		string firstName = Random.Range(0, 2) == 0 ? firstNamesMale[Random.Range(0, firstNamesMale.Length)] : firstNamesFemale[Random.Range(0, firstNamesFemale.Length)];
		string lastName = lastNames[Random.Range(0, lastNames.Length)];
		string playerName = $"{firstName} {lastName}";
		int skillLevel = Random.Range(1, 10); // Skill level between 1 and 9

		// Calculate points required to win based on skill level
		int pointsToWin = pointsRequiredToWin[skillLevel - 1];

		// Simulate the player's performance over 16 matches
		int lifetimeGamesPlayed = 16; // Fixed to 16 games per season
		int lifetimeGamesWon = 0;
		int lifetimeMiniSlams = Random.Range(0, 2);
		int lifetimeNineOnTheSnap = Random.Range(0, 2);
		int lifetimeShutouts = 0;
		int totalPointsEarned = 0;

		// Adjust stats based on skill level
		float winRate = Mathf.Clamp(skillLevel / 9f, 0.2f, 1f); // Higher skill level increases win rate
		float breakAndRunRate = Mathf.Clamp(skillLevel / 9f, 0.1f, 0.8f); // Higher skill level increases break-and-run rate
		float pointsPerMatchRate = Mathf.Clamp(skillLevel / 9f, 0.5f, 1.5f); // Higher skill level increases points per match

		// Randomize based on skill level
		for (int i = 0; i < lifetimeGamesPlayed; i++)
			{
			bool wonGame = Random.Range(0f, 1f) < winRate; // Win or lose based on skill level
			if (wonGame)
				lifetimeGamesWon++;

			int pointsScored = Mathf.RoundToInt(pointsToWin * Random.Range(0.7f, 1.2f)); // Scale points scored by skill level
			totalPointsEarned += pointsScored;

			// Simulate break-and-run success
			bool breakAndRun = Random.Range(0f, 1f) < breakAndRunRate;
			if (breakAndRun)
				lifetimeMiniSlams++;

			// Simulate 9-ball on the break
			bool nineOnTheBreak = Random.Range(0f, 1f) < (breakAndRunRate / 2f); // Lower chance for lower skill levels
			if (nineOnTheBreak)
				lifetimeNineOnTheSnap++;

			// Simulate shutouts
			bool shutout = Random.Range(0f, 1f) < (winRate * 0.2f); // Lower chance for lower skill levels
			if (shutout)
				lifetimeShutouts++;
			}

		// Current season stats (randomized, adjusted by skill level)
		int currentSeasonBreakAndRun = Mathf.RoundToInt(breakAndRunRate * 5); // Higher skill = more break-and-runs
		float currentSeasonDefensiveShotAverage = Random.Range(0f, 1f); // This stat may not be directly affected by skill level
		int currentSeasonMatchesPlayed = 16; // Fixed to 16
		int currentSeasonMatchesWon = Mathf.RoundToInt(currentSeasonMatchesPlayed * winRate); // Wins scaled by skill level
		int currentSeasonMiniSlams = Mathf.RoundToInt(lifetimeMiniSlams * skillLevel / 9f); // More mini slams for higher skill
		int currentSeasonNineOnTheSnap = Mathf.RoundToInt(lifetimeNineOnTheSnap * skillLevel / 9f); // More 9-ball on break for higher skill
		float currentSeasonPaPercentage = Mathf.RoundToInt(50f + (skillLevel * 5)); // Skill level scales performance percentage
		int currentSeasonPointsAwarded = Mathf.RoundToInt(totalPointsEarned / lifetimeGamesPlayed); // Scale points by performance
		float currentSeasonPointsPerMatch = Mathf.Round(pointsPerMatchRate * 4); // Higher skill scores more points per match
		float currentSeasonPpm = currentSeasonPointsPerMatch;
		int currentSeasonShutouts = lifetimeShutouts; // Based on lifetime shutouts
		int currentSeasonSkillLevel = skillLevel;
		int currentSeasonTotalPoints = currentSeasonPointsAwarded + Random.Range(0, 20); // Add a bit of randomness

		// Create the player with the stats generated above
		Player newPlayer = new Player(playerName, skillLevel, 0, 0, 0)
			{
			TeamId = teamId,
			PlayerName = playerName,
			SkillLevel = skillLevel,
			TeamName = teamName,
			LifetimeGamesPlayed = lifetimeGamesPlayed,
			LifetimeGamesWon = lifetimeGamesWon,
			LifetimeDefensiveShotAvg = Mathf.Round(Random.Range(1f, 10f)), // Round to whole number
			LifetimeMiniSlams = lifetimeMiniSlams,
			LifetimeNineOnTheSnap = lifetimeNineOnTheSnap,
			LifetimeShutouts = lifetimeShutouts,
			CurrentSeasonBreakAndRun = currentSeasonBreakAndRun,
			CurrentSeasonDefensiveShotAverage = Mathf.Round(currentSeasonDefensiveShotAverage * 100) / 100f, // Round to 2 decimal places
			CurrentSeasonMatchesPlayed = currentSeasonMatchesPlayed,
			CurrentSeasonMatchesWon = currentSeasonMatchesWon,
			CurrentSeasonMiniSlams = currentSeasonMiniSlams,
			CurrentSeasonNineOnTheSnap = currentSeasonNineOnTheSnap,
			CurrentSeasonPaPercentage = Mathf.Round(currentSeasonPaPercentage), // Round to whole number percentage
			CurrentSeasonPointsAwarded = currentSeasonPointsAwarded,
			CurrentSeasonPointsPerMatch = Mathf.Round(currentSeasonPointsPerMatch), // Round to whole number
			CurrentSeasonPpm = Mathf.Round(currentSeasonPpm), // Round to whole number
			CurrentSeasonShutouts = currentSeasonShutouts,
			CurrentSeasonSkillLevel = currentSeasonSkillLevel,
			CurrentSeasonTotalPoints = currentSeasonTotalPoints
			};

		return newPlayer;
		}
	}
