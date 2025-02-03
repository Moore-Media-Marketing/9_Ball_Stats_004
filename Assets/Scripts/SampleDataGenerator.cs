using System.Collections.Generic;
using System.Linq;

using UnityEngine;

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

	// --- Region: Start Method --- //
	private void Start()
		{
		GenerateSampleTeamsAndPlayers();
		}
	// --- End Region: Start Method --- //

	// --- Region: Generate Sample Teams and Players --- //
	private void GenerateSampleTeamsAndPlayers()
		{
		// --- Region: Team Generation Loop --- //
		for (int i = 0; i < numberOfTeams; i++)
			{
			Team newTeam = GenerateTeam();
			DatabaseManager.Instance.AddTeam(newTeam);

			// Randomize number of players per team between 5 and 8
			int numberOfPlayersForTeam = UnityEngine.Random.Range(5, 9);

			// --- Region: Player Generation Loop --- //
			for (int j = 0; j < numberOfPlayersForTeam; j++)
				{
				Player newPlayer = GeneratePlayer(newTeam.id);
				DatabaseManager.Instance.AddPlayer(newPlayer);
				}
			// --- End Region: Player Generation Loop --- //

			// After adding the team and players, update PlayerPrefs
			SaveToPlayerPrefs();  // Save to PlayerPrefs after each team's players are added
			}
		// --- End Region: Team Generation Loop --- //
		}
	// --- End Region: Generate Sample Teams and Players --- //

	// --- Region: Generate Team Method --- //
	private Team GenerateTeam()
		{
		string teamName = teamNames[UnityEngine.Random.Range(0, teamNames.Length)];

		// Get all teams from DatabaseManager
		var allTeams = DatabaseManager.Instance.GetAllTeams();

		// Ensure we don't get an error when no teams exist
		int teamId = allTeams.Any() ? allTeams.Max(t => t.id) + 1 : 1;  // If there are teams, get the max ID and add 1, else start from 1

		Team newTeam = new(teamId, teamName); // Pass both id and name to the constructor

		int totalSkillLevel = 0;
		List<int> playerSkillLevels = new();

		// Ensure the total skill level doesn't exceed 23 (for 5 players)
		for (int i = 0; i < 5; i++)
			{
			int skillLevel = UnityEngine.Random.Range(1, 10);
			totalSkillLevel += skillLevel;
			playerSkillLevels.Add(skillLevel);

			// If total exceeds 23, adjust the current skill level to fit
			if (totalSkillLevel > 23)
				{
				skillLevel = Mathf.Max(1, 23 - (totalSkillLevel - skillLevel));
				playerSkillLevels[i] = skillLevel;
				totalSkillLevel = 23;
				}
			}

		newTeam.playerSkillLevels = playerSkillLevels; // Store the skill levels for this team

		return newTeam;
		}
	// --- End Region: Generate Team Method --- //

	// --- Region: Generate Player Method --- //
	private Player GeneratePlayer(int teamId)
		{
		// Randomly select male or female first name
		string firstName = UnityEngine.Random.Range(0, 2) == 0 ? firstNamesMale[UnityEngine.Random.Range(0, firstNamesMale.Length)] : firstNamesFemale[UnityEngine.Random.Range(0, firstNamesFemale.Length)];
		string lastName = lastNames[UnityEngine.Random.Range(0, lastNames.Length)];

		string playerName = $"{firstName} {lastName}";

		int skillLevel = UnityEngine.Random.Range(1, 10); // Random skill level between 1 and 9

		Player newPlayer = new(playerName, skillLevel, teamId)
			{
			// Assign points required to win based on skill level
			PointsRequiredToWin = pointsRequiredToWin[skillLevel - 1], // Points required based on skill level

			// --- Region: Lifetime Stats --- //
			lifetimeMatchesPlayed = UnityEngine.Random.Range(0, 100),
			lifetimeMatchesWon = UnityEngine.Random.Range(0, 50),
			lifetimeDefensiveShotAvg = UnityEngine.Random.Range(0f, 10f),
			lifetimeGamesPlayed = UnityEngine.Random.Range(0, 200),
			lifetimeGamesWon = UnityEngine.Random.Range(0, 150),
			lifetimeMiniSlams = UnityEngine.Random.Range(0, 10),
			lifetimeNineOnTheSnap = UnityEngine.Random.Range(0, 5),
			lifetimeShutouts = UnityEngine.Random.Range(0, 10),
			// --- End Region: Lifetime Stats --- //

			// --- Region: Current Season Stats --- //
			currentSeasonBreakAndRun = UnityEngine.Random.Range(0, 5),
			currentSeasonDefensiveShotAverage = UnityEngine.Random.Range(0f, 10f),
			currentSeasonMatchesPlayed = UnityEngine.Random.Range(0, 50),
			currentSeasonMatchesWon = UnityEngine.Random.Range(0, 30),
			currentSeasonMiniSlams = UnityEngine.Random.Range(0, 5),
			currentSeasonNineOnTheSnap = UnityEngine.Random.Range(0, 3),
			currentSeasonPaPercentage = UnityEngine.Random.Range(0f, 100f), // Assumed as a percentage
			currentSeasonPointsAwarded = UnityEngine.Random.Range(0, 100),
			currentSeasonPointsPerMatch = UnityEngine.Random.Range(0f, 10f),
			currentSeasonShutouts = UnityEngine.Random.Range(0, 5),
			currentSeasonSkillLevel = skillLevel, // Use the same skill level for simplicity
			currentSeasonTotalPoints = UnityEngine.Random.Range(0, 200),
			// --- End Region: Current Season Stats --- //
			};

		return newPlayer;
		}
	// --- End Region: Generate Player Method --- //

	// --- Region: Save to PlayerPrefs --- //
	private void SaveToPlayerPrefs()
		{
		// Save all teams
		var allTeams = DatabaseManager.Instance.GetAllTeams();
		for (int i = 0; i < allTeams.Count; i++)
			{
			PlayerPrefs.SetString($"Team_{allTeams[i].id}_Name", allTeams[i].name); // Update to match 'name' property
			PlayerPrefs.SetString($"Team_{allTeams[i].id}_PlayerSkillLevels", string.Join(",", allTeams[i].playerSkillLevels));
			}

		PlayerPrefs.Save(); // Save to PlayerPrefs
		}
	// --- End Region: Save to PlayerPrefs --- //

	// --- Region: Additional Functions --- //
	// Any other utility functions would go here
	// --- End Region: Additional Functions --- //
	}
