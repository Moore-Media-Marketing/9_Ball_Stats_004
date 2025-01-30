using System.Collections.Generic;

using UnityEngine;

public class TeamManager:MonoBehaviour
	{
	// --- TeamManager Variables ---
	[Header("Team Management")]
	[Tooltip("List of all teams in the game")]
	public List<Team> allTeams; // List to store all the teams

	// --- Add Team Method ---
	public void AddTeam(string teamName)
		{
		// Create a new team and add it to the allTeams list
		Team newTeam = new(teamName);
		allTeams.Add(newTeam);
		Debug.Log("Team added: " + teamName);
		}

	// --- Remove Team Method ---
	public void RemoveTeam(string teamName)
		{
		// Find the team by name and remove it from the list
		Team teamToRemove = allTeams.Find(team => team.TeamName == teamName);

		if (teamToRemove != null)
			{
			allTeams.Remove(teamToRemove);
			Debug.Log("Team removed: " + teamName);
			}
		else
			{
			Debug.LogWarning("Team not found: " + teamName);
			}
		}

	// --- Add Player to Team Method ---
	public void AddPlayerToTeam(string teamName, Player player)
		{
		// Find the team by name and add the player
		Team team = allTeams.Find(t => t.TeamName == teamName);

		if (team != null)
			{
			team.AddPlayer(player);
			Debug.Log("Player " + player.PlayerName + " added to team " + teamName);
			}
		else
			{
			Debug.LogWarning("Team not found: " + teamName);
			}
		}

	// --- Remove Player from Team Method ---
	public void RemovePlayerFromTeam(string teamName, Player player)
		{
		// Find the team by name and remove the player
		Team team = allTeams.Find(t => t.TeamName == teamName);

		if (team != null)
			{
			team.RemovePlayer(player);
			Debug.Log("Player " + player.PlayerName + " removed from team " + teamName);
			}
		else
			{
			Debug.LogWarning("Team not found: " + teamName);
			}
		}

	// --- Display All Teams Info Method ---
	public void DisplayAllTeamsInfo()
		{
		// Display all teams and their players
		foreach (Team team in allTeams)
			{
			team.DisplayTeamInfo();
			}
		}
	}