// --- Region: Team Class Definition --- //
using System.Collections.Generic;
using System;

using UnityEngine;

[Serializable]
public class Team
	{
	// --- Region: Public Variables --- //
	public int id;                         // --- Unique ID for the team ---
	public string name;                    // --- Name of the team ---
	public List<Player> players;           // --- List of players in the team ---
	public List<int> playerSkillLevels;    // --- List to store player skill levels ---

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

	// --- Region: Methods --- //
	// --- Method to add a player to the team --- 
	public void AddPlayer(Player player)
		{
		if (player != null)                // --- Check if the player is not null ---
			{
			players.Add(player);          // --- Add player to the team ---
			playerSkillLevels.Add(player.skillLevel); // --- Add player's skill level ---
			Debug.Log($"Player {player.name} added to team {name}");  // --- Debug log for player addition ---
			}
		else
			{
			Debug.LogWarning("Attempted to add a null player to the team");  // --- Debug warning for null player ---
			}
		}

	// --- Region: Error Handling --- //
	// --- Check if the team has players --- 
	public bool HasPlayers()
		{
		if (players.Count == 0)           // --- Check if there are no players in the list ---
			{
			Debug.LogWarning($"Team {name} has no players."); // --- Debug warning for empty team ---
			return false;                  // --- Return false if no players exist ---
			}
		return true;                       // --- Return true if players exist ---
		}

	// --- Region: Additional Functions --- //
	// --- Additional custom functions for the team can go here ---

	// --- Helper function to generate a new unique ID for the team ---
	private int GenerateNewId()
		{
		// This is a simple example; you can implement your own logic for generating IDs
		return new System.Random().Next(1, 10000); // Random ID between 1 and 10,000
		}
	}
// --- End Region: Team Class Definition --- //
