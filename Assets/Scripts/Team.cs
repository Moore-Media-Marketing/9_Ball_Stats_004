using System;
using System.Collections.Generic;

[Serializable]
public class Team
	{
	// --- Team Variables ---
	public string TeamName; // Name of the team

	public List<Player> Players; // List of players in the team
	public float WinPercentage { get; private set; } // Win percentage, to be set manually or via a method

	// --- Constructor to initialize Team with teamName, winPercentage, and list of players ---
	public Team(string teamName, float winPercentage, List<Player> players)
		{
		TeamName = teamName;
		WinPercentage = winPercentage;
		Players = players ?? new List<Player>(); // If players is null, initialize an empty list
		}

	// --- Constructor to initialize Team with teamName ---
	public Team(string teamName)
		{
		TeamName = teamName;
		Players = new List<Player>(); // Initialize the list of players
		WinPercentage = 0f; // Default value for WinPercentage
		}

	// --- Add Player Method ---
	public void AddPlayer(Player player)
		{
		Players.Add(player); // Add a player to the team
		}

	// --- Remove Player Method ---
	public void RemovePlayer(Player player)
		{
		Players.Remove(player); // Remove a player from the team
		}

	// --- Calculate Total Skill Level (Read-only) ---
	public int TotalSkillLevel
		{
		get
			{
			int totalSkill = 0;
			foreach (var player in Players)
				{
				totalSkill += player.SkillLevel; // Assuming Player has a SkillLevel property
				}
			return totalSkill;
			}
		}

	// --- Set Win Percentage ---
	public void SetWinPercentage(int gamesWon, int gamesPlayed)
		{
		if (gamesPlayed == 0) // Prevent division by zero
			{
			WinPercentage = 0f;
			}
		else
			{
			WinPercentage = (float) gamesWon / gamesPlayed; // Calculate win percentage
			}
		}

	// --- Display Team Information ---
	public void DisplayTeamInfo()
		{
		Console.WriteLine("Team: " + TeamName);
		Console.WriteLine("Total Skill Level: " + TotalSkillLevel);
		Console.WriteLine("Win Percentage: " + WinPercentage * 100 + "%");
		Console.WriteLine("Players: ");
		foreach (Player player in Players)
			{
			Console.WriteLine("- " + player.PlayerName + " (Skill Level: " + player.SkillLevel + ")");
			}
		}
	}