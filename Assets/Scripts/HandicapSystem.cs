using System.Collections.Generic;

using UnityEngine;

public static class HandicapSystem
	{
	// Declare weight constants at the class level
	private static readonly float currentSeasonWeight = 0.6f; // 60% weight on current season stats
	private static readonly float lifetimeWeight = 0.4f;      // 40% weight on lifetime stats

	// --- Validating Team Selection for 23-Rule --- //
	public static bool IsValidTeamSelection(List<Player> players)
		{
		int totalSkill = 0;
		int seniorPlayers = 0;

		foreach (var player in players)
			{
			totalSkill += player.Stats.CurrentSeasonSkillLevel;
			if (player.Stats.CurrentSeasonSkillLevel >= 6) seniorPlayers++;
			}

		return totalSkill <= 23 && seniorPlayers <= 2;
		}

	// --- Calculating Win Probability --- //
	public static float CalculateWinProbability(Player player1, Player player2)
		{
		// Calculate the weighted sum of stats for each player
		float player1Skill = currentSeasonWeight * player1.Stats.CurrentSeasonSkillLevel + lifetimeWeight * player1.Stats.LifetimeMatchesWon;
		float player2Skill = currentSeasonWeight * player2.Stats.CurrentSeasonSkillLevel + lifetimeWeight * player2.Stats.LifetimeMatchesWon;

		// Normalize and calculate win probability
		float totalSkill = player1Skill + player2Skill;
		return player1Skill / totalSkill;
		}

	// --- Adjusted Win Probability for Team Matchups --- //
	public static float CalculateAdjustedWinProbability(List<Player> teamA, List<Player> teamB)
		{
		float teamASkill = 0f;
		float teamBSkill = 0f;

		foreach (var player in teamA)
			{
			teamASkill += currentSeasonWeight * player.Stats.CurrentSeasonSkillLevel + lifetimeWeight * player.Stats.LifetimeMatchesWon;
			}

		foreach (var player in teamB)
			{
			teamBSkill += currentSeasonWeight * player.Stats.CurrentSeasonSkillLevel + lifetimeWeight * player.Stats.LifetimeMatchesWon;
			}

		float totalSkill = teamASkill + teamBSkill;
		return teamASkill / totalSkill;
		}

	// --- Finding Optimal Team Based on Stats --- //
	public static List<Player> FindOptimalTeamSelection(List<Player> team)
		{
		// Sort players based on their combined weighted stats (current season + lifetime)
		team.Sort((player1, player2) =>
		{
			float player1Score = currentSeasonWeight * player1.Stats.CurrentSeasonSkillLevel + lifetimeWeight * player1.Stats.LifetimeMatchesWon;
			float player2Score = currentSeasonWeight * player2.Stats.CurrentSeasonSkillLevel + lifetimeWeight * player2.Stats.LifetimeMatchesWon;

			return player2Score.CompareTo(player1Score); // Descending order
		});

		// Select top 5 players (or fewer if the team has less than 5 players)
		return team.GetRange(0, Mathf.Min(5, team.Count));
		}
	}
