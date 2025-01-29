using System.Collections.Generic;

using UnityEngine;

namespace NickWasHere
	{
	public class DataManager:MonoBehaviour
		{
		public static DataManager Instance { get; private set; }

		private List<Team> teams = new();  // --- List to store teams --- //
		private List<Player> players = new();  // --- List to store players --- //

		private void Awake()
			{
			if (Instance == null)
				{
				Instance = this;
				DontDestroyOnLoad(gameObject);
				}
			else
				{
				Destroy(gameObject);
				}
			}

		// --- Get all team names --- //
		public List<string> GetTeamNames()
			{
			List<string> teamNames = new();
			foreach (var team in teams)
				{
				teamNames.Add(team.teamName);
				}
			return teamNames;
			}

		// --- Add a new team --- //
		public void AddTeam(string teamName)
			{
			// Check if team already exists
			if (teams.Exists(t => t.teamName == teamName))
				{
				Debug.LogError($"Team '{teamName}' already exists.");
				return;
				}

			// Create new team and add it to the list
			Team newTeam = new(teamName);
			teams.Add(newTeam);

			// Save to PlayerPrefs
			SaveTeamsToPlayerPrefs();

			Debug.Log($"Team '{teamName}' added successfully.");
			}

		public void SaveTeamsToPlayerPrefs()
			{
			// Clear any existing teams in PlayerPrefs before saving new data
			PlayerPrefs.DeleteKey("Teams");

			// Save each team's name to PlayerPrefs
			for (int i = 0; i < teams.Count; i++)
				{
				PlayerPrefs.SetString($"Team_{i}_Name", teams[i].teamName);
				}

			// Optionally save the total count of teams
			PlayerPrefs.SetInt("TotalTeams", teams.Count);

			PlayerPrefs.Save();
			}

		public void LoadTeamsFromPlayerPrefs()
			{
			// Retrieve the total number of teams saved
			int totalTeams = PlayerPrefs.GetInt("TotalTeams", 0);

			// Load each team's name from PlayerPrefs
			teams.Clear();
			for (int i = 0; i < totalTeams; i++)
				{
				string teamName = PlayerPrefs.GetString($"Team_{i}_Name", "");
				if (!string.IsNullOrEmpty(teamName))
					{
					Team loadedTeam = new(teamName);
					teams.Add(loadedTeam);
					}
				}

			Debug.Log("Teams loaded from PlayerPrefs.");
			}




		// --- Update team name --- //
		public void UpdateTeamName(string oldName, string newName)
			{
			Debug.Log($"Updating Team: {oldName} to {newName}");

			Team team = teams.Find(t => t.teamName == oldName);
			if (team == null)
				{
				Debug.LogError("Team not found: " + oldName);
				return;
				}

			team.teamName = newName;  // --- Update team name --- //
			Debug.Log("Team updated successfully.");
			}

		// --- Remove a team --- //
		public void RemoveTeam(string teamName)
			{
			int removedCount = teams.RemoveAll(t => t.teamName == teamName);

			if (removedCount > 0)
				{
				Debug.Log($"Team '{teamName}' removed successfully.");
				}
			else
				{
				Debug.LogError($"Team '{teamName}' not found.");
				}
			}

		// --- Add a new player --- //
		public void AddPlayer(string playerName, int skillLevel, int gamesPlayed, int gamesWon)
			{
			if (players.Exists(p => p.name == playerName))  // Changed to 'name' instead of 'playerName'
				{
				Debug.LogError($"Player '{playerName}' already exists.");
				return;
				}

			Player newPlayer = new(playerName, skillLevel, gamesPlayed, gamesWon);  // Changed to include gamesPlayed and gamesWon
			players.Add(newPlayer);

			Debug.Log($"Player '{playerName}' added successfully.");
			}

		// --- Remove an existing player --- //
		public void RemovePlayer(string playerName)
			{
			int removedCount = players.RemoveAll(p => p.name == playerName);  // Changed to 'name' instead of 'playerName'

			if (removedCount > 0)
				{
				Debug.Log($"Player '{playerName}' removed successfully.");
				}
			else
				{
				Debug.LogError($"Player '{playerName}' not found.");
				}
			}
		}
	}