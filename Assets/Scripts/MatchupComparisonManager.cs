using UnityEngine;

// --- Region: Matchup Comparison Manager --- //
public class MatchupComparisonManager:MonoBehaviour
	{
	// --- Comment: Method to compare matchups between two teams --- //
	public void CompareMatchups(Team team1, Team team2)
		{
		int team1SkillLevel = 0;
		int team2SkillLevel = 0;

		// Calculate total skill level for team 1
		foreach (Player player in team1.Players)  // Use 'Players' instead of 'players' (ensure proper casing)
			{
			team1SkillLevel += player.SkillLevel; // Use 'SkillLevel' instead of 'skillLevel'
			}

		// Calculate total skill level for team 2
		foreach (Player player in team2.Players)  // Use 'Players' instead of 'players'
			{
			team2SkillLevel += player.SkillLevel; // Use 'SkillLevel' instead of 'skillLevel'
			}

		Debug.Log($"Team 1 Skill Level: {team1SkillLevel}, Team 2 Skill Level: {team2SkillLevel}");

		// Additional logic for Equalizer® Handicap System can be implemented here.
		}
	}
