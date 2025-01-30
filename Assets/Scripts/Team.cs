using System;
using System.Collections.Generic;

[System.Serializable]
public class Team
	{
	public string teamName;        // Team's name
	public List<Player> Players { get; set; }   // Public property for players

	// Event to notify about team list changes
	public event Action OnTeamListChanged;

	// Default constructor for initialization without parameters
	public Team()
		{
		teamName = "Default Team";
		Players = new List<Player>(); // Initialize as an empty list of players
		}

	// Constructor to initialize the team with a name and players
	public Team(string teamName, List<Player> players = null)
		{
		this.teamName = teamName;
		Players = players ?? new List<Player>(); // If no players are provided, initialize an empty list
		}

	// Property for team name
	public string Name
		{
		get { return teamName; }
		set { teamName = value; }
		}

	// Method to add a player and notify listeners
	public void AddPlayer(Player player)
		{
		Players.Add(player);
		OnTeamListChanged?.Invoke(); // Notify listeners
		}

	// Method to remove a player and notify listeners
	public void RemovePlayer(Player player)
		{
		Players.Remove(player);
		OnTeamListChanged?.Invoke(); // Notify listeners
		}

	// Method to get the total skill level of all players on the team
	public int GetTotalSkillLevel()
		{
		int totalSkillLevel = 0;
		foreach (var player in Players)
			{
			totalSkillLevel += player.skillLevel;
			}
		return totalSkillLevel;
		}

	// Method to calculate the team's win percentage
	public float GetWinPercentage()
		{
		int totalGamesPlayed = 0;
		int totalGamesWon = 0;

		foreach (var player in Players)
			{
			totalGamesPlayed += player.gamesPlayed;
			totalGamesWon += player.gamesWon;
			}

		if (totalGamesPlayed == 0) return 0f;

		return (float) totalGamesWon / totalGamesPlayed * 100f;
		}

	// Method to calculate the total games played by all players
	public int GetTotalGamesPlayed()
		{
		int totalGamesPlayed = 0;
		foreach (var player in Players)
			{
			totalGamesPlayed += player.gamesPlayed;
			}
		return totalGamesPlayed;
		}

	// Method to calculate the total games won by all players
	public int GetTotalGamesWon()
		{
		int totalGamesWon = 0;
		foreach (var player in Players)
			{
			totalGamesWon += player.gamesWon;
			}
		return totalGamesWon;
		}

	// Method to compare two teams based on win percentage
	public static string CompareTeams(Team team1, Team team2)
		{
		float winPercentage1 = team1.GetWinPercentage();
		float winPercentage2 = team2.GetWinPercentage();

		if (winPercentage1 > winPercentage2)
			{
			return $"{team1.Name} wins!";
			}
		else if (winPercentage1 < winPercentage2)
			{
			return $"{team2.Name} wins!";
			}
		return "It's a tie!";
		}
	}
