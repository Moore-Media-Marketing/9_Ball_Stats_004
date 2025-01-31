using System.Collections.Generic;

using SQLite;

public class Team
	{
	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }

	// Team name to save in SQLite
	public string Name { get; set; }

	// Parameterless constructor for SQLite
	public Team() { }

	// Constructor to initialize team name
	public Team(string name)
		{
		Name = name;
		}

	// --- Add a player to the team --- //
	public void AddPlayer(Player player)
		{
		// Link player with this team via TeamId
		player.TeamId = this.Id;
		DatabaseManager.Instance.SavePlayer(player); // Save player in the database
		}

	// --- Fetch all players for this team --- //
	public List<Player> GetPlayers()
		{
		// Fetch players using the DatabaseManager (which interacts with SQLite)
		return DatabaseManager.Instance.GetPlayersByTeam(this.Id);
		}

	// --- SQLite CRUD Operations --- //
	public void SaveToDatabase(SQLiteConnection connection)
		{
		connection.Insert(this); // Save the team to the database
		}

	// Fetch all teams from the database
	public static List<Team> GetAllTeams(SQLiteConnection connection)
		{
		return connection.Table<Team>().ToList(); // Retrieve all teams from the database
		}

	// Fetch a team by its ID
	public static Team GetTeamById(SQLiteConnection connection, int id)
		{
		return connection.Find<Team>(id); // Fetch a specific team by its ID
		}
	}
