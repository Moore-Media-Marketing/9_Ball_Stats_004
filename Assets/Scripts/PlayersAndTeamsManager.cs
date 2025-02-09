using UnityEngine;
using System.Collections.Generic;

public class PlayersAndTeamsManager:MonoBehaviour
	{
	public static PlayersAndTeamsManager Instance;
	public List<Team> teams = new();
	public List<Player> players = new();

	private void Awake()
		{
		if (Instance == null)
			{
			Instance = this;
			}
		else
			{
			Destroy(gameObject);
			}
		}

	// Get all teams
	public List<Team> GetAllTeams()
		{
		return teams;
		}

	// Get players by team ID
	public List<Player> GetPlayersByTeamId(int teamId)
		{
		List<Player> teamPlayers = new();
		foreach (Player player in players)
			{
			if (player.TeamId == teamId)
				{
				teamPlayers.Add(player);
				}
			}
		return teamPlayers;
		}

	// Get team name by team ID
	public string GetTeamNameById(int teamId)
		{
		Team team = teams.Find(t => t.TeamId == teamId);
		if (team != null)
			{
			return team.TeamName;
			}
		else
			{
			return "Unknown Team";
			}
		}

	// Add a new team
	public void AddTeam(string teamName)
		{
		// Check if the team already exists
		if (teams.Exists(t => t.TeamName == teamName))
			{
			Debug.LogWarning($"Team '{teamName}' already exists.");
			return;
			}

		// Create and add a new team
		int newTeamId = teams.Count + 1;
		teams.Add(new Team(newTeamId, teamName));
		Debug.Log($"Added new team: {teamName}");
		}

	// Modify an existing team's name
	public void ModifyTeam(int teamId, string newTeamName)
		{
		Team teamToModify = teams.Find(t => t.TeamId == teamId);
		if (teamToModify != null)
			{
			teamToModify.TeamName = newTeamName;
			Debug.Log($"Modified team ID {teamId} to new name: {newTeamName}");
			}
		else
			{
			Debug.LogWarning($"Team with ID {teamId} not found.");
			}
		}

	// Delete a team and reassign players to "Unassigned"
	public void DeleteTeam(int teamId)
		{
		Team teamToDelete = teams.Find(t => t.TeamId == teamId);
		if (teamToDelete != null)
			{
			// Remove players assigned to this team
			foreach (Player player in players)
				{
				if (player.TeamId == teamId)
					{
					player.TeamId = -1; // Set to "Unassigned" team ID
					}
				}

			// Remove the team from the list
			teams.Remove(teamToDelete);
			Debug.Log($"Deleted team: {teamToDelete.TeamName}");
			}
		else
			{
			Debug.LogWarning($"Team with ID {teamId} not found.");
			}
		}

	// Update player's lifetime data
	public void UpdatePlayerLifetimeData(int playerId, int gamesWon, int gamesPlayed, float defensiveShotAvg,
										 int matchesPlayedInLast2Years, int breakAndRun, int nineOnTheSnap,
										 int miniSlams, int shutouts)
		{
		Player playerToUpdate = players.Find(p => p.PlayerId == playerId);
		if (playerToUpdate != null)
			{
			playerToUpdate.Stats.LifetimeGamesWon = gamesWon;
			playerToUpdate.Stats.LifetimeGamesPlayed = gamesPlayed;
			playerToUpdate.Stats.LifetimeDefensiveShotAverage = defensiveShotAvg;
			playerToUpdate.Stats.LifetimeMatchesPlayed = matchesPlayedInLast2Years;
			playerToUpdate.Stats.LifetimeBreakAndRun = breakAndRun;
			playerToUpdate.Stats.LifetimeNineOnTheSnap = nineOnTheSnap;
			playerToUpdate.Stats.LifetimeMiniSlams = miniSlams;
			playerToUpdate.Stats.LifetimeShutouts = shutouts;

			Debug.Log($"Player {playerToUpdate.PlayerName}'s lifetime data updated.");
			}
		else
			{
			Debug.LogWarning($"Player with ID {playerId} not found.");
			}
		}
	}
