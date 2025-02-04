using System.Collections.Generic;

using SQLite; // Required for SQLite attributes

using UnityEngine;

public class Team
	{
	// --- SQLite Attributes --- //
	[PrimaryKey, AutoIncrement]
	public int TeamId { get; set; }  // Unique ID for each team

	// --- Team Name --- //
	public string TeamName { get; set; }  // Team's name (getter/setter)

	// --- Players List --- //
	[Ignore]  // Ignore this field for SQLite since it's a non-database entity
	public List<Player> Players { get; set; }  // List of players in the team

	// --- Constructor --- //
	// Initializes a Team with a given team name and an empty players list
	public Team(string teamName)
		{
		this.TeamName = teamName;
		this.Players = new List<Player>(); // Initialize the players list
		}

	// --- Add Player to Team --- //
	// Adds a player to the team after checking the 23-Rule and senior player count
	public bool AddPlayer(Player player)
		{
		int teamSkillLevel = GetTeamSkillLevel(); // Get the current skill level of the team

		// Check if adding the player would violate the 23-Rule or exceed the max senior player limit
		if (teamSkillLevel + player.SkillLevel > 23 || CountSeniorPlayers() >= 2)
			{
			Debug.LogWarning("Cannot add player: Violates 23-Rule or too many senior players.");
			return false; // Reject player if it violates the 23-Rule or too many senior players
			}

		Players.Add(player); // Add player to the team
		return true;
		}

	// --- Calculate Team Skill Level --- //
	// Returns the total skill level of all players in the team
	public int GetTeamSkillLevel()
		{
		int totalSkillLevel = 0;
		foreach (Player player in Players)
			{
			totalSkillLevel += player.SkillLevel;  // Sum up the skill level of each player
			}
		return totalSkillLevel;
		}

	// --- Count Senior Players --- //
	// Returns the count of senior players (SkillLevel 6-9) in the team
	public int CountSeniorPlayers()
		{
		int seniorPlayers = 0;
		foreach (Player player in Players)
			{
			if (player.SkillLevel >= 6)  // Check if player is considered senior
				{
				seniorPlayers++;
				}
			}
		return seniorPlayers;
		}

	// --- Remove Player from Team --- //
	// Removes a player from the team if they exist in the list
	public void RemovePlayer(Player player)
		{
		if (Players.Contains(player))
			{
			Players.Remove(player);  // Remove the player from the team
			}
		}
	}
