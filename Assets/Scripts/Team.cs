// --- Region: Team Class --- //
using System.Collections.Generic;

public class Team
	{
	// --- Properties --- //
	public int Id { get; set; }
	public string Name { get; set; }
	public List<Player> Players { get; set; } // List of players in the team

	// --- Parameterless Constructor for SQLite --- //
	public Team()
		{
		Players = new List<Player>(); // Initialize the list of players
		}

	// --- Constructor with parameters --- //
	public Team(string name)
		{
		Name = name;
		Players = new List<Player>(); // Initialize the list of players
		}
	}
// --- End Region --- //
