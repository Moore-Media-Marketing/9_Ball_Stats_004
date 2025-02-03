using System.Collections.Generic;

using TMPro;

using UnityEngine;

public class MatchupManager:MonoBehaviour
	{
	#region UI References

	[Header("Matchup UI Elements")]
	[Tooltip("Panel that displays matchup results.")]
	public GameObject matchupPanel;  // --- Panel for displaying matchup results ---

	[Tooltip("Template for matchup entries in the scroll view.")]
	public GameObject matchupEntryTemplate;  // --- Template for creating matchup entries ---

	[Tooltip("Parent object for matchup entries.")]
	public Transform matchupListContent;  // --- Parent transform for all matchup entries ---

	[Tooltip("Parent object for best matchup entries.")]
	public Transform bestMatchupListContent;  // --- Parent transform for best matchup entries ---

	[Tooltip("Text element for the best matchup header.")]
	public TMP_Text bestMatchupHeader;  // --- Header text for the best matchup section ---

	[Tooltip("Reference to the MatchupComparisonManager.")]
	public MatchupComparisonManager matchupComparisonManager;  // --- Reference to MatchupComparisonManager ---

	#endregion UI References

	#region Methods

	// --- Compares two selected teams and generates matchup results --- //
	public void CompareTeams()
		{
		// --- Ensure the matchup panel is active --- //
		matchupPanel.SetActive(true);

		// --- Clear previous matchup results --- //
		foreach (Transform child in matchupListContent)
			{
			Destroy(child.gameObject);
			}

		foreach (Transform child in bestMatchupListContent)
			{
			Destroy(child.gameObject);
			}

		// --- Get the selected teams' data from the MatchupComparisonManager --- //
		MatchupComparisonManager.Team selectedTeamA = matchupComparisonManager.selectedTeamA;
		MatchupComparisonManager.Team selectedTeamB = matchupComparisonManager.selectedTeamB;

		// --- Generate matchups (Placeholder for actual logic) --- //
		List<string> matchupResults = new List<string>();

		// Compare players from both teams
		for (int i = 0; i < selectedTeamA.players.Count; i++)
			{
			for (int j = 0; j < selectedTeamB.players.Count; j++)
				{
				var playerA = selectedTeamA.players[i];
				var playerB = selectedTeamB.players[j];

				// Compare players and add the result
				string result = ComparePlayers(playerA, playerB);
				matchupResults.Add(result);

				// --- Instantiate matchup entries and display them in the scroll view --- //
				CreateMatchupEntry(result);
				}
			}

		// --- Display the best matchup --- //
		DisplayBestMatchup(matchupResults);
		}

	// --- Compare players based on selected stats --- //
	private string ComparePlayers(MatchupComparisonManager.Player playerA, MatchupComparisonManager.Player playerB)
		{
		// Calculate player scores (for example purposes, using a simple comparison)
		float playerAScore = CalculatePlayerScore(playerA);
		float playerBScore = CalculatePlayerScore(playerB);

		if (playerAScore > playerBScore)
			{
			return $"{playerA.name} wins vs {playerB.name}";
			}
		else if (playerBScore > playerAScore)
			{
			return $"{playerB.name} wins vs {playerA.name}";
			}
		else
			{
			return $"{playerA.name} vs {playerB.name} is a tie";
			}
		}

	// --- Calculate a score for a player based on stats --- //
	private float CalculatePlayerScore(MatchupComparisonManager.Player player)
		{
		// Example: Calculate score based on lifetime and current season data (simplified)
		float lifetimeScore = (player.lifetimeGamesWon / player.lifetimeGamesPlayed) * 0.3f +
							  player.lifetimeBreakAndRun * 0.2f +
							  player.lifetimeMiniSlams * 0.2f +
							  player.lifetimeShutouts * 0.3f;

		float currentSeasonScore = (player.currentSeasonGamesWon / player.currentSeasonGamesPlayed) * 0.4f +
								   player.currentSeasonPPM * 0.3f +
								   player.currentSeasonSkillLevel * 0.3f;

		return lifetimeScore + currentSeasonScore;
		}

	// --- Create a UI entry for the matchup result --- //
	private void CreateMatchupEntry(string result)
		{
		// --- Instantiate a new matchup entry and set the result text --- //
		GameObject newEntry = Instantiate(matchupEntryTemplate, matchupListContent);
		TMP_Text entryText = newEntry.GetComponentInChildren<TMP_Text>();
		entryText.text = result;
		}

	// --- Display the best matchup based on scores --- //
	private void DisplayBestMatchup(List<string> matchupResults)
		{
		// Example logic: Display the first matchup as the "best" matchup
		if (matchupResults.Count > 0)
			{
			bestMatchupHeader.text = "Best Matchup";
			CreateBestMatchupEntry(matchupResults[0]);
			}
		}

	// --- Create the best matchup entry --- //
	private void CreateBestMatchupEntry(string bestMatchup)
		{
		// --- Instantiate and display the best matchup result --- //
		GameObject newBestEntry = Instantiate(matchupEntryTemplate, bestMatchupListContent);
		TMP_Text entryText = newBestEntry.GetComponentInChildren<TMP_Text>();
		entryText.text = bestMatchup;
		}

	// --- Closes the matchup results panel --- //
	public void CloseMatchupPanel()
		{
		matchupPanel.SetActive(false);
		}

	#endregion Methods
	}
