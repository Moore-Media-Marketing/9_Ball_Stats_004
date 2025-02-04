using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite; // Ensure you have a SQLite library installed
using System.Data;
using System.IO;

// --- Region: Sample Data Generator Class Definition --- //
public class SampleDataGenerator:MonoBehaviour
	{
	// --- Region: Public Variables --- //
	[Tooltip("The number of teams to generate.")]
	public int numberOfTeams = 10; // Number of teams to generate
								   // --- End Region --- //

	// --- Region: Sample Data --- //
	private static readonly string[] firstNamesMale = { "James", "John", "Robert", "Michael", "William", "David", "Richard", "Charles", "Joseph", "Thomas" };
	private static readonly string[] firstNamesFemale = { "Mary", "Patricia", "Linda", "Barbara", "Elizabeth", "Jennifer", "Maria", "Susan", "Margaret", "Dorothy" };
	private static readonly string[] lastNames = { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez" };
	private static readonly string[] teamNames = { "Atomic Squirrels", "Crimson Cobras", "Electric Eels", "Phantom Phantoms", "Mystic Moose" };
	// --- End Region --- //

	// --- Region: Handicap Points for Each Skill Level --- //
	private readonly int[] pointsRequiredToWin = { 14, 19, 25, 31, 38, 46, 55, 65, 75 }; // Points required based on skill level (1 to 9)
																						 // --- End Region --- //

	// --- Region: Start Method --- //
	private void Start()
		{
		GenerateSampleTeamsAndPlayers(); // Generate sample data on start
		}
	// --- End Region --- //

	// --- Region: Generate Sample Data --- //
	public void GenerateSampleTeamsAndPlayers() // Make this method public
		{
		string dbPath = Path.Combine(Application.persistentDataPath, "billiardsDatabase.db");

		if (!File.Exists(dbPath))
			{
			CreateDatabase(dbPath); // Create the database if it doesn't exist
			}

		for (int i = 0; i < numberOfTeams; i++)
			{
			Team newTeam = GenerateTeam(); // Generate a new team

			AddTeamToDatabase(dbPath, newTeam); // Add team to database

			int numberOfPlayersForTeam = Random.Range(5, 9); // Randomly choose number of players for the team

			List<int> playerSkillLevels = new(); // List to store player skill levels
			List<string> playerNames = new();  // List to store player names

			for (int j = 0; j < numberOfPlayersForTeam; j++)
				{
				Player newPlayer = GeneratePlayer(newTeam.id); // Generate a new player for the team
				AddPlayerToDatabase(dbPath, newPlayer); // Add player to database
				playerSkillLevels.Add(newPlayer.skillLevel); // Add player skill level to the list
				playerNames.Add(newPlayer.name); // Add player name to the list
				}
			}

		Debug.Log("Sample teams and players generated and saved to SQLite database successfully.");
		}
	// --- End Region --- //

	// --- Region: Clear Sample Data --- //
	public void ClearSampleData() // Public method to clear the sample data
		{
		string dbPath = Path.Combine(Application.persistentDataPath, "billiardsDatabase.db");
		DeleteDatabase(dbPath); // Delete the existing database to clear the data
		Debug.Log("Sample Data cleared from SQLite database.");
		}
	// --- End Region --- //

	// --- Region: Create Database --- //
	private void CreateDatabase(string dbPath)
		{
		SqliteConnection.CreateFile(dbPath);
		SqliteConnection connection = new($"URI=file:{dbPath}");

		connection.Open();

		string createTeamTableQuery = @"
            CREATE TABLE IF NOT EXISTS Teams (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT
            );
        ";

		string createPlayerTableQuery = @"
            CREATE TABLE IF NOT EXISTS Players (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT,
                SkillLevel INTEGER,
                TeamId INTEGER,
                FOREIGN KEY(TeamId) REFERENCES Teams(Id)
            );
        ";

		SqliteCommand command = new(createTeamTableQuery, connection);
		command.ExecuteNonQuery();

		command = new SqliteCommand(createPlayerTableQuery, connection);
		command.ExecuteNonQuery();

		connection.Close();

		Debug.Log("Database created successfully.");
		}
	// --- End Region --- //

	// --- Region: Add Team to Database --- //
	private void AddTeamToDatabase(string dbPath, Team team)
		{
		SqliteConnection connection = new($"URI=file:{dbPath}");
		connection.Open();

		string insertTeamQuery = "INSERT INTO Teams (Name) VALUES (@Name)";
		SqliteCommand command = new(insertTeamQuery, connection);
		command.Parameters.AddWithValue("@Name", team.name);
		command.ExecuteNonQuery();

		connection.Close();
		}
	// --- End Region --- //

	// --- Region: Add Player to Database --- //
	private void AddPlayerToDatabase(string dbPath, Player player)
		{
		SqliteConnection connection = new($"URI=file:{dbPath}");
		connection.Open();

		string insertPlayerQuery = "INSERT INTO Players (Name, SkillLevel, TeamId) VALUES (@Name, @SkillLevel, @TeamId)";
		SqliteCommand command = new(insertPlayerQuery, connection);
		command.Parameters.AddWithValue("@Name", player.name);
		command.Parameters.AddWithValue("@SkillLevel", player.skillLevel);
		command.Parameters.AddWithValue("@TeamId", player.teamId);
		command.ExecuteNonQuery();

		connection.Close();
		}
	// --- End Region --- //

	// --- Region: Delete Database --- //
	private void DeleteDatabase(string dbPath)
		{
		if (File.Exists(dbPath))
			{
			File.Delete(dbPath);
			Debug.Log("Database deleted successfully.");
			}
		else
			{
			Debug.LogError("Database not found.");
			}
		}
	// --- End Region --- //

	// --- Region: Generate Team Method --- //
	private Team GenerateTeam()
		{
		string teamName = GenerateUniqueTeamName(); // Generate a unique team name

		Team newTeam = new(teamName); // Create a new team

		Debug.Log($"Generated Team: {newTeam.name}");

		return newTeam;
		}
	// --- End Region --- //

	// --- Region: Generate Player Method --- //
	private Player GeneratePlayer(int teamId)
		{
		// Randomly select first name and last name
		string firstName = Random.Range(0, 2) == 0 ? firstNamesMale[Random.Range(0, firstNamesMale.Length)] : firstNamesFemale[Random.Range(0, firstNamesFemale.Length)];
		string lastName = lastNames[Random.Range(0, lastNames.Length)];

		string playerName = $"{firstName} {lastName}"; // Full name of the player
		int skillLevel = Random.Range(1, 10); // Random skill level

		// Generate lifetime and current season stats
		Player newPlayer = new(playerName, skillLevel, teamId);

		Debug.Log($"Generated Player: {newPlayer.name} (Skill Level: {newPlayer.skillLevel}) for Team ID: {teamId}");

		return newPlayer;
		}
	// --- End Region --- //

	// --- Region: Generate Unique Team Name Method --- //
	private string GenerateUniqueTeamName()
		{
		string teamName = $"{teamNames[Random.Range(0, teamNames.Length)]} {teamNames[Random.Range(0, teamNames.Length)]}";
		return teamName;
		}
	// --- End Region --- //

	// --- Region: Additional Functions --- //
	// Additional utility functions can be added here if needed
	// --- End Region --- //
	}
// --- End Region: Sample Data Generator Class Definition --- //

public class Team
	{
	public int id;
	public string name;

	public Team(string name)
		{
		this.name = name;
		}
	}

public class Player
	{
	public string name;
	public int skillLevel;
	public int teamId;

	public Player(string name, int skillLevel, int teamId)
		{
		this.name = name;
		this.skillLevel = skillLevel;
		this.teamId = teamId;
		}
	}
