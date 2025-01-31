using SQLite;

using System.Collections.Generic;

public class Team
	{
	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }

	// Make sure Name is what you want to be saved to SQLite
	public string Name { get; set; }

	// Parameterless constructor for SQLite
	public Team() { }

	// Constructor to initialize team name
	public Team(string name)
		{
		Name = name;
		}

	// --- Add a player to the team --- //
	// Note: This is a runtime method, not something you need to persist in SQLite.
	public void AddPlayer(Player player)
		{
		// Here you'd typically add to a collection in-memory, not directly tied to the database.
		// Use the Player's TeamId to link players to the team in the database.
		}
	}
