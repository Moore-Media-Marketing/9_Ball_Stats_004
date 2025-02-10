using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class MatchupResultsPanel:MonoBehaviour
	{
	// Singleton instance for MatchupResultsPanel
	public static MatchupResultsPanel Instance { get; private set; }

	[Header("UI Elements")]
	public TMP_Text team1WinProbabilityText;  // Text to display team 1 win probability
	public TMP_Text team2WinProbabilityText;  // Text to display team 2 win probability
	public Transform matchupListContainer;    // Container for matchups list
	public Transform bestMatchupListContainer; // Container for the best matchups
	public GameObject matchupEntryPrefab;     // Prefab for each matchup entry
	public Button backButton;                 // Back button to close the panel

	// Singleton initialization to ensure only one instance of MatchupResultsPanel exists
	private void Awake()
		{
		if (Instance == null)
			{
			Instance = this;
			Debug.Log("MatchupResultsPanel Singleton Initialized");
			}
		else
			{
			Debug.LogWarning("Duplicate MatchupResultsPanel instance detected! Destroying new one.");
			Destroy(gameObject);
			}
		}

	/// <summary>
	/// Displays matchup results based on selected teams.
	/// </summary>
	public void DisplayMatchupResults(List<Player> team1Players, List<Player> team2Players)
		{
		// Ensure valid player lists are received
		if (team1Players == null || team2Players == null)
			{
			Debug.LogError("Null teams received in MatchupResultsPanel!");
			return;
			}

		Debug.Log($"Displaying matchup: Team 1 ({team1Players.Count} players) vs Team 2 ({team2Players.Count} players)");

		// Calculate win probabilities
		float winChanceTeam1 = HandicapSystem.CalculateWinProbability(team1Players, team2Players);
		float winChanceTeam2 = 1 - winChanceTeam1;

		// Update UI with win probabilities
		team1WinProbabilityText.text = $"Win Probability: {winChanceTeam1 * 100:F1}%";
		team2WinProbabilityText.text = $"Win Probability: {winChanceTeam2 * 100:F1}%";

		// Populate matchups and determine the best pairing
		PopulateMatchupList(team1Players, team2Players);

		// Make sure the panel is visible
		gameObject.SetActive(true);
		}

	/// <summary>
	/// Populates the list of matchups and identifies the best matchup.
	/// </summary>
	private void PopulateMatchupList(List<Player> team1Players, List<Player> team2Players)
		{
		// Clear previous entries
		foreach (Transform child in matchupListContainer)
			Destroy(child.gameObject);

		foreach (Transform child in bestMatchupListContainer)
			Destroy(child.gameObject);

		GameObject bestMatchupEntry = null;
		float highestWinChance = 0;

		// Loop through all players in team 1 and team 2 to create matchups
		foreach (var player1 in team1Players)
			{
			foreach (var player2 in team2Players)
				{
				// Calculate win probability for the current matchup
				float matchupOdds = HandicapSystem.CalculateWinProbability(new List<Player> { player1 }, new List<Player> { player2 });

				// Create a matchup entry in the UI
				GameObject matchupEntry = Instantiate(matchupEntryPrefab, matchupListContainer);
				matchupEntry.GetComponent<TMP_Text>().text = $"{player1.PlayerName} vs {player2.PlayerName}: {matchupOdds * 100:F1}%";

				// Identify the best matchup based on win probability
				if (matchupOdds > highestWinChance)
					{
					highestWinChance = matchupOdds;
					bestMatchupEntry = matchupEntry;
					}
				}
			}

		// Highlight the best matchup
		if (bestMatchupEntry != null)
			{
			// Instantiate the best matchup entry in the best matchup list container
			Instantiate(bestMatchupEntry, bestMatchupListContainer);
			Debug.Log($"Best matchup identified: {bestMatchupEntry.GetComponent<TMP_Text>().text}");
			}
		}
	}
