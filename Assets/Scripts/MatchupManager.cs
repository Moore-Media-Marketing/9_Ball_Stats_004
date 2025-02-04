using UnityEngine;

// --- Region: Matchup Manager --- //
public class MatchupManager:MonoBehaviour
	{
	// --- Comment: Method to get the combined skill level of a team --- //
	public int GetTeamSkillLevel(Team team)
		{
		int skillLevel = 0;
		foreach (Player player in team.Players)  // Use 'Players' instead of 'players' (ensure proper casing)
			{
			skillLevel += player.SkillLevel; // Use 'SkillLevel' instead of 'skillLevel'
			}
		return skillLevel;
		}

	// --- Comment: Method to compare two teams --- //
	public void CompareTeams(Team team1, Team team2)
		{
		int team1SkillLevel = GetTeamSkillLevel(team1); // Get combined skill level for team 1
		int team2SkillLevel = GetTeamSkillLevel(team2); // Get combined skill level for team 2

		Debug.Log($"Team 1 Skill Level: {team1SkillLevel}, Team 2 Skill Level: {team2SkillLevel}");

		// Logic to compare teams and decide the winner can be added here
		}
	}
