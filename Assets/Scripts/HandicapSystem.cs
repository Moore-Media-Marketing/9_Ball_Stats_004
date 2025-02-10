using System.Collections.Generic;

using UnityEngine;

public static class HandicapSystem
	{
	/// <summary>
	/// Ensures a valid team selection under the 23-Rule.
	/// </summary>
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

	/// <summary>
	/// Calculates win probability based on skill levels.
	/// </summary>
	public static float CalculateWinProbability(List<Player> teamA, List<Player> teamB)
		{
		int skillA = 0, skillB = 0;
		teamA.ForEach(p => skillA += p.Stats.CurrentSeasonSkillLevel);
		teamB.ForEach(p => skillB += p.Stats.CurrentSeasonSkillLevel);
		return skillA / (float) (skillA + skillB);
		}
	}
