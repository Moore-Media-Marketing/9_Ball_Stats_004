using System.Collections.Generic;

using UnityEngine;

public class PlayersAndTeamsManager:MonoBehaviour
	{
	// --- Region: Singleton Instance ---
	// This ensures that only one instance of PlayersAndTeamsManager exists.
	public static PlayersAndTeamsManager Instance;

	// --- Region: Data Storage ---
	// Lists to store all teams and players
	public List<Team> teams = new();

	public List<Player> players = new();

	// --- Region: Initialization ---
	// Called when the script is first run to set the singleton instance.
	private void Awake()
		{
		// Ensure that only one instance of PlayersAndTeamsManager exists
		if (Instance == null)
			{
			Instance = this;
			}
		else
			{
			Destroy(gameObject);
			}
		}

	// --- Region: Team Management Methods ---

	/// <summary>
	/// Get all teams in the system.
	/// </summary>
	public List<Team> GetAllTeams()
		{
		return teams;
		}

	/// <summary>
	/// Get all players by team ID.
	/// </summary>
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

	/// <summary>
	/// Get the team name by team ID.
	/// </summary>
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

	/// <summary>
	/// Add a new team to the system.
	/// </summary>
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

	/// <summary>
	/// Modify the name of an existing team by team ID.
	/// </summary>
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

	/// <summary>
	/// Delete a team and reassign players to "Unassigned".
	/// </summary>
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

	// --- Region: Player Management Methods ---

	/// <summary>
	/// Update player's lifetime data.
	/// </summary>
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

	// --- Region: Additional Functions ---
	// You can add any additional helper methods here if needed.
	}