using MyGame.Database;  // Reference to your custom SQLite helper namespace
using UnityEngine;
using System.Collections.Generic;

public class Team
	{
	// --- SQLite Attributes --- //
	[PrimaryKey, AutoIncrement]
	public int TeamId { get; set; }  // Unique ID for each team

	// --- Team Name --- //
	public string TeamName { get; set; }  // Team's name (getter/setter)

	// Constructor
	public Team(string teamName)
		{
		this.TeamName = teamName;
		}

	// --- Add Player to Team --- //
	public bool AddPlayer(Player player)
		{
		int teamSkillLevel = GetTeamSkillLevel(); // Get the current skill level of the team

		// Check if adding the player would violate the 23-Rule or exceed the max senior player limit
		if (teamSkillLevel + player.SkillLevel > 23 || CountSeniorPlayers() >= 2)
			{
			Debug.LogWarning("Cannot add player: Violates 23-Rule or too many senior players.");
			return false; // Reject player if it violates the 23-Rule or too many senior players
			}

		// Insert the player into the database (teamId is set when adding the player)
		SQLiteHelper.InsertPlayer(player);  // Use custom helper to insert player
		return true;
		}

	// --- Get Team Skill Level --- //
	public int GetTeamSkillLevel()
		{
		int totalSkillLevel = 0;
		foreach (var player in SQLiteHelper.GetPlayersForTeam(TeamId))  // Fetch players using custom SQLite helper
			{
			totalSkillLevel += player.SkillLevel;
			}
		return totalSkillLevel;
		}

	// --- Count Senior Players --- //
	public int CountSeniorPlayers()
		{
		int seniorPlayers = 0;
		foreach (var player in SQLiteHelper.GetPlayersForTeam(TeamId))  // Fetch players using custom SQLite helper
			{
			if (player.SkillLevel >= 6)
				{
				seniorPlayers++;
				}
			}
		return seniorPlayers;
		}

	// --- Remove Player from Team --- //
	public void RemovePlayer(Player player)
		{
		// Remove player from database using custom SQLite helper
		SQLiteHelper.DeletePlayer(player);
		}
	}
