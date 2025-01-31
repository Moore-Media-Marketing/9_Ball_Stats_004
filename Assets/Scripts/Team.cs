using SQLite;

using System.Collections.Generic;

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
	// The player class should also be defined and contain a TeamId to link to the Team.
	public void AddPlayer(Player player)
		{
		// Example: Link player with this team via TeamId
		player.TeamId = this.Id;
		DatabaseManager.Instance.SavePlayer(player); // Assuming you have a save method for players.
		}

	// --- SQLite CRUD Operations --- //
	// Save this team to the SQLite database
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
