using System.Collections.Generic;

using UnityEngine;

public class Match:MonoBehaviour
	{
	[Header("Match Details")]
	[Tooltip("The home team in the match")]
	public Team homeTeam;

	[Tooltip("The away team in the match")]
	public Team awayTeam;

	[Tooltip("The list of home players that will be selected for the match")]
	public List<Player> homeLineup;

	[Tooltip("The list of away players that will be selected for the match")]
	public List<Player> awayLineup;

	[Header("Match Result")]
	[Tooltip("The winner of the match")]
	public Team winner;

	// --- Start Match Method ---
	public void StartMatch()
		{
		// Compare each player 1v1
		int homeWins = 0;
		int awayWins = 0;

		for (int i = 0; i < homeLineup.Count; i++)
			{
			Player homePlayer = homeLineup[i];
			Player awayPlayer = awayLineup[i];

			if (homePlayer.SkillLevel > awayPlayer.SkillLevel)
				{
				homeWins++;
				}
			else if (awayPlayer.SkillLevel > homePlayer.SkillLevel)
				{
				awayWins++;
				}
			}

		// Determine the winner based on number of wins
		if (homeWins > awayWins)
			{
			winner = homeTeam;
			Debug.Log(homeTeam.TeamName + " wins the match!");
			}
		else if (awayWins > homeWins)
			{
			winner = awayTeam;
			Debug.Log(awayTeam.TeamName + " wins the match!");
			}
		else
			{
			winner = null; // Draw scenario
			Debug.Log("The match is a draw!");
			}
		}
	}