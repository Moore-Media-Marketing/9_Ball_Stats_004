using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

[Serializable]
public class Team
	{
	// --- Region: Public Variables --- //
	public int id;                         // --- Unique ID for the team ---
	public string name;                    // --- Name of the team ---
	public List<Player> players;           // --- List of players in the team ---
	public List<int> playerSkillLevels;    // --- List to store player skill levels ---
										   // --- End Region: Public Variables --- //

	// --- Region: Static Team List --- //
	public static List<Team> teamList = new(); // --- Static list to hold all team objects ---
											   // --- End Region: Static Team List --- //

	// --- Region: Constructor --- //
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

		teamList.Add(this); // --- Add the new team to the static list ---
		Debug.Log($"Team created: {name} with ID: {id}");  // --- Debug log for team creation ---
		}

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

		teamList.Add(this); // --- Add the new team to the static list ---
		Debug.Log($"Team created: {name} with ID: {id}");  // --- Debug log for team creation ---
		}
	// --- End Region: Constructor --- //

	// --- Region: Methods --- //
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

	// --- Region: Static Utility Functions --- //
	public static Team GetTeamById(int teamId)
		{
		return teamList.FirstOrDefault(t => t.id == teamId); // --- Find a team by ID ---
		}

	public static List<Team> GetAllTeams()
		{
		return teamList; // --- Return the list of all teams ---
		}

	public static void RemoveTeamById(int teamId)
		{
		Team teamToRemove = teamList.FirstOrDefault(t => t.id == teamId);
		if (teamToRemove != null)
			{
			teamList.Remove(teamToRemove); // --- Remove team from the list ---
			Debug.Log("Team removed: " + teamToRemove.name);
			}
		else
			{
			Debug.LogError("Team with ID: " + teamId + " not found.");
			}
		}
	// --- End Region: Static Utility Functions --- //

	// --- Region: JSON Serialization Functions --- //
	public static void SaveTeamsToJson(string filePath)
		{
		string json = JsonUtility.ToJson(new TeamListWrapper { teams = teamList }, true);
		System.IO.File.WriteAllText(filePath, json);
		Debug.Log("Teams saved to JSON.");
		}

	public static void LoadTeamsFromJson(string filePath)
		{
		if (System.IO.File.Exists(filePath))
			{
			string json = System.IO.File.ReadAllText(filePath);
			TeamListWrapper wrapper = JsonUtility.FromJson<TeamListWrapper>(json);
			teamList = wrapper.teams;
			Debug.Log("Teams loaded from JSON.");
			}
		else
			{
			Debug.LogError("File not found: " + filePath);
			}
		}
	// --- End Region: JSON Serialization Functions --- //

	// --- Region: ID Generation --- //
	private static int GenerateNewId()
		{
		int maxId = 0;

		if (teamList.Count > 0)
			{
			maxId = teamList.Max(t => t.id);
			}

		return maxId + 1;  // Return the next available ID
		}
	// --- End Region: ID Generation --- //

	// --- Region: Wrapper Class for JSON Serialization --- //
	[Serializable]
	private class TeamListWrapper
		{
		public List<Team> teams; // --- List of teams for JSON deserialization ---
		}
	// --- End Region: Wrapper Class for JSON Serialization --- //
	}
