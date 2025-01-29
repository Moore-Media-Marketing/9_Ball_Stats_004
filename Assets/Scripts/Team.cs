using NickWasHere;
using System.Collections.Generic;

[System.Serializable]
public class Team
	{
	public string teamName;        // Team's name
	public List<Player> players;   // List of players in the team

	// Default constructor for initialization without parameters
	public Team()
		{
		teamName = "Default Team";
		players = new List<Player>();  // Initialize with an empty player list
		}

	// Constructor to initialize the team with a name and players
	// Constructor for creating a team with a name and an empty list of players by default
	public Team(string teamName, List<Player> players = null)
		{
		this.teamName = teamName;
		this.players = players ?? new List<Player>();  // Use an empty list if no players are passed
		}


	// Property for team name
	public string Name
		{
		get { return teamName; }
		set { teamName = value; }
		}

	// Method to get the total skill level of all players on the team
	public int GetTotalSkillLevel()
		{
		int totalSkillLevel = 0;
		foreach (var player in players)
			{
			totalSkillLevel += player.skillLevel;
			}
		return totalSkillLevel;
		}

	// Method to get the total games played by all players in the team
	public int GetTotalGamesPlayed()
		{
		int totalGamesPlayed = 0;
		foreach (var player in players)
			{
			totalGamesPlayed += player.gamesPlayed;
			}
		return totalGamesPlayed;
		}

	// Method to get the total games won by all players in the team
	public int GetTotalGamesWon()
		{
		int totalGamesWon = 0;
		foreach (var player in players)
			{
			totalGamesWon += player.gamesWon;
			}
		return totalGamesWon;
		}

	// Method to calculate the average win percentage of the team
	public float GetWinPercentage()
		{
		float totalWinPercentage = 0f;
		foreach (var player in players)
			{
			totalWinPercentage += player.WinPercentage;
			}
		return players.Count > 0 ? totalWinPercentage / players.Count : 0f;
		}
	}
