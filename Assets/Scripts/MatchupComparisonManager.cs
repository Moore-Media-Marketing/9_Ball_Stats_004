using System.Collections.Generic;

using UnityEngine;

public class MatchupComparisonManager:MonoBehaviour
	{
	// --- Region: Player and Team Data --- //
	// Updated Player structure to match the data fields used in the SampleDataGenerator
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
		public float currentSeasonPointsAwarded;
		public int pointsRequiredToWin;
		}

	public class Team
		{
		public string teamName;
		public List<Player> players;
		}

	// --- End Region --- //

	// --- Region: UI Elements --- //
	public MatchupResultsPanel matchupResultsPanel;  // Reference to the MatchupResultsPanel for displaying results

	// --- End Region --- //

	// --- Region: Team Selection --- //
	// These represent the selected teams A and B (we'll get this data from the UI)
	public Team selectedTeamA;

	public Team selectedTeamB;

	// For demonstration, the following methods return dummy data.
	// Replace them with actual UIManager data fetching.

	private Team GetSelectedTeamA()
		{
		return new Team
			{
			teamName = "Team A",
			players = new List<Player>
			{
				new() { name = "Player A1", lifetimeGamesWon = 50, lifetimeGamesPlayed = 100, lifetimeBreakAndRun = 1.5f, lifetimeMiniSlams = 10, lifetimeShutouts = 5, currentSeasonGamesWon = 10, currentSeasonGamesPlayed = 20, currentSeasonPPM = 2.3f, currentSeasonSkillLevel = 4.2f, pointsRequiredToWin = 7 },
				new() { name = "Player A2", lifetimeGamesWon = 40, lifetimeGamesPlayed = 90, lifetimeBreakAndRun = 1.3f, lifetimeMiniSlams = 8, lifetimeShutouts = 6, currentSeasonGamesWon = 8, currentSeasonGamesPlayed = 18, currentSeasonPPM = 1.9f, currentSeasonSkillLevel = 4.0f, pointsRequiredToWin = 7 }
			}
			};
		}

	private Team GetSelectedTeamB()
		{
		return new Team
			{
			teamName = "Team B",
			players = new List<Player>
			{
				new() { name = "Player B1", lifetimeGamesWon = 60, lifetimeGamesPlayed = 120, lifetimeBreakAndRun = 1.8f, lifetimeMiniSlams = 15, lifetimeShutouts = 7, currentSeasonGamesWon = 12, currentSeasonGamesPlayed = 22, currentSeasonPPM = 2.5f, currentSeasonSkillLevel = 4.5f, pointsRequiredToWin = 7 },
				new() { name = "Player B2", lifetimeGamesWon = 55, lifetimeGamesPlayed = 110, lifetimeBreakAndRun = 1.6f, lifetimeMiniSlams = 14, lifetimeShutouts = 9, currentSeasonGamesWon = 11, currentSeasonGamesPlayed = 20, currentSeasonPPM = 2.1f, currentSeasonSkillLevel = 4.3f, pointsRequiredToWin = 7 }
			}
			};
		}

	// --- End Region --- //

	// --- Region: Compare Matchups --- //
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

		// Display results in the UI
		DisplayMatchupResults(matchupResults);
		}

	// --- End Region --- //

	// --- Region: Player Comparison --- //
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

		// Add PointsRequiredToWin to the calculation if needed (you can adjust the weight here)
		float pointsAdjustment = player.pointsRequiredToWin * 0.1f;

		return lifetimeScore + currentSeasonScore + pointsAdjustment;
		}

	// --- End Region --- //

	// --- Region: UI Update --- //
	private void DisplayMatchupResults(List<string> results)
		{
		// Update the MatchupResultsPanel UI with the results
		matchupResultsPanel.UpdateMatchupResults("Matchup Results", selectedTeamA.teamName, selectedTeamB.teamName, string.Join("\n", results));
		}

	// --- End Region --- //
	}