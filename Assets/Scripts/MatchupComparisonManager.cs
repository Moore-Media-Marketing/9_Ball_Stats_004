using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MatchupComparisonManager:MonoBehaviour
	{
	// Example structures for player data (simplified for illustration)
	public class Player
		{
		public string name;
		public float lifetimeGamesWon;
		public float lifetimeGamesPlayed;
		public float lifetimeDefensiveShotAvg;
		public float lifetimeBreakAndRun;
		public float lifetimeMiniSlams;
		public float lifetimeShutouts;

		public float currentSeasonGamesWon;
		public float currentSeasonGamesPlayed;
		public float currentSeasonTotalPoints;
		public float currentSeasonPPM;
		public float currentSeasonSkillLevel;
		// More stats can be added as needed
		}

	public class Team
		{
		public string teamName;
		public List<Player> players;
		}

	// Example teams (to be populated with actual data)
	private List<Team> teams = new();

	// These represent the selected teams A and B (we'll get this data from the UI)
	public Team selectedTeamA;
	public Team selectedTeamB;

	// Update these methods to get selected teams and players from UI (assumed to be set via UIManager)
	private Team GetSelectedTeamA()
		{
		// For demonstration, return dummy data
		// Replace with actual logic to get the selected team
		return new Team
			{
			teamName = "Team A",
			players = new List<Player>
			{
				new() { name = "Player A1", lifetimeGamesWon = 50, lifetimeGamesPlayed = 100, lifetimeBreakAndRun = 1.5f, lifetimeMiniSlams = 10, lifetimeShutouts = 5, currentSeasonGamesWon = 10, currentSeasonGamesPlayed = 20, currentSeasonPPM = 2.3f, currentSeasonSkillLevel = 4.2f },
				new() { name = "Player A2", lifetimeGamesWon = 40, lifetimeGamesPlayed = 90, lifetimeBreakAndRun = 1.3f, lifetimeMiniSlams = 8, lifetimeShutouts = 6, currentSeasonGamesWon = 8, currentSeasonGamesPlayed = 18, currentSeasonPPM = 1.9f, currentSeasonSkillLevel = 4.0f }
			}
			};
		}

	private Team GetSelectedTeamB()
		{
		// For demonstration, return dummy data
		// Replace with actual logic to get the selected team
		return new Team
			{
			teamName = "Team B",
			players = new List<Player>
			{
				new() { name = "Player B1", lifetimeGamesWon = 60, lifetimeGamesPlayed = 120, lifetimeBreakAndRun = 1.8f, lifetimeMiniSlams = 15, lifetimeShutouts = 7, currentSeasonGamesWon = 12, currentSeasonGamesPlayed = 22, currentSeasonPPM = 2.5f, currentSeasonSkillLevel = 4.5f },
				new() { name = "Player B2", lifetimeGamesWon = 55, lifetimeGamesPlayed = 110, lifetimeBreakAndRun = 1.6f, lifetimeMiniSlams = 14, lifetimeShutouts = 9, currentSeasonGamesWon = 11, currentSeasonGamesPlayed = 20, currentSeasonPPM = 2.1f, currentSeasonSkillLevel = 4.3f }
			}
			};
		}

	// Compare matchups method
	public void CompareMatchups()
		{
		selectedTeamA = GetSelectedTeamA();
		selectedTeamB = GetSelectedTeamB();

		List<string> matchupResults = new();

		// Compare each player from Team A with each player from Team B
		for (int i = 0; i < selectedTeamA.players.Count; i++)
			{
			for (int j = 0; j < selectedTeamB.players.Count; j++)
				{
				Player playerA = selectedTeamA.players[i];
				Player playerB = selectedTeamB.players[j];

				// Compare players based on lifetime and current season stats
				string result = ComparePlayers(playerA, playerB);
				matchupResults.Add(result);
				}
			}

		// Display results
		DisplayMatchupResults(matchupResults);
		}

	// Comparing players based on lifetime and current season stats
	private string ComparePlayers(Player playerA, Player playerB)
		{
		// Calculate comparison scores based on lifetime and current season stats
		float playerAScore = CalculatePlayerScore(playerA);
		float playerBScore = CalculatePlayerScore(playerB);

		if (playerAScore > playerBScore)
			return $"{playerA.name} wins vs {playerB.name}";
		else if (playerBScore > playerAScore)
			return $"{playerB.name} wins vs {playerA.name}";
		else
			return $"{playerA.name} vs {playerB.name} is a tie";
		}

	// Calculate a score for each player based on weighted stats (lifetime and current season)
	private float CalculatePlayerScore(Player player)
		{
		// Example weighted score formula (adjust weights as needed)
		float lifetimeScore = (player.lifetimeGamesWon / player.lifetimeGamesPlayed) * 0.3f +
							  player.lifetimeBreakAndRun * 0.2f +
							  player.lifetimeMiniSlams * 0.2f +
							  player.lifetimeShutouts * 0.3f;

		float currentSeasonScore = (player.currentSeasonGamesWon / player.currentSeasonGamesPlayed) * 0.4f +
								   player.currentSeasonPPM * 0.3f +
								   player.currentSeasonSkillLevel * 0.3f;

		return lifetimeScore + currentSeasonScore;
		}

	// Display the results of the comparison (e.g., output to UI or console)
	private void DisplayMatchupResults(List<string> results)
		{
		foreach (var result in results)
			{
			Debug.Log(result);  // For now, output results to console
			}

		// Optionally, update the UI with the results
		// Example: resultsPanel.UpdateResults(results);
		}
	}
