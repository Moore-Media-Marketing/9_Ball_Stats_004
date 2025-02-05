using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections.Generic;

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
			playersWriter.WriteLine("TeamName,PlayerName,SkillLevel,LifetimeGamesPlayed,LifetimeGamesWon,LifetimeMiniSlams,LifetimeNineOnTheSnap,LifetimeShutouts");

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
		int lifetimeGamesPlayed = matchesPerSeason;
		int lifetimeGamesWon = 0;
		int lifetimeMiniSlams = Random.Range(0, 5);
		int lifetimeNineOnTheSnap = Random.Range(0, 3);
		int lifetimeShutouts = Random.Range(0, 2);
		int totalPointsEarned = 0;

		for (int i = 0; i < matchesPerSeason; i++)
			{
			int pointsScored = Random.Range(pointsToWin - 5, pointsToWin + 5); // Randomize points scored around required win points
			totalPointsEarned += pointsScored;

			if (pointsScored >= pointsToWin)
				lifetimeGamesWon++; // Player wins if they score enough points
			}

		Player newPlayer = new(playerName, skillLevel, 0, 0, 0)
			{
			TeamId = teamId,
			PlayerName = playerName,
			SkillLevel = skillLevel,
			TeamName = teamName,  // Save the team name here for CSV
			LifetimeGamesPlayed = lifetimeGamesPlayed,
			LifetimeGamesWon = lifetimeGamesWon,
			LifetimeDefensiveShotAvg = Random.Range(1f, 10f), // Random lifetime defensive shot avg
			LifetimeMiniSlams = lifetimeMiniSlams,
			LifetimeNineOnTheSnap = lifetimeNineOnTheSnap,
			LifetimeShutouts = lifetimeShutouts
			};

		return newPlayer;
		}
	}
