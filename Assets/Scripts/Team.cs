using System.Collections.Generic;
using System;

using UnityEngine;
using System.Linq;

[Serializable]
public class Team
	{
	// --- Region: Public Variables --- //
	public int id;                         // --- Unique ID for the team ---
	public string name;                    // --- Name of the team ---
	public List<Player> players;           // --- List of players in the team ---
	public List<int> playerSkillLevels;    // --- List to store player skill levels ---
										   // --- End Region: Public Variables --- //

	// --- Region: Constructor --- //
	// --- Constructor to initialize a new team with a name ---
	public Team(string name)
		{
		if (string.IsNullOrEmpty(name))    // --- Check if the name is null or empty ---
			{
			Debug.LogError("Team name cannot be null or empty.");
			return;
			}

		this.id = GenerateNewId();         // --- Automatically generate a new team ID ---
		this.name = name;                  // --- Set the team name ---
		players = new List<Player>();      // --- Initialize the players list ---
		playerSkillLevels = new List<int>(); // --- Initialize the player skill levels list ---

		Debug.Log($"Team created: {name} with ID: {id}");  // --- Debug log for team creation ---
		}

	// --- Constructor to initialize a new team with both an ID and a name ---
	public Team(int id, string name)
		{
		if (string.IsNullOrEmpty(name))    // --- Check if the name is null or empty ---
			{
			Debug.LogError("Team name cannot be null or empty.");
			return;
			}

		this.id = id;                      // --- Set the team ID ---
		this.name = name;                  // --- Set the team name ---
		players = new List<Player>();      // --- Initialize the players list ---
		playerSkillLevels = new List<int>(); // --- Initialize the player skill levels list ---

		Debug.Log($"Team created: {name} with ID: {id}");  // --- Debug log for team creation ---
		}

	// --- End Region: Constructor --- //

	// --- Region: Methods --- //
	// --- Method to add a player to the team --- 
	public void AddPlayer(Player player)
		{
		if (player != null)                // --- Check if the player is not null ---
			{
			players.Add(player);          // --- Add the player to the team's list ---
			playerSkillLevels.Add(player.skillLevel);  // --- Add player's skill level ---
			Debug.Log("Player added: " + player.name);
			}
		else
			{
			Debug.LogError("Cannot add a null player to the team.");
			}
		}

	// --- End Region: Methods --- //

	// --- Region: ID Generation --- //

	// --- Method to generate a new unique ID for the team ---
	private static int GenerateNewId()
		{
		// Try to find the highest existing team ID and increment by 1
		int maxId = 0;

		// Check all existing teams (if any) and find the max ID
		List<Team> allTeams = DatabaseManager.Instance.GetAllTeams();
		if (allTeams.Count > 0)
			{
			maxId = allTeams.Max(t => t.id);
			}

		return maxId + 1;  // Return the next available ID
		}

	// --- End Region: ID Generation --- //
	}
