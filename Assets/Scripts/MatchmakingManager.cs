using System.Collections.Generic;

using UnityEngine;

public class MatchmakingManager:MonoBehaviour
	{
	// --- MatchmakingManager Variables ---
	[Header("Matchmaking")]
	[Tooltip("List of all teams to compare")]
	public List<Team> teams;

	// --- Compare Teams Method ---
	public void CompareTeams(Team homeTeam, Team awayTeam)
		{
		Debug.Log("Comparing Teams: " + homeTeam.TeamName + " vs " + awayTeam.TeamName);

		// Get optimal lineup for home team
		List<Player> homeOptimalLineup = GetOptimalLineup(homeTeam);
		// Get optimal lineup for away team
		List<Player> awayOptimalLineup = GetOptimalLineup(awayTeam);

		// Show the optimal lineups (or do other comparison logic)
		Debug.Log("Optimal Home Lineup: ");
		foreach (Player player in homeOptimalLineup)
			{
			Debug.Log(player.PlayerName + " (S/L " + player.SkillLevel + ")");
			}

		Debug.Log("Optimal Away Lineup: ");
		foreach (Player player in awayOptimalLineup)
			{
			Debug.Log(player.PlayerName + " (S/L " + player.SkillLevel + ")");
			}

		// Determine winner based on lineups
		Team winner = DetermineWinner(homeOptimalLineup, awayOptimalLineup);
		Debug.Log("Winner: " + winner.TeamName);
		}

	// --- Get Optimal Lineup for a Team ---
	public List<Player> GetOptimalLineup(Team team)
		{
		// Sort players by skill level in descending order
		team.Players.Sort((player1, player2) => player2.SkillLevel.CompareTo(player1.SkillLevel));

		// Select the top 5 players based on their skill level
		List<Player> optimalLineup = new();
		for (int i = 0; i < 5 && i < team.Players.Count; i++)
			{
			optimalLineup.Add(team.Players[i]);
			}

		return optimalLineup;
		}

	// --- Determine Winner Based on Lineups ---
	public Team DetermineWinner(List<Player> homeLineup, List<Player> awayLineup)
		{
		// For simplicity, we'll assume that the team with higher total skill wins.
		int homeSkillTotal = 0;
		int awaySkillTotal = 0;

		foreach (Player player in homeLineup)
			{
			homeSkillTotal += player.SkillLevel;
			}

		foreach (Player player in awayLineup)
			{
			awaySkillTotal += player.SkillLevel;
			}

		if (homeSkillTotal > awaySkillTotal)
			{
			return homeLineup[0].SkillLevel >= awayLineup[0].SkillLevel ? new Team("Home Team") : new Team("Away Team");
			}
		else
			{
			return awayLineup[0].SkillLevel >= homeLineup[0].SkillLevel ? new Team("Away Team") : new Team("Home Team");
			}
		}
	}