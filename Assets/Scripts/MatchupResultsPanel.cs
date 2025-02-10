using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class MatchupResultsPanel:MonoBehaviour
	{
	public static MatchupResultsPanel Instance { get; private set; }

	[Header("UI Elements")]
	public TMP_Text team1WinProbabilityText;
	public TMP_Text team2WinProbabilityText;
	public Transform matchupListContainer;
	public Transform bestMatchupListContainer;
	public GameObject matchupEntryPrefab;
	public Button backButton;

	private void Awake()
		{
		// Singleton pattern to ensure only one instance exists
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

		// Populate matchups and determine best pairing
		PopulateMatchupList(team1Players, team2Players);

		// Ensure the panel is visible
		gameObject.SetActive(true);
		}

	/// <summary>
	/// Populates the list of matchups and identifies the best matchup.
	/// </summary>
	private void PopulateMatchupList(List<Player> team1Players, List<Player> team2Players)
		{
		// Clear previous entries
		foreach (Transform child in matchupListContainer) Destroy(child.gameObject);
		foreach (Transform child in bestMatchupListContainer) Destroy(child.gameObject);

		GameObject bestMatchupEntry = null;
		float highestWinChance = 0;

		foreach (var player1 in team1Players)
			{
			foreach (var player2 in team2Players)
				{
				float matchupOdds = HandicapSystem.CalculateWinProbability(new List<Player> { player1 }, new List<Player> { player2 });

				// Create a matchup entry in the UI
				GameObject matchupEntry = Instantiate(matchupEntryPrefab, matchupListContainer);
				matchupEntry.GetComponent<TMP_Text>().text = $"{player1.PlayerName} vs {player2.PlayerName}: {matchupOdds * 100:F1}%";

				// Find the best matchup
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
			Instantiate(bestMatchupEntry, bestMatchupListContainer);
			Debug.Log($"Best matchup identified: {bestMatchupEntry.GetComponent<TMP_Text>().text}");
			}
		}
	}
