using System.Collections.Generic;

using UnityEngine;

namespace NickWasHere
	{
	public class DataManager:MonoBehaviour
		{
		// --- Singleton Instance --- //
		public static DataManager Instance { get; private set; }

		// --- Lists to store teams and players --- //
		private List<Team> teams = new();

		private List<Player> players = new();

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
				return;
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

		// --- Get all player names --- //
		public List<string> GetPlayerNames()
			{
			List<string> playerNames = new();
			foreach (var player in players)
				{
				playerNames.Add(player.name);
				}
			return playerNames;
			}

		// --- Get home team names --- //
		public List<string> GetHomeTeamNames()
			{
			List<string> homeTeamNames = new();
			foreach (var team in teams)
				{
				homeTeamNames.Add(team.teamName);
				}
			return homeTeamNames;
			}

		// --- Get away team names --- //
		public List<string> GetAwayTeamNames()
			{
			List<string> awayTeamNames = new();
			foreach (var team in teams)
				{
				awayTeamNames.Add(team.teamName);
				}
			return awayTeamNames;
			}

		// --- Get skill level options --- //
		public List<string> GetSkillLevelOptions()
			{
			List<string> skillLevels = new();
			for (int i = 1; i <= 9; i++)
				{
				skillLevels.Add(i.ToString());
				}
			return skillLevels;
			}

		// --- Add a new team --- //
		public void AddTeam(string teamName)
			{
			if (DoesTeamExist(teamName))
				{
				Debug.LogError($"Team '{teamName}' already exists.");
				return;
				}

			teams.Add(new Team(teamName));
			SaveTeamsToPlayerPrefs();
			Debug.Log($"Team '{teamName}' added successfully.");
			}

		// --- Save teams to PlayerPrefs --- //
		public void SaveTeamsToPlayerPrefs()
			{
			PlayerPrefs.DeleteKey("Teams");
			for (int i = 0; i < teams.Count; i++)
				{
				PlayerPrefs.SetString($"Team_{i}_Name", teams[i].teamName);
				}
			PlayerPrefs.SetInt("TotalTeams", teams.Count);
			PlayerPrefs.Save();
			}

		// --- Load teams from PlayerPrefs --- //
		public void LoadTeamsFromPlayerPrefs()
			{
			teams.Clear();
			int totalTeams = PlayerPrefs.GetInt("TotalTeams", 0);
			for (int i = 0; i < totalTeams; i++)
				{
				string teamName = PlayerPrefs.GetString($"Team_{i}_Name", "");
				if (!string.IsNullOrEmpty(teamName))
					{
					teams.Add(new Team(teamName));
					}
				}
			Debug.Log("Teams loaded from PlayerPrefs.");
			}

		// --- Check if a team exists --- //
		public bool DoesTeamExist(string teamName)
			{
			return teams.Exists(t => t.teamName == teamName);
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
			team.teamName = newName;
			Debug.Log("Team updated successfully.");
			}

		// --- Remove a team --- //
		public void RemoveTeam(string teamName)
			{
			if (teams.RemoveAll(t => t.teamName == teamName) > 0)
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
			if (players.Exists(p => p.name == playerName))
				{
				Debug.LogError($"Player '{playerName}' already exists.");
				return;
				}
			players.Add(new Player(playerName, skillLevel, gamesPlayed, gamesWon));
			Debug.Log($"Player '{playerName}' added successfully.");
			}

		// --- Remove an existing player --- //
		public void RemovePlayer(string playerName)
			{
			if (players.RemoveAll(p => p.name == playerName) > 0)
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